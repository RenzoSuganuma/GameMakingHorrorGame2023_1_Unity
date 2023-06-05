using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// �G�L��������p�N���X ver - alpha
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class SpectorController : MonoBehaviour
{
    /// <summary> �G�I�u�W�F�N�g����p��Rigidbody </summary>
    Rigidbody _rb;

    /// <summary> �v���C���[�I�u�W�F�N�g </summary>
    GameObject _playerGameObject = null;

    [SerializeField] private bool _isMadNow = true;

    private void Start()
    {
        this._rb = GetComponent<Rigidbody>();//Rigidbody�̎擾
        this._playerGameObject = GameObject.FindGameObjectWithTag("PlayerCamera");//�v���C���[�I�u�W�F�N�g�̌���

        { this._rb.useGravity = false; }//Rigidbody������
    }

    private void FixedUpdate()
    {
        this.gameObject.transform.LookAt(this._playerGameObject.transform.position, this._playerGameObject.transform.up);//�v���C���[�I�u�W�F�N�g������
        if(this._isMadNow)
            this._rb.AddForce(this.transform.forward * .3f, ForceMode.VelocityChange);//���ʂɈړ�
        else
            this._rb.velocity = this.transform.forward * .6f;//���ʂɈړ�
        //Debug.Log(_isMadNow);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Collided WITH Player");
            Destroy(this.gameObject);
        }
    }
}
