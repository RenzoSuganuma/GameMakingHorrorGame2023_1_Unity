using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent (typeof(PlayerInput))]
[RequireComponent (typeof(CapsuleCollider))]
/// <summary>
/// �v���C���[����p�̃N���X ver - alpha
/// </summary>
/// 
public class PlayerController : MonoBehaviour
{
    /// <summary> InputSystem �� PlayerInput�R���|�[�l���g </summary>
    PlayerInput _playerInput;

    /// <summary> �L�����ړ��Ɏg�� </summary>
    Rigidbody _rigidbody;

    /// <summary> �L�����ړ��Ɏg���R���C�_�[ </summary>
    CapsuleCollider _capsuleCollider;

    /// <summary> �ړ��x�N�g�����͒l�̑���� </summary>
    Vector2 _moveVector = Vector2.zero;
    /// <summary> �U������x�N�g�����͒l�̑���� </summary>
    Vector2 _lookVector = Vector2.zero;

    /// <summary> ���ꂪ�^�̒l�̎��ɂ͉����d���͂��Ă���悤�ɂ��� </summary>
    private bool _illuminate = !true;
    /// <summary> Light�R���|�[�l���g�@�����ł̓v���C���[�̉����d���̃R���|�[�l���g </summary>
    Light _light;
    /// <summary> �v���C���[�̉����d���̃I�u�W�F�N�g </summary>
    GameObject _flashLight;

    /// <summary> �ړ����xfloat�^���� </summary>
    [Tooltip("�ړ����xfloat�^����")]
    [SerializeField] private float _moveSpeed = 1.0f;
    /// <summary> �ړ����xfloat�^���� </summary>
    [Tooltip("�U��������xfloat�^����")]
    [SerializeField] private float _lookSpeed = 1.0f;

    /// <summary> �v���C���[�̃J�����̃I�u�W�F�N�g </summary>
    GameObject _playerCamera;

    /// <summary> �����d�������Ă邩����t���O : �G�I�u�W�F�N�g�̗L���������f�p �ǂݍ��ݐ�p </summary>
    public bool _flashLightIsOn = true;

    /// <summary> SE�p�̃I�u�W�F�N�g�̃^�O���R�Â�����Ă�I�u�W�F�N�g�i�[�p </summary>
    GameObject[] _compareTagSoundEffect;
    /// <summary> �v���C���[���s���̌��ʉ��Đ��p�̃I�u�W�F�N�g�i�[�p </summary>
    GameObject _walkingSoundEffectObject = null;

    PlayerMover _playerMover;
    PlayerLooker _playerLooker;
    MouseCursoreLocker _cursoreLocker;
    FlashLightController _flashLightController;
    PlayerCameraController _playerCameraController;
    WalkingSoundEffectController _walkingSoundEffectController;

    private void Start()
    {
        if (TryGetComponent<PlayerInput>(out PlayerInput player))//PlayerInput�R���|�[�l���g�����邩�`�F�b�N�A����Ȃ�Q�b�g����
        {
            _playerInput = player;//���肵���R���|�[�l���g�̑��M
            _playerInput.defaultActionMap = "Player";//DefaultMap�̏�����
        }
        else
            Debug.LogError("The Component PlayerInput Is Not Found");

        if (TryGetComponent<Rigidbody>(out Rigidbody rigid))//Digidbody�R���|�[�l���g�����邩�`�F�b�N�A����Ȃ�Q�b�g����
        {
            _rigidbody = rigid;//���肵���R���|�[�l���g�̑��M
            //�e�p�����[�^�[�̏�����
            _rigidbody.mass = 1.0f;
            _rigidbody.drag = 1.5f;
            _rigidbody.angularDrag = .05f;
            _rigidbody.useGravity = true;
            _rigidbody.isKinematic = !true;
            _rigidbody.freezeRotation = true;
        }
        else
            Debug.LogError("The Component Rigidbody Is Not Found");

        if (TryGetComponent<CapsuleCollider>(out CapsuleCollider capsule))//Digidbody�R���|�[�l���g�����邩�`�F�b�N�A����Ȃ�Q�b�g����
        {
            //�e�p�����[�^�[�̏�����
            _capsuleCollider = capsule;
            _capsuleCollider.radius = .3f;
            _capsuleCollider.height = 1.5f;
        }
        else
            Debug.LogError("The Component CapsuleCollider Is Not Found");


        { _flashLight = GameObject.FindGameObjectWithTag("FlashLight"); }//�����d���̃I�u�W�F�N�g�̌���
        if (_flashLight.TryGetComponent<Light>(out Light light))//Light�R���|�[�l���g�̌���
            _light = light;
        else
            Debug.LogError("The Component Light Is Not Found");


        { _playerCamera = GameObject.FindGameObjectWithTag("PlayerCamera"); }


        _compareTagSoundEffect = GameObject.FindGameObjectsWithTag("SoundEffect");//���ʉ��p�̃^�O���A�^�b�`����Ă���I�u�W�F�N�g�i�z��j������
        foreach (GameObject @object in _compareTagSoundEffect)//�{�g������g�b�v�܂ł̗v�f�̃`�F�b�N
        {
            if (@object.CompareTag("SoundEffect") && @object.name == "Walk_SE")//����̏����𖞂����I�u�W�F�N�g���������ꍇ
            {
                _walkingSoundEffectObject = @object; // �ϐ��ɃI�u�W�F�N�g���i�[
                break; // �����𖞂����I�u�W�F�N�g�����������烋�[�v���I��
            }
        }

        _cursoreLocker = new MouseCursoreLocker();//�}�E�X�J�[�\���̃��b�N�p�N���X�g�p
        _cursoreLocker.MouseCursoreLock();//�}�E�X�J�[�\���̃��b�N

        _flashLightController = new FlashLightController();
        _playerMover = new PlayerMover();
        _playerLooker = new PlayerLooker();
        _playerCameraController = new PlayerCameraController();
        _walkingSoundEffectController = new WalkingSoundEffectController();
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        { _flashLightIsOn = _illuminate; }//�ق��N���X��������d���̃X�e�[�^�X���X�R�[�v���邽��

        _playerMover.PlayerMove(this.gameObject, this._rigidbody, this._moveVector, this._moveSpeed);//�ړ�
        _playerLooker.PlayerLooking(this.gameObject.transform, this._lookVector, this._lookSpeed);//�U�����
        _flashLightController.FlushLightLight(this._light, this._illuminate);//�����d����ONOFF
        _playerCameraController.PlayerCameraMove(this._playerCamera, this._lookVector, this._lookSpeed, 30f);//�J�����㉺��]
        _walkingSoundEffectController.WalkingSoundEffectPlayStatusSet(this._walkingSoundEffectObject, this._moveVector);//���s���ʉ�����
    }

    /// <summary>
    /// PlayerInput����̃v���C���[�ړ��p��Vector2�̎��֐�
    /// </summary>
    /// <param name="context"></param>
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.ReadValue<Vector2>() != null)
        {
            _moveVector = context.ReadValue<Vector2>().normalized;//�x�N�g���̐��K�������ĒP�ʃx�N�g���ɕϊ������Ď΂߈ړ��̕s���R��������
            //print($"{_moveVector} : InputDevice - InputValue");//�f�o�b�O�p
        }
    }

    /// <summary>
    /// PlayerInput����̃v���C���[�U������p��Vector2�̎��֐�
    /// </summary>
    /// <param name="context"></param>
    public void OnLook(InputAction.CallbackContext context)
    {
        if (context.ReadValue<Vector2>() != null)
        {
            _lookVector= context.ReadValue<Vector2>().normalized;//�x�N�g���̐��K�������ĒP�ʃx�N�g���ɕϊ������Ď΂߂ӂ�����̕s���R��������
            //print($"{_lookVector} : InputDevice - InputValue : Look");//�f�o�b�O�p
        }
    }

    /// <summary>
    /// �����d����������͎��֐�
    /// </summary>
    /// <param name="context"></param>
    public void OnFire(InputAction.CallbackContext context)
    {
        _illuminate = context.ReadValueAsButton();//�}�E�X�{�^�����̃N���b�N�̏�Ԃ̊i�[
        //print($"{_illuminate} : is Illuminate");//�f�o�b�O�p
    }
}

/// <summary>
/// �L�����ړ��p�̃N���X
/// </summary>
public class PlayerMover
{
    public void PlayerMove(GameObject playerObject, Rigidbody rigidbody, Vector2 moveVector, float moveSpeed)
    {
        if (moveVector != Vector2.zero)
        {
            rigidbody.WakeUp();//��������̓��͒l���������͂�������
            rigidbody.AddForce(playerObject.transform.forward * moveVector.y * moveSpeed * .01f, ForceMode.Impulse);//�O��̈ړ��@���ʂ��ϓ�����̂ł��̎��X�̐��ʊ�œ���
            rigidbody.AddForce(playerObject.transform.right * moveVector.x * moveSpeed * .01f, ForceMode.Impulse);//���E�̈ړ��@���ʂ��ϓ�����̂ł��̎��X�̐��ʊ�œ���
        }
        else
        {
            rigidbody.Sleep();//�������͒l���Ȃ��̂ŗ͉͂����Ȃ�
        }
    }
}

/// <summary>
/// �v���C���[�U������ړ��p�N���X
/// </summary>
public class PlayerLooker
{
    public void PlayerLooking(Transform transform, Vector2 lookVector, float lookSpeed)
    {
        transform.Rotate(new Vector3(0, lookVector.x * lookSpeed * .5f, 0));
    }
}

/// <summary>
/// �}�E�X�J�[�\���Œ�p�N���X
/// </summary>
public class MouseCursoreLocker
{
    public void MouseCursoreLock()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
}

/// <summary>
/// �����d������N���X
/// </summary>
public class FlashLightController
{
    private static float _batteryLife = 10f;
    public void FlushLightLight(Light light, bool lightOrnot)
    {
        if (_batteryLife > 0)
        {
            light.enabled = lightOrnot;
            _batteryLife -= Time.deltaTime;
            if(lightOrnot) _batteryLife -= Time.deltaTime * 1.5f;
            Debug.Log($"current battery life : {_batteryLife}");
        }
        else
        {
            light.enabled = false;
            Debug.Log($"current battery is death");
        }
    }

    public void FlashLightBatteryCharge(float batteryPower)
    {
        _batteryLife = batteryPower;
        Debug.Log($"battery charged");
    }
}

/// <summary>
/// �J�����̐���N���X
/// </summary>
public class PlayerCameraController
{
    private float _minLimit = 0f;
    private Vector3 _localAngle = Vector3.zero;
    public void PlayerCameraMove(GameObject gameObject, Vector2 lookVector, float lookSpeed, float maxLimit)
    {
        //������Ɍ������ĉ�]�����̐��@�������Ɍ������ĉ�]�����̐� -180[Deg] ~ 180[Deg]
        _minLimit = 360 - maxLimit;
        _localAngle = gameObject.transform.localEulerAngles;
        _localAngle.x += -lookVector.y * lookSpeed * .1f;
        if (_localAngle.x > maxLimit && _localAngle.x < 180)//maxLimit[Deg] ~ 180[Deg]
        {
            //Debug.Log("MaxLimit!");
            _localAngle.x = maxLimit;
        }

        if (_localAngle.x < _minLimit && _localAngle.x > 180)//minLimit[Deg] ~ 180[Deg]
        {
            //Debug.Log("MinLimit!");
            _localAngle.x = _minLimit;
        }

        gameObject.transform.localEulerAngles = _localAngle;
    }
}

/// <summary>
/// �v���C���[���s����SE�Đ��p�N���X
/// </summary>
public class WalkingSoundEffectController
{
    public void WalkingSoundEffectPlayStatusSet(GameObject gameObject,Vector2 moveVector)
    {
        if(moveVector != Vector2.zero)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}

/// <summary>
/// �^�C�}�[�̃N���X
/// </summary>
public class Timer
{
    public float targetTime = 10.0f; // �ڕW���ԁi�b�j
    private float currentTime = 0.0f; // ���݂̌o�ߎ���

    private bool isTimerRunning = false; // �^�C�}�[�����쒆���ǂ���
    public bool isTimerPaused = false;

    public void UpdateTimer()
    {
        if (isTimerRunning)
        {
            currentTime += Time.deltaTime; // �o�ߎ��Ԃ��X�V
            Debug.Log($"CurrentTime : {currentTime}");
            if (currentTime >= targetTime)
            {
                // �ڕW���Ԃ��o�߂�����A���b�Z�[�W��\��
                Debug.Log("Time's up!");

                // �^�C�}�[���~����i�C�ӂ̏������s���ꍇ�͂����ɒǉ��j
                isTimerRunning = false;
                isTimerPaused = !isTimerRunning;//���J�ϐ��̒l�ݒ�
            }
        }
    }

    public void StartTimer()
    {
        // �^�C�}�[���J�n����i�C�ӂ̏������s���ꍇ�͂����ɒǉ��j
        isTimerRunning = true;
        isTimerPaused = !isTimerRunning;//���J�ϐ��̒l�ݒ�

        // �o�ߎ��Ԃ����Z�b�g
        currentTime = 0.0f;
    }
}