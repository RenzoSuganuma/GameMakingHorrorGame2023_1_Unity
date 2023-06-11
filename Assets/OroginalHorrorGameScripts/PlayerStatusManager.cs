using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �v���C���[�̃X�e�[�^�X�Ǘ��N���X
/// </summary>
public class PlayerStatusManager : MonoBehaviour
{
    [SerializeField] public float _playerMaxHealth = 100;
    [SerializeField] public float _playerCurrentHealth = 0;

    private void Awake()
    {
        this._playerCurrentHealth = this._playerMaxHealth;//�̗͂̏�����
    }

    /// <summary>
    /// +-�ő̗͂̏C�����ł���
    /// </summary>
    /// <param name="health"></param>
    public void ModifyHealth(float health)
    {
        this._playerCurrentHealth += health;
    }

    /// <summary>
    /// ���݂̗̑͒l��Ԃ�
    /// </summary>
    /// <returns></returns>
    public float GetCurrentHealth()
    {
        return this._playerCurrentHealth;
    }
}
