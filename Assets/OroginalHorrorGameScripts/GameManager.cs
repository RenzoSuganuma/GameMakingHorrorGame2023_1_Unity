using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// �v���C���[�̃X�e�[�^�X�Ǘ��N���X
/// </summary>
public class GameManager : MonoBehaviour
{
    //�v���C���[
    [SerializeField] public float _playerMaxHealth = 100;
    [SerializeField] public float _playerCurrentHealth = 0;
    [SerializeField] GameObject _playerGamepadController;
    [SerializeField] Transform _playerRestartposition;
    GameObject _playerObject;

    //�G�L�����iAI�j
    [SerializeField] GameObject _enemyBossObject;
    [SerializeField] Transform[] _enemyBossSpawnPoint;
    HorrorMonsterController _monsterController;

    //�f�o�C�X
    GamePadVibrationControllerSystem _vibrationControllerSystem;

    //����
    [SerializeField] GameObject _guideLight;

    private void Awake()
    {
        this._playerCurrentHealth = this._playerMaxHealth;//�̗͂̏�����
        //�v���C���[�̌���
        this._playerObject = GameObject.FindGameObjectWithTag("Player");
        //�G�{�X����
        this._enemyBossObject = GameObject.FindGameObjectWithTag("SpectorBoss_Enemy");
        if (this._enemyBossObject.TryGetComponent<HorrorMonsterController>(out HorrorMonsterController horrorMonster))
        {
            this._monsterController = horrorMonster;
        }
        //�Q�[���p�b�h����N���X�̎擾
        this._vibrationControllerSystem = this._playerGamepadController.GetComponent<GamePadVibrationControllerSystem>();
        //�����̌����ŏ��͔�\���ɂ���
        //this._guideLight.SetActive(false);
    }

    private void Update()
    {
        #region  �v���C���[���{�X�ɕ⑫����Ă鎞�̑���

        if (this._monsterController._isChasing)
        {
            Debug.Log("BOSS IS CHASING!");
            this._vibrationControllerSystem.GamepadViverate(180);

            //�v���C���[�̌���
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                if (player.TryGetComponent<PlayerController>(out PlayerController controller))
                {
                    controller._playerObjective = "������I";
                }
            }
        }
        else
        {
            //�v���C���[�̌���
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                if (player.TryGetComponent<PlayerController>(out PlayerController controller))
                {
                    controller._playerObjective = "�d�r���W���c�c\n�o���g�c�i�K�b�e���L�������P";
                }
            }
        }
        #endregion

        if (this._playerCurrentHealth <= 0)
        {
            #region  �Q�[���p�b�h�̐U�����~�߂�
            if (this._playerGamepadController != null)//�Q�[���p�b�h�̐U�����~�߂�
            {
                this._vibrationControllerSystem = this._playerGamepadController.GetComponent<GamePadVibrationControllerSystem>();
                this._vibrationControllerSystem.StopGamepadViverate();
            }
            #endregion

            #region  �{�X�̍ăX�|�[��
            this._enemyBossObject.SetActive(false);
            this._enemyBossObject.transform.position = 
                this._enemyBossSpawnPoint[Random.Range(0, this._enemyBossSpawnPoint.Length)/*�����_���ȓG�{�X�̃X�|�[���n�_����*/].position;
            this._enemyBossObject.SetActive(true);
            #endregion

            Debug.Log("LoadStart");

            #region  �v���C���[�̍ăX�|�[��
            this._playerObject.SetActive(false);
            this._playerCurrentHealth = 100;
            this._playerObject.transform.position = this._playerRestartposition.transform.position;
            this._playerObject.SetActive(true);
            #endregion

            //�����̌�����񎀂񂾂�\��
            this._guideLight.SetActive(true);

            #region  �v���C���[�̃X�^�~�i�𑝂₵�ă`�F�C�X���₷������
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                if (player.TryGetComponent<PlayerController>(out PlayerController controller))
                {
                    controller._stamina = 100f;
                }
            }
            #endregion

            //StartCoroutine(LoadScene());
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;

        foreach (Transform t in this._enemyBossSpawnPoint)
        {
            Gizmos.DrawLine(t.position, t.position + Vector3.up * 10);
        }
    }
}
