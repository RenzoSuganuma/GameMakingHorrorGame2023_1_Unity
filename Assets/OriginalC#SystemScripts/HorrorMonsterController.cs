using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HorrorMonsterController : MonoBehaviour
{
    /// <summary>
    /// AI��NavMeshAgent
    /// </summary>
    [SerializeField] NavMeshAgent _navMesh;
    
    /// <summary>
    /// AI�̒ǐՂ���v���C���[�̃g�����X�t�H�[��
    /// </summary>
    [SerializeField] Transform _playerTransform;

    /// <summary>
    /// Raycast�̃I�[�o�[���[�h�̃��C���[�}�X�N�ɓn������
    /// </summary>
    [SerializeField] LayerMask _groundLayer, _playerLayer;

    /// <summary>
    /// ����AI�̗̑�
    /// </summary>
    [SerializeField] float _health = 0;

    /// <summary>
    /// ���Y�l�̃A�j���[�^�[
    /// </summary>
    Animator _animator;

    /// <summary>
    /// ���Y�l���̔���
    /// </summary>
    [SerializeField] bool _iamExecutioner;

    /// <summary>
    /// �ǐՒ����̔���
    /// </summary>
    [SerializeField] public bool _isChasing;

    //�p�j
    [SerializeField] Vector3 _movePoint = Vector3.zero;
    bool _movePointIsSet = false;
    [SerializeField] float _movePointRange = 0;

    //�U��
    [SerializeField] float _intervalAttack = 0;
    bool _isAttacked = false;

    //���
    [SerializeField] float _sightRange, _attackRange;
    [SerializeField] bool _playerFound, _playerCanAttack;

    //�v���C���[�ɓ�������I�u�W�F�N�g
    [SerializeField] GameObject _orb;

    //�p�j����BGM
    [SerializeField] GameObject _patrollBGM;

    //�ǐՒ�BGM
    [SerializeField] GameObject _chaseBGM;

    private void Awake()
    {
        if(GameObject.FindGameObjectWithTag("Player").transform !=  null)
            this._playerTransform = GameObject.FindGameObjectWithTag("Player").transform;//�v���C���[�̌���

        if (this.gameObject.GetComponent<NavMeshAgent>() != null)
            this._navMesh = this.gameObject.GetComponent<NavMeshAgent>();//NavMesh�̃R���|�[�l���g�擾

        if(this.gameObject.GetComponent<Animator>() != null && this._iamExecutioner)
            this._animator = this.gameObject.GetComponent<Animator>();//Animator�̃R���|�[�l���g�擾
    }

    private void FixedUpdate()
    {
        //�v���C���[�Ƃ̋������o��
        Debug.Log($"Enemy Between Player Dis Is{Vector3.Distance(this.gameObject.transform.position,this._playerTransform.position)}");

        //�U�����⑫�͈͂̌���
        this._playerFound = Physics.CheckSphere(this.gameObject.transform.position, this._sightRange, this._playerLayer);
        this._playerCanAttack = Physics.CheckSphere(this.gameObject.transform.position, this._attackRange, this._playerLayer);

        if(!this._playerFound && !this._playerCanAttack)//�����⑫�͈͂ƍU���͈͓��ɂ��Ȃ��ꍇ�p�j����
            PatrollingNow();
        if(this._playerFound && !this._playerCanAttack)//�����⑫�����ł�����v���C���[��ǐՂ���
            ChaseWithPlayer();
        if(this._playerFound && this._playerCanAttack)//�����v���C���[�ɒǂ����čU���͈͓��ɂ���ꍇ
            AttackPlayerNow();
    }

    void PatrollingNow()
    {
        if(!this._movePointIsSet)
            SearchMovePoint();//�p�j������W��������

        if(this._movePointIsSet)
            this._navMesh.SetDestination(this._movePoint);//�������炻���Ɍ�����
        //���Y�l�̃A�j���[�V�������� �ǐՂ̓v���C���[��⑫�����Ƃ��̂݃t���O������
        if (this._iamExecutioner)
            this._animator.SetBool("isWalking", true);

        Vector3 distance = transform.position - _movePoint;//�p�j����ڕW�̍��W�Ƃ̋���

        if(distance.magnitude > 1)
            this._movePointIsSet = false;//�ړ�����t���O���O��
        //���Y�l�̃A�j���[�V�������� �ǐՂ̓v���C���[��⑫�����Ƃ��̂݃t���O������
        if (distance.magnitude > 1 && this._iamExecutioner)
        {
            this._animator.SetBool("isChasing", false);
        }

        //BGM�̐ݒ�
        if (this._chaseBGM != null && this._patrollBGM != null)
        {
            this._patrollBGM.SetActive(true);
            this._chaseBGM.SetActive(false);
        }

        //�ǐՒ��t���O�ݒ�
        this._isChasing = false;

        Debug.Log("Walking");
    }
    void SearchMovePoint()
    {
        //X�AZ���ł̜p�j�̖ڕW�̍��W�𗐐��Ŕ���������
        float randX = Random.Range(-this._movePointRange, this._movePointRange);
        float randZ = Random.Range(-this._movePointRange, this._movePointRange);
        //Vector3�ɂ���
        this._movePoint = new Vector3(transform.position.x + randX, transform.position.y, this.gameObject.transform.position.z + randZ);
        //����Raycast���Č�����������΂����ɍs����̂Ŝp�j�̃t���O�𗧂Ă�
        if (Physics.Raycast(this._movePoint, -transform.up, 2f, this._groundLayer))
            this._movePointIsSet = true;
    }

    void ChaseWithPlayer()
    {
        //BGM�̐ݒ�
        if (this._chaseBGM != null && this._patrollBGM != null)
        {
            this._patrollBGM.SetActive(false);
            this._chaseBGM.SetActive(true);
        }

        //�v���C���[�̒ǔ�
        this._navMesh.SetDestination(this._playerTransform.position);
        this._isChasing = true;//�ǐՒ��t���O�ݒ�

        //���Y�l�̃A�j���[�V��������
        if (this._iamExecutioner)
        {
            this._animator.SetBool("isChasing", true);
            this._animator.SetBool("isWalking", false);
        }
    }

    void AttackPlayerNow()
    {
        //���̎��ɂ�����W�ɂƂǂ܂�
        this._navMesh.SetDestination(this.gameObject.transform.position);
        //�v���C���[������
        this.gameObject.transform.LookAt(this._playerTransform.position);

        if (!this._isAttacked)
        {
            #region �U������
            if (this._orb != null)
            {
                GameObject orb = Instantiate(this._orb);
                orb.transform.position = this.gameObject.transform.position;
                Rigidbody rigidbody = orb.GetComponent<Rigidbody>();
                rigidbody.velocity = (this._playerTransform.position - this.gameObject.transform.position) * 10;
                Destroy(orb, 1f);
            }
            #endregion

            //�U�������������̂ōU���������t���O�𗧂Ă�
            this._isAttacked = true;
            //�t���O�������ŗ��Ă�
            Invoke(nameof(RasetAttackCondition), this._intervalAttack);
        }
    }

    void RasetAttackCondition()
    {
        this._isAttacked = false;
    }

    public void ModifyHealth(int health)
    {
        //�̗͂̕␳
        this._health += health;
        //�̗͂��O�ȉ��̎�
        if (this._health < 0)
            Invoke(nameof(DestroyThisObject), .05f);
    }

    void DestroyThisObject()
    {
        Destroy(this.gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, this._attackRange);//�U���͈͂̃M�Y����`��
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position, this._sightRange);//�⑫�͈͂̃M�Y����`��
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(this.gameObject.transform.position, Vector3.one * 10);
    }
}
