using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �⌾�̊i�[�N���X 2023/06/06
/// </summary>
public class DiyingWillTextContainer : MonoBehaviour
{
    [SerializeField] private TextAsset _textAsset;
    public string _text;
    public bool _showText = false;

    private void Awake()
    {
        this._text = this._textAsset.text;
    }

    public string GetText()
    {
        return this._text;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.gameObject.transform.position, 2);
    }
}
