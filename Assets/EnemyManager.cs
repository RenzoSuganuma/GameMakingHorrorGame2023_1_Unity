using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���̃N���X�͓G�I�u�W�F�N�g�̎��̂ɃA�^�b�`���Ȃ����ƁB�G�L�����}�l�[�W���[�Ƃ��ċ@�\ ver - alpha
/// </summary>

public class EnemyManager : MonoBehaviour
{
    /// <summary> �v���C���[����p�̃N���X </summary>
    PlayerController _playerController;

    /// <summary> �v���C���[�������d�������Ă邩�̃t���O </summary>
    private bool _playerIsLighting = false;

    /// <summary> �G�̃I�u�W�F�N�g </summary>
    [SerializeField] GameObject _enemyObject = null;

    private void Start()
    {
        if(GameObject.FindGameObjectWithTag("Player").TryGetComponent<PlayerController>(out PlayerController playerController))//�v���C���[�I�u�W�F�N�g�̌���
            _playerController = playerController;
    }

    // Update is called once per frame
    private void Update()
    {
        _playerIsLighting = _playerController._flashLightIsOn;//�v���C���[����p�̃N���X����̉����d���̃t���O�̎�M
        if (_playerIsLighting)//�����d�������Ă��邤���ɂ͂��̃I�u�W�F�N�g�͖���
            _enemyObject.SetActive(false);
        else
            _enemyObject.SetActive(true);

        //print($"{_playerIsLighting} : is player lighting status");
    }
}
