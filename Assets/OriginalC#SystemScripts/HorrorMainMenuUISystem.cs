using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
/// <summary>
/// Unity UIToolkit�Ή��ł̃N���X 2023/06/06
/// </summary>
public class HorrorMainMenuUISystem : MonoBehaviour
{
    VisualTreeAsset _elementTemplate;
    UIDocument _document;

    private  UnityEngine.UIElements.Button _startButton, _settingsButton, _quitButton, _backToMainMenu;

    private  UnityEngine.UIElements.GroupBox _groupBoxMain, _groupBoxSettings, _groupBoxLoadingScene;

    private  UnityEngine.UIElements.ProgressBar _sceneLoadingProgressBar;

    private bool _calledNextScene = false;

    private void OnEnable()
    {
        if (GetComponent<UIDocument>() != null)//UIDocumen���擾�o�����炻�̂܂܎擾
        {
            _document = GetComponent<UIDocument>();
        }
    }

    private void Start()
    {
        GroupBoxController(_document.rootVisualElement);
    }

    private void Update()
    {
        ButtonInputChecker(_document.rootVisualElement);
    }

    public void ButtonInputChecker(VisualElement root)//Button�̐錾�������܂��Ȃ���
    {
        {                                   /*GroupBox_All_Main*/
            this._startButton = root.Q<UnityEngine.UIElements.Button>("StartGame_Button");//�n�߂�{�^��
            this._startButton.clicked += StartButtonClicked;

            this._settingsButton = root.Q<UnityEngine.UIElements.Button>("Settings_Button");//�ݒ�{�^��
            this._settingsButton.clicked += SettingButtonClicked;

            this._quitButton = root.Q<UnityEngine.UIElements.Button>("Quit_Button");//OS�ɖ߂�{�^��
            this._quitButton.clicked += QuitButtonClicked;

            this._backToMainMenu = root.Q<UnityEngine.UIElements.Button>("Setting_BackToMainButton");//���C�����j���[�ɖ߂�{�^��
            this._backToMainMenu.clicked += BackToMainMenuClicked;
        }                                   /*GroupBox_All_Main*/
    }

    private void StartButtonClicked()//�n�߂�{�^��
    {
        //throw new System.NotImplementedException();
        Debug.Log(_startButton.text);
        MoveToGameSceneFirst(this._document.rootVisualElement);
    }

    private void SettingButtonClicked()//�ݒ�{�^��
    {
        Debug.Log(_settingsButton.text);
        SelectSettingsGroupBox();
    }

    private void QuitButtonClicked()//OS�ɖ߂�{�^��
    {
        Debug.Log(_quitButton.text);
    }

    private void BackToMainMenuClicked()//���C�����j���[�ɖ߂�{�^��
    {
        Debug.Log(_backToMainMenu.text);
        SelectMainGroupBox();
    }


    public void GroupBoxController(VisualElement root)
    {
        this._groupBoxMain = root.Q<UnityEngine.UIElements.GroupBox>("GroupBox_All_Main");//���C�����j���[��ʂ̃O���{�b�N�X
        this._groupBoxMain.visible = true;

        this._groupBoxSettings = root.Q<UnityEngine.UIElements.GroupBox>("GroupBox_All_Settings");//�e��ݒ��ʂ̃O���{�b�N�X
        this._groupBoxSettings.visible = false;

        this._groupBoxLoadingScene = root.Q<UnityEngine.UIElements.GroupBox>("GroupBox_All_SceneLoading");//�V�[�����[�h��ʂ̃O���{�b�N�X
        this._groupBoxLoadingScene.visible = false;
    }

    public void SelectMainGroupBox()//���C�����j���[��ʂ̃O���{�b�N�X
    {
        this._groupBoxMain.visible = true;
        this._groupBoxSettings.visible = false;
        this._groupBoxLoadingScene.visible = false;
    }

    public void SelectSettingsGroupBox()//�e��ݒ��ʂ̃O���{�b�N�X
    {
        this._groupBoxSettings.visible = true;
        this._groupBoxMain.visible = false;
        this._groupBoxLoadingScene.visible = false;
    }

    public void MoveToGameSceneFirst(VisualElement root)
    {
        this._sceneLoadingProgressBar = root.Q<UnityEngine.UIElements.ProgressBar>("LoadingScene_Progress");
        this._sceneLoadingProgressBar.value = 0;//�v���O�o�[�l������
        this._sceneLoadingProgressBar.visible = true;
        this._groupBoxMain.visible = false;
        this._groupBoxSettings.visible = false;
        if(!this._calledNextScene)
            StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        this._calledNextScene = true;
        AsyncOperation async = SceneManager.LoadSceneAsync("FirstStage");
        while (!async.isDone)
        {
            this._sceneLoadingProgressBar.value = async.progress;
            yield return null;
        }
    }
}