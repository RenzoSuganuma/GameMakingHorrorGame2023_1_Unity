using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
/// <summary>
/// Unity UIToolkit�Ή��ł̃N���X 2023/06/06
/// </summary>
public class UISystemForHorrorGame : MonoBehaviour
{
    /// <summary> �����K�w��UIDocument </summary>
    UIDocument _document;

    /// <summary> HP�̃v���O�o�[�̃N���X </summary>
    public HPProgressBarController _hpProgBarController;

    /// <summary> �A�C�e�����\���̃N���X </summary>
    public ItemTextController _itemTextController;

    /// <summary> �E�����A�C�e�����\���̃N���X </summary>
    public ItemPickedTextController _itemPickedTextController;

    private void Start()
    {
        if (GetComponent<UIDocument>() != null)//UIDocumen���擾�o�����炻�̂܂܎擾
        {
            _document = GetComponent<UIDocument>();
            //public �̃N���X�ɑ��
            this._hpProgBarController = new HPProgressBarController(_document.rootVisualElement);
            this._itemTextController = new ItemTextController(_document.rootVisualElement);
            this._itemPickedTextController = new ItemPickedTextController(_document.rootVisualElement);
        }
    }

    private void Update()
    {
        
    }
}

public sealed class HPProgressBarController
{
    private UnityEngine.UIElements.ProgressBar _progressBar;

    public HPProgressBarController(VisualElement root)
    {
        this._progressBar = root.Q<UnityEngine.UIElements.ProgressBar>("HPBar");//()���̕������Name�Ńo�C���h����Ă��镶����
        //�l�̏�����
        this._progressBar.lowValue = 0f;
        this._progressBar.highValue = 100f;
    }

    /// <summary>
    /// �Q�[����ʂɕ\������镶����̃��f�B�t�@�C
    /// </summary>
    /// <param name="title"></param>
    public void ModifyTitle(string title)
    {
        this._progressBar.title = title;
    }

    /// <summary>
    /// �v���O���X�o�[�̒l�̃��f�B�t�@�C
    /// </summary>
    /// <param name="deltaValue"></param>
    public void ModifyProgressValue(float deltaValue)
    {
        this._progressBar.value += deltaValue;
    }
}

public class ItemTextController
{
    private UnityEngine.UIElements.Label _label;
    
    public ItemTextController(VisualElement root)
    {
        this._label = root.Q<UnityEngine.UIElements.Label>("ItemText");//()���̕������Name�Ńo�C���h����Ă��镶����
        //�l�̏�����
        this._label.text = string.Empty;
    }

    public void OutPutTextToDisplay(string title)
    {
        this._label.text = title;
    }
}

public class ItemPickedTextController
{
    private UnityEngine.UIElements.Label _label;
    public ItemPickedTextController(VisualElement root)
    {
        this._label = root.Q<UnityEngine.UIElements.Label>("ItemPickedText");//()���̕������Name�Ńo�C���h����Ă��镶����
        //�l�̏�����
        this._label.text = string.Empty;
    }

    public void OutPutTextToDisplay(string title)
    {
        this._label.text = title;
    }
}