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

    /// <summary> �ړ����xfloat�^���� </summary>
    [Tooltip("�ړ����xfloat�^����")]
    [SerializeField] private float _moveSpeed = 1.0f;
    /// <summary> �ړ����xfloat�^���� </summary>
    [Tooltip("�U��������xfloat�^����")]
    [SerializeField] private float _lookSpeed = 1.0f;


    private void Start()
    {
        if (TryGetComponent<PlayerInput>(out PlayerInput player))//PlayerInput�R���|�[�l���g�����邩�`�F�b�N�A����Ȃ�Q�b�g����
        {
            _playerInput = player;//���肵���R���|�[�l���g�̑��M
            _playerInput.defaultActionMap = "Player";//DefaultMap�̏�����
        }

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
    }

    private void FixedUpdate()
    {
        new PlayerMover(this.gameObject, _rigidbody, _moveVector, _moveSpeed);//�ړ�
        new PlayerLooker(this.gameObject.transform, _lookVector, _lookSpeed);
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
            rigidbody.WakeUp();
            rigidbody.AddForce(playerObject.transform.forward * moveVector.y *  moveSpeed * .01f, ForceMode.Impulse);
            rigidbody.AddForce(playerObject.transform.right * moveVector.x *  moveSpeed * .01f, ForceMode.Impulse);
        }
        else
        {
            rigidbody.Sleep();
        }
    }
}

public class PlayerLooker
{
    public PlayerLooker(Transform transform, Vector2 lookVector, float lookSpeed)
    {
        transform.Rotate(new Vector3(0, lookVector.x * lookSpeed * .5f, 0));
    }
}