using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SearchService;

public class BossWarpToPlayerTrap : MonoBehaviour
{
    //���[�v��̃g�����X�t�H�[��
    [SerializeField] private Transform _targetTransform;
    //���[�v����I�u�W�F�N�g
    [SerializeField] GameObject _bossObject;

    private void OnTriggerEnter(Collider other)
    {
        //�v���C���[���g���b�v�̃R���C�_�[�i�g���K�[�j�ƐڐG�����Ƃ�
        if (other.gameObject.CompareTag("Player") && other.gameObject != null)
        {
            this._bossObject.SetActive(false);
            this._bossObject.transform.position = this._targetTransform.position;
            this._bossObject.SetActive(true);

            Destroy(this.gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(this.transform.position, 5);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(this._targetTransform.position, Vector3.one);
    }
}
