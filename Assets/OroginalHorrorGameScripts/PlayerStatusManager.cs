using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// �v���C���[�̃X�e�[�^�X�Ǘ��N���X
/// </summary>
public class PlayerStatusManager : MonoBehaviour
{
    [SerializeField] public float _playerMaxHealth = 100;
    [SerializeField] public float _playerCurrentHealth = 0;
    [SerializeField] GameObject _playerGamepadController;
    GamePadVibrationControllerSystem _vibrationControllerSystem;

    private void Awake()
    {
        this._playerCurrentHealth = this._playerMaxHealth;//�̗͂̏�����
    }

    private void Update()
    {
        if (this._playerCurrentHealth <= 0)
        {
            Debug.Log("LoadStart");
            StartCoroutine(LoadScene());
        }
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

    IEnumerator LoadScene()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync("FirstStage");
        while (!async.isDone)
        {
            if (this._playerGamepadController != null)
            {
                this._vibrationControllerSystem = this._playerGamepadController.GetComponent<GamePadVibrationControllerSystem>();
                this._vibrationControllerSystem.StopGamepadViverate();
            }
            yield return null;
        }
    }
}
