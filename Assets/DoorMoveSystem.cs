using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �j�Q�����i�C �̃h�A���󂯂邽�߂Ɏg���N���X �g���̂āB�����܂킵NG
/// </summary>
public class DoorMoveSystem : MonoBehaviour
{
    [SerializeField] GameObject _doorObjectOpened, _doorObjectClosed;

    public bool _DoorIsOpened = false;

    private void Awake()
    {
        this._doorObjectOpened.SetActive(false);
    }

    private void Update()
    {
        Debug.Log($"Door Transform Is {this._doorObjectOpened.transform.localPosition}");
        InteractDoor(this._DoorIsOpened);
    }
    
    /// <summary>
    /// �󂯂���߂邱�Ƃ��ł��Ȃ��Ȃ�d�l�ł�
    /// </summary>
    /// <param name="isOpen"></param>
    public void InteractDoor(bool isOpen)
    {
        if(isOpen)
        {
            this._doorObjectOpened.SetActive(true);
            this._doorObjectClosed.SetActive(false);
            if (TryGetComponent<Collider>(out Collider component))//�A�^�b�`����Ă�Colllider�R���|�[�l���g�����ׂĔj���B
            {
                Collider[] components = GetComponents<Collider>();
                foreach(Collider c in components)
                {
                    Destroy(c);
                }
            }
        }
    }
    
}
