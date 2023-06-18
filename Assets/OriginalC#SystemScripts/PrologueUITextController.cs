using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
/// <summary>
/// Unity UIToolkit�Ή��ł̃N���X 2023/06/06
/// </summary>
public class PrologueUITextController : MonoBehaviour
{
    /// <summary> �����K�w��UIDocument </summary>
    UIDocument _uiDocument;

    /// <summary>
    /// �o�͎Q�Ɛ惉�x��
    /// </summary>
    private UnityEngine.UIElements.Label _label;

    /// <summary>
    /// �o�͂���e�L�X�g���i�[���Ă���e�L�X�g�A�Z�b�g _�ŃX�v���b�g���Ĕz�񉻁A�e�z��v�f���Ɖ�ʏo��
    /// </summary>
    [SerializeField] TextAsset _textAsset;

    /// <summary>
    /// �o�͐惉�x���̃N���X
    /// </summary>
    TextController _textController;

    /// <summary>
    /// �X�L�b�v����{�^���̃N���X
    /// </summary>
    SkipPrologueButtonChecker _skipPrologueButtonChecker;

    /// <summary>
    /// �Q�[���p�b�h�̐U���R���g���[���̃N���X
    /// </summary>
    GamePadVibrationControllerSystem _gamePadVibrationControllerSystem;

    /// <summary>
    /// �o�͂��镶����̔z��
    /// </summary>
    private string[] _outPutTextArray = null;

    private void Awake()
    {
        if (GetComponent<UIDocument>() != null)//UIDocumen���擾�o�����炻�̂܂܎擾
        {
            this._uiDocument = GetComponent<UIDocument>();
            this._textController = new TextController(_uiDocument.rootVisualElement);
            this._skipPrologueButtonChecker = new SkipPrologueButtonChecker(_uiDocument.rootVisualElement);

            this._outPutTextArray = this._textAsset.text.Split("_");//�X�v���b�g
            //this._textController.UpdateText(this._outPutTextArray[0]);//�A�E�g�v�b�g
            Debug.Log($"array length {this._outPutTextArray.Length}");
        }

        if (GetComponent<GamePadVibrationControllerSystem>() != null)//�Q�[���p�b�h�̐U���R���g���[���[���擾�o�����炻�̂܂܎擾
        {
            this._gamePadVibrationControllerSystem = GetComponent<GamePadVibrationControllerSystem>();
        }
    }

    private void Start()
    {
        StartCoroutine(OutPutTextRoutine(_outPutTextArray, _outPutTextArray.Length));
    }

    IEnumerator OutPutTextRoutine(string[] strArray, int arrayLength)
    {
        for (int i = 0; i < arrayLength; i++)
        {
            this._textController.UpdateText(this._outPutTextArray[i],this._gamePadVibrationControllerSystem);//�A�E�g�v�b�g
            yield return new WaitForSeconds(3);
        }
        yield return null;
        StartCoroutine(LoadScene());
        Debug.Log($"Output END");
    }

    IEnumerator LoadScene()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync("StartMenu");
        while (!async.isDone)
        {
            yield return null;
        }
    }
}

public class TextController
{
    private UnityEngine.UIElements.Label _label, _emphasisLabel;
    public TextController(VisualElement root)
    {
        this._label = root.Q<UnityEngine.UIElements.Label>("Prologue_Label");
        this._emphasisLabel = root.Q<UnityEngine.UIElements.Label>("Prologue_Label_Emphasis");

        this._label.text = string.Empty;//�ʏ�
        this._emphasisLabel.text = string.Empty;//����
        this._emphasisLabel.visible = false;//���
    }

    public void UpdateText(string text)
    {
        if (text.StartsWith("|"))
        {
            this._emphasisLabel.visible = true;//����
            this._label.visible = false;//���
            this._emphasisLabel.text = text.Replace("|", "");//�o��
        }
        else
        {
            this._label.visible = true;//����
            this._emphasisLabel.visible = false;//���
            this._label.text = text;//�o��
        }
    }

    public void UpdateText(string text, GamePadVibrationControllerSystem gamePadVibrationControllerSystem)
    {
        if (text.StartsWith("|"))//�����\���̎�
        {
            this._emphasisLabel.visible = true;//����
            this._label.visible = false;//���
            this._emphasisLabel.text = text.Replace("|", "");//�o��
        }
        else if (text.StartsWith("^"))//�U���\�����������Ƃ��F
        {
            text = text.Replace("^", "");//^�������ċl�߂�
            gamePadVibrationControllerSystem.GamepadViverateRapid(30, 2);//�Q�[���p�b�h�U��

            this._label.visible = true;//����
            this._emphasisLabel.visible = false;//���
            this._label.text = text;//�o��
        }
        else//���̂ǂ���ł��Ȃ��ʏ�\���̎�
        {
            this._label.visible = true;//����
            this._emphasisLabel.visible = false;//���
            this._label.text = text;//�o��
        }
    }
}

public sealed class SkipPrologueButtonChecker
{
    private readonly UnityEngine.UIElements.Button _button;
    private bool _calledTitleScene = false;

    public SkipPrologueButtonChecker(VisualElement root)//Button�̐錾�������܂��Ȃ���
    {
        _button = root.Q<UnityEngine.UIElements.Button>("Skip_Button");
        _button.clicked += buttonClicked;
    }

    private void buttonClicked()
    {
        Debug.Log(_button.text);
        BackToTitleClickedNow();
    }

    private void BackToTitleClickedNow()
    {
        if (!this._calledTitleScene)
        {
            SceneManager.LoadScene("StartMenu");
        }
    }
}