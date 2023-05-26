using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    /// <summary> InputSystem �� PlayerInput�R���|�[�l���g </summary>
    PlayerInput _playerInput;

    /// <summary> �L�����ړ��Ɏg�� </summary>
    Rigidbody _rigidbody;

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

    GameObject _playerCamera;


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

        { _flashLight = GameObject.FindGameObjectWithTag("FlashLight"); }//�����d���̃I�u�W�F�N�g�̌���
        if (_flashLight.TryGetComponent<Light>(out Light light))//Light�R���|�[�l���g�̌���
            _light = light;
        else
            Debug.LogError("The Component Light Is Not Found");

        { _playerCamera = GameObject.FindGameObjectWithTag("PlayerCamera"); }

        new MouseCursoreLocker();//�}�E�X�J�[�\���̃��b�N
    }

    private void FixedUpdate()
    {
        new PlayerMover(this.gameObject, this._rigidbody, this._moveVector, this._moveSpeed);//�ړ�
        new PlayerLooker(this.gameObject.transform, this._lookVector, this._lookSpeed);//�U�����
        new FlushLightController(this._light, this._illuminate);//�����d����ONOFF
        new PlayerCameraController(this._playerCamera, this._lookVector, this._lookSpeed, 30f);//�J�����㉺��]
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
    public PlayerMover(GameObject playerObject, Rigidbody rigidbody, Vector2 moveVector, float moveSpeed)
    {
        if (moveVector != Vector2.zero)
        {
            rigidbody.WakeUp();//��������̓��͒l���������͂�������
            rigidbody.AddForce(playerObject.transform.forward * moveVector.y *  moveSpeed * .01f, ForceMode.Impulse);//�O��̈ړ��@���ʂ��ϓ�����̂ł��̎��X�̐��ʊ�œ���
            rigidbody.AddForce(playerObject.transform.right * moveVector.x *  moveSpeed * .01f, ForceMode.Impulse);//���E�̈ړ��@���ʂ��ϓ�����̂ł��̎��X�̐��ʊ�œ���
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
    public PlayerLooker(Transform transform, Vector2 lookVector, float lookSpeed)
    {
        transform.Rotate(new Vector3(0, lookVector.x * lookSpeed * .5f, 0));
    }
}

/// <summary>
/// �}�E�X�J�[�\���Œ�p�N���X
/// </summary>
public class MouseCursoreLocker
{
    public MouseCursoreLocker()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
}

/// <summary>
/// �����d������N���X
/// </summary>
public class FlushLightController
{
    public FlushLightController(Light light, bool lightOrnot)
    {
        light.enabled = lightOrnot;
    }
}

/// <summary>
/// �J�����̐���N���X
/// </summary>
public class PlayerCameraController
{
    private float _minLimit = 0f;
    private Vector3 _localAngle = Vector3.zero;
    public PlayerCameraController(GameObject gameObject, Vector2 lookVector, float lookSpeed, float maxLimit)
    {
        //������Ɍ������ĉ�]�����̐��@�������Ɍ������ĉ�]�����̐� -180[Deg] ~ 180[Deg]
        _minLimit = 360 - maxLimit;
        _localAngle = gameObject.transform.localEulerAngles;
        _localAngle.x += -lookVector.y * lookSpeed * .1f;
        if (_localAngle.x > maxLimit && _localAngle.x < 180)//maxLimit[Deg] ~ 180[Deg]
        {
            Debug.Log("MaxLimit!");
            _localAngle.x = maxLimit;
        }

        if (_localAngle.x < _minLimit && _localAngle.x > 180)//minLimit[Deg] ~ 180[Deg]
        {
            Debug.Log("MinLimit!");
            _localAngle.x = _minLimit;
        }

        gameObject.transform.localEulerAngles = _localAngle;
    }
}