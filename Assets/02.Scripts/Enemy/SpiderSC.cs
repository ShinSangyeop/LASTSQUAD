using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpiderSC : LivingEntity
{
    public LayerMask target;
<<<<<<< HEAD
    private GameObject targetEntity;
=======
    private GameObject targetEnitity;
    public GameObject mainDoor;
>>>>>>> parent of 21a53d0 (20211012_enemy수정)
    public GameObject player;
    public GameObject Bulletobj;

    private float traceRange = 15f;
    private float attackDistance = 12f;

    private NavMeshAgent pathFinder;
    private Animator enemyAnimator;

    [SerializeField]
    private bool isTrace = false;
    [SerializeField]
    private bool isAttacking = false;



    List<GameObject> list = new List<GameObject>();


    Vector3 targetPosition;
    Vector3 targetSize;

    Coroutine co_updatePath;
    Coroutine co_changeTarget;

    /// <summary>
    /// 초기화
    /// </summary>
    private void Awake()
    {
        pathFinder = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();
        SetUp();
<<<<<<< HEAD

        LayerMask playerLayer = 1 << LayerMask.NameToLayer("PLAYER");
        LayerMask defensiveGoodsLayer = 1 << LayerMask.NameToLayer("DEFENSIVEGOODS");

        targetLayer = playerLayer | defensiveGoodsLayer;

        _exp = 1f;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
=======
>>>>>>> parent of 21a53d0 (20211012_enemy수정)
    }
    /// <summary>
    /// 초기 스텟 설정
    /// </summary>
    /// <param name="newHP"></param>
    /// <param name="newSpeed"></param>
    /// <param name="newDamage"></param>
    public void SetUp(float newHP = 20f, float newAP = 0f, float newSpeed = 3f, float newDamage = 0f)
    {
        startHp = newHP;
        currHP = newHP;
        armour = newAP;
        damage = newDamage;
        pathFinder.speed = newSpeed;
    }
    /// <summary>
    /// 애니매이션 Duration 값 얻기
    /// </summary>
    /// <param name="moveType"></param>
    /// <returns></returns>
    public float MoveDuration(eCharacterState moveType)
    {
        string name = string.Empty;
        switch (moveType)
        {
            case eCharacterState.Trace:
                name = "Spider _ Run";
                break;
            case eCharacterState.Attack:
                name = "Spider _ Attack";
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

        if (state == eCharacterState.Trace && Vector3.Distance(new Vector3(targetPosition.x, (targetPosition.y - (targetSize.y / 2)), targetPosition.z), this.transform.position) <= attackDistance && !isAttacking)
        {
            NowAttack();
        }

        if (isAttacking == true)
        {
            Quaternion LookRot = Quaternion.LookRotation(new Vector3(targetPosition.x, (targetPosition.y - (targetSize.y / 2)), targetPosition.z) - new Vector3(this.transform.position.x, 0, this.transform.position.z));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, LookRot, 60f * Time.deltaTime);
        }
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

    IEnumerator FireCo;

    /// <summary>
    /// 공격 함수
    /// </summary>
    void NowAttack()
    {
        isAttacking = true;

        state = eCharacterState.Attack;
        pathFinder.isStopped = true;
        enemyAnimator.SetTrigger("IsAttack");

        float attackdelayTime = 0.5f;
        StartCoroutine(StartAttack(attackdelayTime));

        attackdelayTime = MoveDuration(eCharacterState.Attack);
        StartCoroutine(EndAttacking(attackdelayTime));
    }

    /// <summary>
    /// 추적대상을 찾아서 경로를 갱신
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
                pathFinder.SetDestination(targetEntity.transform.position);
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
            Collider[] colliders = Physics.OverlapSphere(this.transform.position, traceRange, 1 << LayerMask.NameToLayer("PLAYER"));

            try
            {
                Debug.Log($"___ TARGET {targetEntity.name} ____");
            }
            catch (System.Exception e)
            {

            }

            if (colliders.Length >= 1)
                targetEntity = colliders[0].gameObject;
            else
<<<<<<< HEAD
                targetEntity = startTarget;
=======
                targetEnitity = mainDoor;
>>>>>>> parent of 21a53d0 (20211012_enemy수정)

            yield return new WaitForSeconds(0.1f);
        }
    }
    IEnumerator StartAttack(float _delaytime)
    {
        pathFinder.enabled = false;
        yield return new WaitForSeconds(_delaytime);
        //Debug.Log("=== Start Attack ===");
    }
    /// <summary>
    /// 공격 이후 추적 상태 변경 코루틴
    /// </summary>
    /// <param name="_delaytime"></param>
    /// <returns></returns>
    IEnumerator EndAttacking(float _delaytime)
    {
        yield return new WaitForSeconds(_delaytime * 0.8f);
        //Debug.Log("=== End Attack ===");
        isAttacking = false;
        //if (!(Vector3.Distance(targetEnitity.transform.position, this.transform.position) <= attackDistance))
        //{
        //Debug.Log("____Path Finder On____");
        pathFinder.enabled = true;
        NowTrace();
        //}
    }
    /// <summary>
    /// 투사체 발사하는 게임이벤트에 적용될 함수
    /// </summary>
    /// <param name="_delaytime"></param>
    /// <returns></returns>
    IEnumerator Fire(float _delaytime)
    {
        yield return new WaitForSeconds(_delaytime);
        GameObject bullet = Instantiate(Bulletobj, this.transform.position + transform.forward * 1.3f + transform.up * 0.5f, transform.rotation);
        FireCo = null;
    }

    protected override void Down()
    {
        base.Down();
        pathFinder.enabled = false;
        enemyAnimator.SetTrigger("IsDead");
        Debug.Log(MoveDuration(eCharacterState.Die));
        Die();
    }
}