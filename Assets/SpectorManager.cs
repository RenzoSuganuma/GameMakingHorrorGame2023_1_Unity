using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// ���̃N���X�͓G�I�u�W�F�N�g�̎��̂ɃA�^�b�`���Ȃ����ƁB�G�L�����}�l�[�W���[�Ƃ��ċ@�\ ver - alpha
/// </summary>

public class SpectorManager : MonoBehaviour
{
    /// <summary> �v���C���[�������d�������Ă邩�̃t���O </summary>
    private bool _playerIsLighting = false;

    /// <summary> �G�̃I�u�W�F�N�g�z�� </summary>/// 
    [SerializeField] GameObject[] _enemyObject = null;
    /// <summary> �X�}�z�̉����d���̃I�u�W�F�N�g </summary>
    [SerializeField] GameObject _playerFlashLight = null;

    private void Start()
    {
        this._playerFlashLight = GameObject.FindGameObjectWithTag("FlashLight");
        this._enemyObject = GameObject.FindGameObjectsWithTag("Spector_Enemy");//Spector�QEnemy�̃^�O�̕R�Â�����Ă���G���ׂČ���
    }

    // Update is called once per frame
    private void Update()
    {
        this._playerIsLighting = this._playerFlashLight.GetComponent<Light>().enabled;//�v���C���[����p�̃N���X����̉����d����Light�R���|�[�l���g��OnOFF���Ď��A�R���f�B�V�����̎擾
        if (_playerIsLighting)//�����d�������Ă��邤���ɂ͂��̃I�u�W�F�N�g�͖���
        {
            if (_enemyObject != null)//null�`�F�b�N
            {
                foreach (GameObject obj in _enemyObject)
                {
                    if (obj != null)//null�`�F�b�N���Ă���A�N�e�B�u��Ԃ̐؂�ւ�
                        obj.SetActive(false);
                }
            }
        }
        else
        {
            if(_enemyObject != null)
            {
                foreach (GameObject obj in _enemyObject)
                {
                    if(obj !=  null)//null�`�F�b�N���Ă���A�N�e�B�u��Ԃ̐؂�ւ�
                        obj.SetActive(true);
                }
            }
        } 
        //print($"{_playerIsLighting} : is player lighting status");
    }
}
