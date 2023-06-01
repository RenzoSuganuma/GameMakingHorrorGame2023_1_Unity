using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// �G�L��������p�N���X ver - alpha
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]

public class SpectorController : MonoBehaviour
{
    /// <summary> �G�I�u�W�F�N�g����p��Rigidbody </summary>
    Rigidbody _rb;

    /// <summary> �v���C���[�I�u�W�F�N�g </summary>
    GameObject _playerGameObject = null;

    /// <summary> �G�I�u�W�F�N�g����p��CapsuleCollider </summary>
    CapsuleCollider _capsuleCollider;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();//Rigidbody�̎擾
        _capsuleCollider = GetComponent<CapsuleCollider>();//CapsuleCollider�̎擾
        _playerGameObject = GameObject.FindGameObjectWithTag("Player");//�v���C���[�I�u�W�F�N�g�̌���

        { _rb.useGravity = false; }//Rigidbody������
        { _capsuleCollider.isTrigger = true; }//CapsuleCollider������
    }

    private void FixedUpdate()
    {
        this.gameObject.transform.LookAt(_playerGameObject.transform.position, _playerGameObject.transform.up);//�v���C���[�I�u�W�F�N�g������
        _rb.velocity = this.transform.forward * .5f;//���ʂɈړ�
    }
}
