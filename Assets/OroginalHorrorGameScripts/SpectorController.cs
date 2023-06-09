using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// �G�L��������p�N���X ver - alpha 2023/06/06
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class SpectorController : MonoBehaviour
{
    /// <summary> �G�I�u�W�F�N�g����p��Rigidbody </summary>
    Rigidbody _rb;

    /// <summary> �v���C���[�I�u�W�F�N�g </summary>
    GameObject _playerGameObject = null;

    /// <summary> �S�삪�{���Ԃ� </summary>
    [SerializeField] private bool _isMadNow = true;

    /// <summary> �S����Ŏ���VFX </summary>
    [SerializeField] private GameObject _deathEffect = null;

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
        if (other.gameObject.CompareTag("Player"))//Player�ƂԂ�������
        {
            Debug.Log("Collided WITH Player");
            if (this._deathEffect != null)//VFX���X�N���v�g�ɃA�^�b�`����Ă���
            {
                //this._deathEffect.SetActive(true);
                GameObject effect = Instantiate(this._deathEffect);//����
                effect.transform.position = this.gameObject.transform.position;//�Ԃ������ʒu�Ƀ|�W�V�������C��
                effect.gameObject.name = effect.gameObject.name.Replace("(Clone)", "");//���O��(Clone)���폜����
            }
            Destroy(this.gameObject);//���̃I�u�W�F�N�g�̔j��
        }
    }
}
