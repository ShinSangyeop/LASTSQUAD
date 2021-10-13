using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovidicSC : LivingEntity
{
    public LayerMask target;

    private GameObject targetEntity;
    public GameObject maindoor;
    public GameObject attackColl;
    private GameObject pastTarget; // 이전 타겟 저장 변수

    float traceRange = 15f;
    float attackDistance = 7f;
    float rushDistance = 20f;

    private NavMeshAgent pathFinder;
    private Animator enemyAnimator;
    private Rigidbody rigid;

    [SerializeField]
    private bool isTrace = false;
    [SerializeField]
    private bool isAttacking = false;
    private bool canRush = true;
    public bool isRush = false;

    Coroutine co_updatePath;
    Coroutine co_changeTarget;

    List<GameObject> list = new List<GameObject>();

    Vector3 targetPosition;
    Vector3 targetSize;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        pathFinder = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();
        Setup();

        _exp = 4f;
    }

    public void Setup(float newHp = 300f, float newAP = 5f, float newSpeed = 2f, float newDamage = 20f)
    {
        startHp = newHp;
        currHP = newHp;
        armour = newAP;
        damage = newDamage;
        pathFinder.speed = newSpeed;
    }

    public float MoveDuration(eCharacterState moveType)
    {
        string name = string.Empty;
        switch (moveType)
        {
            case eCharacterState.Trace:
                name = "Run";
                break;
            case eCharacterState.Attack:
                name = "AttackBite";
                break;
            case eCharacterState.Die:
                name = "DeathHit";
                break;
            default:
                return 0;
        }

        float time = 0;

        RuntimeAnimatorController ac = enemyAnimator.runtimeAnimatorController;

        for (int i = 0; i < ac.animationClips.Length; i++)
        {
            if (ac.animationClips[i].name == name)
            {
                time = ac.animationClips[i].length;
            }

        }
        return time;
    }
    void Start()
    {
        NowTrace();
        co_updatePath = StartCoroutine(UpdatePath());
        co_changeTarget = StartCoroutine(ChangeTarget());
        targetPosition = targetEntity.GetComponent<Collider>().bounds.center;
        targetSize = targetEntity.GetComponent<Collider>().bounds.size;
    }

    void Update()
    {
        if (dead)
            return;

        if (state == eCharacterState.Trace && Vector3.Distance(new Vector3(targetPosition.x, (targetPosition.y - (targetSize.y / 2)), targetPosition.z), this.transform.position) <= rushDistance && !isAttacking)
        {
            if (canRush == true)
            {
                isRush = true;
                pastTarget = targetEntity.gameObject;
                pathFinder.enabled = false;
                isTrace = false;
                enemyAnimator.SetBool("IsTrace", isTrace);

                canRush = false;
                Invoke("RushAttack", 2f);
            }

            if (Vector3.Distance(targetEntity.transform.position, this.transform.position) <= attackDistance && !isRush)
            {
                NowAttack();
            }
        }

        if (isAttacking == true)
        {
            Quaternion LookRot = Quaternion.LookRotation(new Vector3(targetPosition.x, (targetPosition.y - (targetSize.y / 2)), targetPosition.z) - new Vector3(this.transform.position.x, 0, this.transform.position.z));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, LookRot, 60f * Time.deltaTime);
        }

        if (pastTarget != null && targetEntity.gameObject != pastTarget.gameObject)
        {
            canRush = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("MAINDOOR"))
        {
            if (!list.Contains(other.gameObject))
            {
                list.Add(other.gameObject);
                isTrace = false;

                Vector3 hitPoint = other.ClosestPoint(gameObject.GetComponent<Collider>().bounds.center);
                Vector3 hitnormal = new Vector3(hitPoint.x, hitPoint.y, hitPoint.z).normalized;

                MovidicSC modivic = other.GetComponent<MovidicSC>();
                other.GetComponent<LivingEntity>().Damaged(damage, hitPoint, hitnormal);
            }
            else
                return;
        }
        else if (other.CompareTag("DEFENSIVEGOODS"))
        {
            if (!list.Contains(other.gameObject))
            {
                list.Add(other.gameObject);
                isTrace = false;

                Vector3 hitPoint = other.ClosestPoint(gameObject.GetComponent<Collider>().bounds.center);
                Vector3 hitnormal = new Vector3(hitPoint.x, hitPoint.y, hitPoint.z).normalized;

                MovidicSC modivic = other.GetComponent<MovidicSC>();
                other.GetComponent<LivingEntity>().Damaged(damage, hitPoint, hitnormal);
            }
            else
                return;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isTrace = true;

        enemyAnimator.SetBool("IsTrace", isTrace);
    }

    /// <summary>
    /// 추적 함수
    /// </summary>
    void NowTrace()
    {
        state = eCharacterState.Trace;
        if (pathFinder.enabled)
        {
            pathFinder.isStopped = false;
            pathFinder.speed = 3f;
            isTrace = true;
            enemyAnimator.SetBool("IsTrace", isTrace);
        }
    }
    /// <summary>
    /// 공격 함수
    /// </summary>
    void NowAttack()
    {
        damage = 20f;
        isAttacking = true;

        state = eCharacterState.Attack;

        pathFinder.enabled = true;
        pathFinder.speed = 0f;
        //enemyAnimator.SetFloat("RushSpeed", 1f);
        enemyAnimator.SetTrigger("IsAttack");
        float attackTime = 0.5f;
        StartCoroutine(StartAttacking(attackTime));
        attackTime = 0.6f;
        StartCoroutine(NowAttacking(attackTime));
        float attackdelayTime = MoveDuration(eCharacterState.Attack);
        StartCoroutine(EndAttacking(attackdelayTime));

        Debug.Log(MoveDuration(eCharacterState.Attack));
    }
    /// <summary>
    /// 돌진 공격 함수
    /// </summary>
    void RushAttack()
    {
        damage = 40f;
        rigid.AddForce(this.transform.forward * 25f, ForceMode.Impulse);
        enemyAnimator.SetTrigger("IsAttack");
    }

    public void ClearList()
    {
        list.Clear();
    }

    /// <summary>
    /// 추적 대상을 찾아서 경로를 갱신 
    /// </summary>
    /// <returns></returns>
    IEnumerator UpdatePath()
    {
        yield return new WaitForSeconds(0.3f);
        while (!dead)
        {
            if (pathFinder.enabled)
            {
                pathFinder.isStopped = false;
                targetPosition = targetEntity.GetComponent<Collider>().bounds.center;
                targetSize = targetEntity.GetComponent<Collider>().bounds.size;
                pathFinder.SetDestination(new Vector3(targetPosition.x, (targetPosition.y - (targetSize.y / 2)), targetPosition.z));
            }

            yield return new WaitForSeconds(0.1f);
        }
    }
    /// <summary>
    /// 추적 대상을 변경
    /// </summary>
    /// <returns></returns>
    IEnumerator ChangeTarget()
    {
        while (!dead)
        {
            Collider[] colliders = Physics.OverlapSphere(this.transform.position, traceRange, 1 << LayerMask.NameToLayer("DEFENSIVEGOODS"));

            if (colliders.Length >= 1)
                targetEntity = colliders[0].gameObject;
            else
                targetEntity = maindoor;

            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator StartAttacking(float _delaytime)
    {
        yield return new WaitForSeconds(_delaytime);
        pathFinder.enabled = false;
    }

    IEnumerator NowAttacking(float _delaytime)
    {
        yield return new WaitForSeconds(_delaytime);
        ClearList();
    }

    IEnumerator EndAttacking(float _delaytime)
    {
        yield return new WaitForSeconds(_delaytime * 0.8f);
        isAttacking = false;
        pathFinder.enabled = true;
        NowTrace();
        print("=== ATTACK END ===");
    }

    void ColliderOn()
    {
        attackColl.SetActive(true);
    }

    void ColliderOFF()
    {
        attackColl.SetActive(false);
    }

    protected override void Down()
    {
        base.Down();
        pathFinder.enabled = false;
        enemyAnimator.SetTrigger("IsDead");
        //Debug.Log(MoveDuration(eCharacterState.Die));
        Die();
    }
}
