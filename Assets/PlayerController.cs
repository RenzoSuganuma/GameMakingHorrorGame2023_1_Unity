using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent (typeof(PlayerInput))]
[RequireComponent (typeof(CapsuleCollider))]
/// <summary>
/// �v���C���[����p�̃N���X ver - alpha
/// </summary>
public class PlayerController : MonoBehaviour
{
    /// <summary> InputSystem �� PlayerInput�R���|�[�l���g </summary>
    PlayerInput _playerInput;

    /// <summary> �L�����ړ��Ɏg�� </summary>
    Rigidbody _rigidbody = default;

    /// <summary> �L�����ړ��Ɏg���R���C�_�[ </summary>
    CapsuleCollider _capsuleCollider = default;

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
    private bool _flashLightIsOn = true;

    /// <summary> �v���C���[���A�C�e�����E������ </summary>
    private bool _pickkingNow = false;

    /// <summary> �v���C���[�������[�h�L�[���������Ƃ� </summary>
    private bool _reloadNow = false;

    /// <summary> SE�p�̃I�u�W�F�N�g�̃^�O���R�Â�����Ă�I�u�W�F�N�g�i�[�p </summary>
    GameObject[] _compareTagSoundEffect;
    /// <summary> �v���C���[���s���̌��ʉ��Đ��p�̃I�u�W�F�N�g�i�[�p </summary>
    GameObject _walkingSoundEffectObject = null;

    /// <summary> �A�C�e���|�[�`�̃I�u�W�F�N�g </summary>
    [SerializeField] GameObject _itemPoach;
    /// <summary> �g�p���̃A�C�e���̃z���_�[�I�u�W�F�N�g </summary>
    [SerializeField] GameObject _playerHand;

    /// <summary> �g�p���̓d�r�I�u�W�F�N�g </summary>
    [SerializeField] GameObject _usingBatteryNow;

    //�ȉ��v���C���[�̑���ɕK�v�ȃN���X
    PlayerMover _playerMover;//�v���C���[�̈ړ��p�N���X
    PlayerLooker _playerLooker;//Player�U������p�N���X
    MouseCursoreLocker _cursoreLocker;//�J�[�\�����b�N�N���X
    FlashLightController _flashLightController;//�����d������p�N���X
    PlayerCameraController _playerCameraController;//Player�J�����̑���p�N���X
    WalkingSoundEffectController _walkingSoundEffectController;//���sSE����p�N���X

    /// <summary> �A�C�e���|�[�`�ɕK�v�ȋ@�\�𐷂荞��ł���R���|�[�l���g </summary>
    InventrySystem _inventrySystem;

    /// <summary> �d�r�������i�[���Ă���R���|�[�l���g </summary>
    BatteryLifeContainor _batteryLifeContainor;

    private void Start()
    {
        //�e�K�v�ȃN���X�̃C���X�^���X���i�ȉ��j
        this._itemPoach = GameObject.FindGameObjectWithTag("Inventry_Poach");//�C���x���g���[�̃I�u�W�F�N�g�̌���
        this._playerHand = GameObject.FindGameObjectWithTag("Player_Hand");//�t���b�V�����C�g�̃I�u�W�F�N�g�̌���

        this._cursoreLocker = new MouseCursoreLocker();//�}�E�X�J�[�\���̃��b�N�p�N���X�g�p
        this._cursoreLocker.MouseCursoreLock();//�}�E�X�J�[�\���̃��b�N

        this._flashLightController = new FlashLightController();//�����d���̑���N���X
        this._playerMover = new PlayerMover();//�v���C���[�̑���N���X
        this._playerLooker = new PlayerLooker();//�v���C���[�̐U������̑���N���X
        this._playerCameraController = new PlayerCameraController();//�v���C���[�̎��쑀��N���X
        this._walkingSoundEffectController = new WalkingSoundEffectController();//�v���C���[���sSE�̑���N���X

        if (TryGetComponent<PlayerInput>(out PlayerInput player))//PlayerInput�R���|�[�l���g�����邩�`�F�b�N�A����Ȃ�Q�b�g����
        {
            this._playerInput = player;//���肵���R���|�[�l���g�̑��M
            this._playerInput.defaultActionMap = "Player";//DefaultMap�̏�����
        }
        else
            Debug.LogError("The Component PlayerInput Is Not Found");

        if (TryGetComponent<Rigidbody>(out Rigidbody rigid))//Digidbody�R���|�[�l���g�����邩�`�F�b�N�A����Ȃ�Q�b�g����
        {
            this._rigidbody = rigid;//���肵���R���|�[�l���g�̑��M
            //�e�p�����[�^�[�̏�����
            this._rigidbody.mass = 1.0f;
            this._rigidbody.drag = 1.5f;
            this._rigidbody.angularDrag = .05f;
            this._rigidbody.useGravity = true;
            this._rigidbody.isKinematic = false;
            this._rigidbody.freezeRotation = true;
        }
        else
            Debug.LogError("The Component Rigidbody Is Not Found");

        if (TryGetComponent<CapsuleCollider>(out CapsuleCollider capsule))//Digidbody�R���|�[�l���g�����邩�`�F�b�N�A����Ȃ�Q�b�g����
        {
            //�e�p�����[�^�[�̏�����
            this._capsuleCollider = capsule;
            this._capsuleCollider.radius = .3f;
            this._capsuleCollider.height = 1.5f;
        }
        else
            Debug.LogError("The Component CapsuleCollider Is Not Found");

        if(TryGetComponent<InventrySystem>(out InventrySystem inventrySystem))//�C���x���g���V�X�e���̃R���|�[�l���g�̎擾
        {
            this._inventrySystem = inventrySystem;
        }
        else
            Debug.LogError("The Component InventrySystem Is Not Found");

        //�����d���̃I�u�W�F�N�g�̌���
        { this._flashLight = this._inventrySystem.GetRandomChildObjectWithTag(_playerHand, "FlashLight"); }
        if (_flashLight.TryGetComponent<Light>(out Light light))//Light�R���|�[�l���g�̌���
            this._light = light;
        else
            Debug.LogError("The Component Light Is Not Found");


        { this._playerCamera = GameObject.FindGameObjectWithTag("PlayerCamera"); }

        this._compareTagSoundEffect = GameObject.FindGameObjectsWithTag("SoundEffect");//���ʉ��p�̃^�O���A�^�b�`����Ă���I�u�W�F�N�g�i�z��j������
        foreach (GameObject @object in this._compareTagSoundEffect)//�{�g������g�b�v�܂ł̗v�f�̃`�F�b�N
        {
            if (@object.CompareTag("SoundEffect") && @object.name == "Walk_SE")//����̏����𖞂����I�u�W�F�N�g���������ꍇ
            {
                this._walkingSoundEffectObject = @object; // �ϐ��ɃI�u�W�F�N�g���i�[
                break; // �����𖞂����I�u�W�F�N�g�����������烋�[�v���I��
            }
        }
       }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        //TEST
        SetUsingBattery();
        //#endTEST

        { this._flashLightIsOn = this._illuminate; }//�ق��N���X��������d���̃X�e�[�^�X���X�R�[�v���邽��

        this._playerMover.PlayerMove(this.gameObject, this._rigidbody, this._moveVector, this._moveSpeed);//�ړ�
        this._playerLooker.PlayerLooking(this.gameObject.transform, this._lookVector, this._lookSpeed);//�U�����
        this._flashLightController.FlushLightLight(this._light, this._illuminate, this._usingBatteryNow);//�����d����ONOFF
        this._playerCameraController.PlayerCameraMove(this._playerCamera, this._lookVector, this._lookSpeed, 45f);//�J�����㉺��]
        this._walkingSoundEffectController.WalkingSoundEffectPlayStatusSet(this._walkingSoundEffectObject, this._moveVector);//���s���ʉ���
    }

    /// <summary>
    /// PlayerInput����̃v���C���[�ړ��p��Vector2�̎��֐�
    /// </summary>
    /// <param name="context"></param>
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.ReadValue<Vector2>() != null)
        {
            this._moveVector = context.ReadValue<Vector2>().normalized;//�x�N�g���̐��K�������ĒP�ʃx�N�g���ɕϊ������Ď΂߈ړ��̕s���R��������
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
            this._lookVector = context.ReadValue<Vector2>().normalized;//�x�N�g���̐��K�������ĒP�ʃx�N�g���ɕϊ������Ď΂߂ӂ�����̕s���R��������
            //print($"{_lookVector} : InputDevice - InputValue : Look");//�f�o�b�O�p
        }
    }

    /// <summary>
    /// �����d����������͎��֐�
    /// </summary>
    /// <param name="context"></param>
    public void OnFire(InputAction.CallbackContext context)
    {
        this._illuminate = context.ReadValueAsButton();//�}�E�X�{�^�����̃N���b�N�̏�Ԃ̊i�[
        //print($"{_illuminate} : is Illuminate");//�f�o�b�O�p
    }

    /// <summary>
    /// �C���^���N�g�̓��͂��������Ƃ�
    /// </summary>
    /// <param name="context"></param>
    public void OnInteract(InputAction.CallbackContext context)
    {
        this._pickkingNow = context.ReadValueAsButton();//�L�[�{�[�hE�̓��͒l���i�[
        //Debug.Log("pikkedNOW" + _pickkingNow);
    }
    
    /// <summary>
    /// �����[�h�̓��͂��������Ƃ�
    /// </summary>
    /// <param name="context"></param>
    public void OnReload(InputAction.CallbackContext context)
    {
        this._reloadNow = context.ReadValueAsButton();//�L�[�{�[�hE�̓��͒l���i�[
        //Debug.Log("pikkedNOW" + _pickkingNow);
    }

    private void OnTriggerStay(Collider other)
    {
        if (TriggeredObjectCheckToPickUp(other.gameObject))
        {
            Debug.Log($"{other.name} Is Can Pick");
            if (this._pickkingNow)
            {
                AttachItemToPoach(other.gameObject, this._itemPoach);
                Debug.Log($"Picked {other.name}");
            }
        }
    }

    /// <summary>
    /// ���X�g�ɓo�^���Ă���^�O�Əƍ����Ĉ����̃I�u�W�F�N�g�����X�g�ɂ�����̂Ɠ����^�O�������Ă����True��Ԃ�
    /// </summary>
    /// <param name="triggeredObject"></param>
    /// <returns></returns>
    private bool TriggeredObjectCheckToPickUp(GameObject triggeredObject)
    {
        List<string> _objectTagList = new List<string>() { "FlashLight", "Battery", "Empty" };//�E����A�C�e���̃^�O�̃��X�g
        bool returnValue = true;
        if (triggeredObject != null)//�ЂƂ܂�null�`�F�b�N
        {
            string objectTag = triggeredObject.tag;//�^�O�̑��M
            foreach (string tag in _objectTagList)//foreach����
            {
                Debug.Log("TriggeredObjectSearchingTAG : " + tag);
                if (objectTag == tag)//������v������
                {
                    returnValue = true;//�Ԃ�l��true�ł���ȍ~�͌����������Ȃ��̂�break����B�]�v�Ɍ������|����Ƒz�肵�Ȃ��l���Ԃ�
                    break;
                }
                else
                {
                    returnValue = false;
                }
            }
        }
        Debug.Log("TriggeredObjectRETURN : " + returnValue);
        return returnValue;
    }

    /// <summary>
    /// �A�C�e���|�[�`�ɃA�C�e�����A�^�b�`���ăA�C�e���|�[�`�̔z���ɂ���
    /// </summary>
    /// <param name="itemObject"></param>
    /// <param name="itemPoachObject"></param>
    private void AttachItemToPoach(GameObject itemObject,GameObject itemPoachObject)
    {
        this._inventrySystem.MakeParenToChild(itemObject, itemPoachObject.transform);
    }

    /// <summary>
    /// �o�b�e���̎g�pORNOT����֐�
    /// </summary>
    private void SetUsingBattery()
    {
        //���ݎg�p���̃o�b�e���[���Ȃ��A�C���x���g���ɂ��邩����
        if (this._usingBatteryNow == null)//���ݎg���Ă���o�b�e���[����ɂȂ����ꍇ�ɂ��̏������Ăяo��
        {
            if (this._inventrySystem.GetRandomChildObjectWithTag(this._itemPoach, "Battery") != null)//�C���x���g�����̊Y���I�u�W�F�N�g���������ꍇ
                this._usingBatteryNow = this._inventrySystem.GetRandomChildObjectWithTag(this._itemPoach, "Battery");

            if (this._usingBatteryNow != null && this._usingBatteryNow.GetComponent<BatteryLifeContainor>() != null)//�_�u���̃I�u�W�F�N�g�ƃR���|�[�l���g��null-check
            {
                this._batteryLifeContainor = this._usingBatteryNow.GetComponent<BatteryLifeContainor>();
                this._flashLightController.SetBatteryLife(this._batteryLifeContainor._batteryLife);//���ݎg���Ă���o�b�e���[�̎����l��ݒ�
            }
        }
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
    private float _batteryLife = 0f;//�o�b�e���[�̎����̏����l
    public void FlushLightLight(Light light, bool lightOrnot, GameObject currentBatteryObject)
    {
        if (_batteryLife > 0)//�o�b�e���[�c�ʂ�����Ƃ�
        {
            light.enabled = lightOrnot;//�_���̓��͒l�ɉ����ăR���|�[�l���g�̗L��������؂�ւ�
            this._batteryLife -= Time.deltaTime;//�ʏ����
            if(lightOrnot) _batteryLife -= Time.deltaTime * 1.5f;//�_��������Ώ���ʂ͑����邩��_�����]���ɏ���
            Debug.Log($"current battery life : {_batteryLife}");
        }
        else
        {
            light.enabled = false;//�_���ł��Ȃ��悤�ɂ���
            if(currentBatteryObject != null)
                GameObject.Destroy(currentBatteryObject);
            Debug.Log($"current battery is death");
        }
    }

    /*
    public void FlashLightBatteryCharge(float batteryPower)
    {
        this._batteryLife = batteryPower;//�o�b�e���[�̎����̉񕜂�����
        Debug.Log($"battery charged");
    }
    */

    public float GetBatteryLife()
    {
        return _batteryLife;
    }
    
    public void SetBatteryLife(float batteryLifeValue)
    {
        this._batteryLife = batteryLifeValue;
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
        if(moveVector != Vector2.zero)//���s�̓��̗͂L���ŕ��s�pSE�̃I�u�W�F�N�g�̗L��������؂�ւ�
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
