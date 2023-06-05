using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �⌾�̊i�[�N���X
/// </summary>
public class DiyingWillTextContainer : MonoBehaviour
{
    [SerializeField] private TextAsset _textAsset;
    [SerializeField] public string _text;

    private void Awake()
    {
        this._text = this._textAsset.text;
    }
}
