using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// �G�L��������N���X ver - alpha 2023/06/06
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class SpectorController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))//Player�ƂԂ�������
        {
            Destroy(this.gameObject);//���̃I�u�W�F�N�g�̔j��
        }
    }
}
