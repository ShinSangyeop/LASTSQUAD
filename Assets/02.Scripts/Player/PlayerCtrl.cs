using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using InterfaceSet;
using System;
using UnityEngine.UI;



public class PlayerCtrl : LivingEntity, IAttack, IDamaged
{
    #region 플레이어 UI 관련 변수
    public Image playerHpImage; // 플레이어 체력바
    public Text playerHpText; // 플레이어 체력 텍스트
    private PlayerUI playerUI; // 플레이어 UI 스크립트

    public bool isUIOpen; // 플레이어가 UI(상점)을 열었는지를 판단하는 변수
    // 플레이어가 건설, 수리, 회복 등의 동작을 하고 있는지 판단하는 불리언 변수
    public bool doAction
    {
        get { return playerAction.isBuild || playerAction.isRepair || playerAction.isHeal; }
    }

    #endregion

    #region 플레이어 Status 관련 변수
    /// <summary>
    /// 플레이어 최대 체력
    /// </summary>
    public float maxHp { get { return startHp + addHP; } }
    /// <summary>
    /// 플레이어 추가 체력
    /// </summary>
    public float addHP { get; private set; }
    /// <summary>
    /// 플레이어 현재 체력
    /// </summary>
    //public float currHP { get; private set; } // LivingEntity 에서 받음
    /// <summary>
    /// 플레이어 추가 방어력
    /// </summary>
    public float addArmour { get; private set; }


    //private float addAttack =0f;
    internal float addAttack = 0f;
    // 데미지 증가 Perk 발동 확률
    private float attackPerk_Percent = 0f;
    private float addAttack_Perk = 0f;
    /// <summary>
    /// 현재 플레이어 추가 공격력
    /// </summary>
    public float currAddAttack
    {
        get
        {
            float _value = addAttack;

            if (playerSkillManager.perk2_Level >= 1)
            {
                int percent = UnityEngine.Random.Range(0, 100);
                if (!(percent >= (100 - attackPerk_Percent)))
                {
                    _value += addAttack_Perk;
                    //Debug.Log("___VALUE: " + _value + "___");
                }
            }

            return _value;
        }
    }

    public Image bloodScreen;

    // 각 스킬 및 플레이어의 최대 레벨
    private int playerMaxLevel = 18;
    private int statusMaxLevel = 5;
    private int abilityMaxLevel = 3;
    private int perkMaxLevel = 1;

    public int level = 1;
    public int skillPoint = 0;
    public int _point; // 플레이어가 보유하고 있는 포인트

    internal float targetExp = 50f;
    internal float _playerExp = 0f;

    #endregion

    #region 플레이어 이동 관련 변수
    public CharacterController controller; // 플레이어의 움직임에 사용될 CharacterController 컴포넌트

    private float useSpeed; // 플레이어의 이동에 직접 사용될 변수
    private float changedSpeed; // 플레이어의 동작이 변경했을 때 바뀌는 속도
    // Lerp를 사용해서 useSpeed가 changedSpeed로 천천히 변한다.
    private float walkSpeed; // 플레이어가 걷는 속도
    private float runSpeed; // 플레이어가 달리는 속도
    private float crouchSpeed; // 플레이어가 앉았을 때 속도
    private float gravity; // 플레이어 적용 중력

    private float motionChangeSpeed; // 플레이어의 이동 속도 보간 값.

    #endregion

    [Space(5)]
    [Header("Player Camera", order = 1)]
    #region 플레이어 카메라 관련 변수
    public Transform playerCameraTr; // 플레이어 카메라 위치
    private PlayerAction playerAction; // 플레이어의 동작과 관련 내용이 있는 스크립트
    private Vector3 cameraPosition; // 플레이어 카메라의 포지션 변경 값 저장 변수

    private Rigidbody myRb; // 플레이어의 Rigidbody

    /// <summary>
    /// 플레이어의 상체 회전 속도
    /// </summary>
    private float upperBodyRotation;
    /// <summary>
    /// 상체 회전의 한계 값.
    /// </summary>
    private float upperBodyRotationLimit;
    /// <summary>
    /// 화면(카메라) 회전 속도
    /// </summary>
    /// <param name=""></param>
    private float lookSensitivity;
    /// <summary>
    /// 카메라가 움직일 때 보간 값
    /// </summary>
    //private float cameraMoveSpeed = 4f;
    //private float cameraMoveValue = 0.375f;

    #endregion

    #region 플레이어 행동 bool 변수
    private bool isMove; // 플레이어가 움직이고 있는지 판단하는 변수
    private bool isCrouch; // 플레이어가 앉아있는지 판단하는 변수
    private bool doCrouch; // 플레이어가 앉아있을 때 달리면 이 값을 True로 만들어서 TryCrouch가 동작하게 한다.

    #endregion

    #region 플레이어 관련 변수
    private Transform tr; // 플레이어의 위치(Transform)
    public string playerName { get; set; } // 플레이어 이름(유저 닉네임)
    public PlayerClass.ePlayerClass playerClass { get; set; } // Player의 Class(직업)
    public Animator playerAnim; // 플레이어의 Animator

    #endregion

    #region 플레이어 직업 관련 변수
    Dictionary<string, string> classDict = null;

    #endregion

    #region 플레이어 무기 관련 변수
    public WeaponManager weaponManager = null;
    // 스킬 설정을 잘못해서 스킬을 레벨당 다른 결과 값을 출력하도록 되어있는데
    // 스킬은 레벨이 한 번에 한 번씩 오르므로 그냥 변수에 한 번 씩 값을 증가시키도록 했으면 됐는데
    // 다른 결과가 출력되서 증가가 아니라 대입을 시키는 방식으로 되어있어서 어쩔 수 없이 Perk 용의 변수를 따로 만들어야 한다...
    // 그리고 반환용 변수도 따로 만들어야 한다.....
    // 수정할 여유가 생기면 스킬은 매번 레벨마다 발생하는 증가값을 반환하게 처리하고
    // 변수 하나에 더하는 방식으로 하는 것이 좋을 것 같다.
    public float incCarryBullet;

    // 증가하는 공격 속도
    private float incAttackSpeed;
    // 증가하는 공격 속도 Perk
    private float incAttackSpeed_Perk;
    public float currIncAttackSpeed
    {
        get
        {
            float _value = (incAttackSpeed + incAttackSpeed_Perk);
            return _value;
        }
    }


    #endregion

    #region 플레이어 스킬 관련 변수
    private PlayerSkillManager playerSkillManager;

    // 획득할 스킬의 목록으로 할 스킬 리스트
    public List<string> _select_SkillList = null;

    #endregion

    #region 아이템 관련 변수
    public string _haveItemUID = null; // 가지고 있는 아이템 UID
    // 아이템을 가지고 있는가?
    public bool isHaveItem
    {
        get
        {
            return haveMedikit || haveDefStruct;
        }
    }
    // 회복 아이템을 가지고 있는가?
    public bool haveMedikit = false;
    // 방어물자를 가지고 있는가?
    public bool haveDefStruct = false;

    #endregion

    public GameObject crosshairPanel; // 조준점을 표시해주는 canvas

    private void Awake()
    {
        playerUI = GetComponent<PlayerUI>();
        playerAction = playerCameraTr.GetComponent<PlayerAction>();
        tr = this.GetComponent<Transform>();

        myRb = this.controller.GetComponent<Rigidbody>();
        #region 주석
        // 캐릭터 컨트롤러에 붙어있는 Rigidbody를 받아오는 것??
        #endregion

        playerSkillManager = GetComponent<PlayerSkillManager>();
    }

    // Start is called before the first frame update
    void Start()
    {


        //playerName = string.Format("Player1");
        //playerClass = PlayerClass.ePlayerClass.Soldier;
        // 플레이어의 이름 및 플레이어의 직업 설정
        #region 주석
        /*
         * 플레이어의 직업은 나중에 다른 입력값을 통해서 결정이 될 것이다.
         * 현재는 플레이어의 결정된 직업에 따라서 
         * 들어오는 데이터를 처리해야 한다.
         */
        #endregion

        // String을 Enum Type으로 변경하는 방법.
        //playerClass = (PlayerClass.ePlayerClass)Enum.Parse(typeof(PlayerClass.ePlayerClass), classDict["ClassName"]);

        // 플레이어가 선택한 직업을 가져와서 설정이 끝나면
        // 그 직업과 관련된 데이터를 가져와서 Dictionary에 저장해야한다.
        StartCoroutine(CoPlayerClassSetting());

        controller.enabled = true;


        // 지금은 그냥 설정하고 있지만 나중에는 Init함수를 만들어서 PlayerClassSetting 코루틴의 마지막에 함수를 실행하는 방식으로 변경
        // 나중에는 함수를 사용해서 체력 및 방어력, 공격력 등을 설정할 것.
        // DB에서 데이터를 받아오는 식으로.
        // 그냥 프리팹에 기본 데이터를 설정해놓고 그대로 가져와서 처음 값을 설정하는 방식으로 변경.
        // 저번에 쓴적 있는 데이터만 저장해놓는 부분을 만들고 거기서 가져오는 방식으로
        startHp = 100f;
        addHP = 0f;
        currHP = maxHp;
        addArmour = 0f;
        addAttack = 0f;
        addAttack_Perk = 0f;
        #region 주석
        /*
         * addHp >> 추가 Status에 영향을 주는 것은 초반엔 직업 밖에 없으므로
         * 직업을 설정할 때 추가 Status에 대한 증가 설정을 끝낸다.
         * 최대 체력은 100f + addHP이고 시작할 때 현재 체력은 maxHP이다.
         */
        #endregion

        walkSpeed = 6f;
        runSpeed = walkSpeed * 1.5f;
        crouchSpeed = walkSpeed * 0.35f;
        changedSpeed = walkSpeed;
        useSpeed = changedSpeed;
        gravity = -0.098f;

        motionChangeSpeed = 4f;
        #region 주석
        /*
         * 플레이어들의 이동 속도는 6/s로 고정이며, 달리는 속도는 걷는 속도의 1.5배,
         * 앉아서 이동하는 속도는 걷는 속도의 0.35배로 설정된다.
         * 처음 플레이어는 서있는 상태이므로 기본 이동 속도인 walkSpeed를
         * 플레이어가 목표로하는 이동속도 changedSpeed로 설정하고, 처음엔 목표 속도와 현재 이동 속도가 동일해도 되므로
         * moveSpeed를 changedSpeed로 설정한다.
         * 각각 달리거나 서거나 앉는 동작이 변화할 때의 보간 값은 4로 설정한다.
         */
        #endregion

        upperBodyRotation = 0f;
        lookSensitivity = 6f;
        upperBodyRotationLimit = 35f;
        #region 주석
        /*
         * 상체의 회전각은 게임이 시작할 때는 정면을 보고 있으므로 0이고
         * 플레이어의 카메라가 상하 및 좌우 회전을 할 때의 임계값을 8f설정하였다.
         * 한 번에 플레이어의 상체가 좌우로 회전할 수 있는 최대 각은 35이다.
         */
        #endregion

        // 시작했을 때 플레이어는 가만히 서있을 것이기 때문에 imMove와 isCrouch에 false를 둔다.
        // 앉아있다가 달릴때 doCrouch가 true가 되어야 하므로 초기값은 doCrouch가 false가 된다
        isMove = false;
        isCrouch = false;
        doCrouch = false;

        // 시작할 때 카메라의 현재 위치를 저장한다.
        cameraPosition = playerCameraTr.localPosition;

        // 무기 관련 변수
        incAttackSpeed = 0f;
        incAttackSpeed_Perk = 0f;
        incCarryBullet = 0f;

        HPGaugeChange();

        _select_SkillList = new List<string>();

        // 시작 포인트는 50포인트로 설정했다.
        _point = 50;
    }


    protected override void OnEnable()
    {
        base.OnEnable();
        currHP = maxHp;
    }


    #region 플레이어 직업을 세팅하고 직업 관련 데이터를 가져오는 함수 관련
    /// <summary>
    /// 플레이어 직업 관련된 값을 Dictionary 변수에 저장할 때까지 시도하는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator CoPlayerClassSetting()
    {
        yield return new WaitForSeconds(0.05f);
        while (classDict == null)
        {
            classDict = DBManager.Instance.GetClassInfo(playerClass);
            yield return null;
        }

        //weaponManager.WeaponChange(classDict["WeaponUID"]);
        PlayerWeaponChange(classDict["WeaponUID"]);
        //Debug.Log(classDict["ClassName"]);
    }

    #endregion


    #region Player의 무기를 변경하는 함수 관련
    public bool PlayerWeaponChange(string _weaponUID)
    {
        // 지금 UID 설정을 잘못해서 rarity가 1밖에 차이나지 않지만
        // 자리수가 다르게 설정하면 더 쉽게 판단할 수 있다.
        string _rarity = _weaponUID.Substring(5, 4);
        if (_rarity == "0001" && GameManager.instance.perk0_Active == false)
        {
            ActionTextSetting("1단계 특전을 활성화해야 합니다.");
            return false;
        }
        else if (_rarity == "0002" && GameManager.instance.perk1_Active == false)
        {
            ActionTextSetting("2단계 특전을 활성화해야 합니다.");
            return false;
        }


        ActionTextSetting("무기 변경 중");
        // 상점에서 구매하려는 무기의 transform을 받아서 처리.
        weaponManager.WeaponChange(_weaponUID);
        return true;
    }

    #endregion




    private bool useCheat = false;

    // Update is called once per frame
    void Update()
    {
        // Player Move 함수 실행, 상점을 열고 있는 상황이 아닐경우 플레이어가 움직일 수 있게 한다.
        if (isUIOpen == false)
            PlayerMove();

        if (Input.GetKeyDown(KeyCode.Escape) && transform.Find("StoreCanvas") == null)
        {
            MenuOpen();
        }

        // UI(상점)가 열린 상태이고 Crosshair가 활성화된 상태이면
        if (isUIOpen && crosshairPanel.activeSelf)
            // Crosshair를 비활성화한다.
            crosshairPanel.SetActive(false);
        // UI가 닫힌 상태인데 Crosshair가 비활성화된 상태이면
        else if (!isUIOpen && !crosshairPanel.activeSelf)
            // Crosshair를 활성화한다.
            crosshairPanel.SetActive(true);


        #region Editor Test Code
#if UNITY_EDITOR
        // 무기 변경 테스트 코드
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            playerClass = PlayerClass.ePlayerClass.Soldier;
            classDict = null;
            StartCoroutine(CoPlayerClassSetting());
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            playerClass = PlayerClass.ePlayerClass.Medic;
            classDict = null;
            StartCoroutine(CoPlayerClassSetting());
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            playerClass = PlayerClass.ePlayerClass.Engineer;
            classDict = null;
            StartCoroutine(CoPlayerClassSetting());
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            PlayerWeaponChange(weaponManager.currGun.weaponDict["WeaponUID"]);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SpwanManager.Instance.EnemySpawnDebug();
        }
        else if (Input.GetKeyDown(KeyCode.Backslash))
        {
            _point = 10000;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            _playerExp += 90;
            CheckLevelUp();

        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            skillPoint -= 1;
            if (skillPoint < 0) { skillPoint = 0; }
        }

#endif
        #endregion


        #region Build Cheat

#if UNITY_STANDALONE_WIN
        if (Input.GetKeyDown(KeyCode.F6))
        {
            useCheat = true;
        }
        if (useCheat)
        {
            // 직업 설정 테스트 코드
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                playerClass = PlayerClass.ePlayerClass.Soldier;
                classDict = null;
                StartCoroutine(CoPlayerClassSetting());
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                playerClass = PlayerClass.ePlayerClass.Medic;
                classDict = null;
                StartCoroutine(CoPlayerClassSetting());
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                playerClass = PlayerClass.ePlayerClass.Engineer;
                classDict = null;
                StartCoroutine(CoPlayerClassSetting());
            }
            // 포인트 획득
            else if (Input.GetKeyDown(KeyCode.Backslash))
            {
                _point = 10000;
            }
            // 레벨 증가
            else if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                _playerExp += 90;
                CheckLevelUp();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                SpwanManager.Instance.EnemySpawnDebug();
            }


        }
#endif
        #endregion

    }

    // 체력 감소 테스트 변수
    //float damageDelay = 5f;
    //float damageTime = 0f;
    private void LateUpdate()
    {
        // 커서가 고정이 된 상태에서만 플레이어가 회전하도록 처리.
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            // Player Rotation 함수 실행
            PlayerRotation();
            // Upper Body Rotation 함수 실행
            UpperBodyRotation();
            // Player Camera Move 함수 실행
            //PlayerCameraMove(); // 기능 제거
        }
    }



    #region 플레이어 스킬 관련 
    /// <summary>
    /// 플레이어가 스킬을 획득시 변경되는 값들을 설정하는 함수
    /// </summary>
    /// <param name="_playerSkillUID">획득한 스킬 UID</param>
    /// <param name="_skillLevel">획득한 스킬의 Level</param>
    void PlayerSkillSetting(string _playerSkillUID, int _skillLevel)
    {
        int firstUID;
        int middleUID;
        int lastUID;
        // _skillUID.Substring(startIdx, endIdx);
        try
        {
            //Debug.Log("___UID: " + _playerSkillUID + "___");
            firstUID = int.Parse(_playerSkillUID.Substring(0, 2));
            //Debug.Log("____UID: " + firstUID + "____");
            if (firstUID != 03)
            {
                //Debug.Log("____Worng Skill UID Input____");
                return;
            }

            middleUID = int.Parse(_playerSkillUID.Substring(2, 3));
            //Debug.Log("____UID: " + middleUID + "____");
            lastUID = int.Parse(_playerSkillUID.Substring(5, 4));
            //Debug.Log("____UID: " + lastUID + "____");
        }
        catch (System.Exception e)
        {
            Debug.LogWarning(e);
            return;
        }

        // 플레이어 스킬 정보
        Dictionary<string, string> __skillInfo = DBManager.Instance.GetPlayerSkill(_playerSkillUID);

        string _name = __skillInfo["PlayerSkill_Name"];
        string _skillUID = __skillInfo["PlayerSkill_SkillUID"];
        float _coefficient = float.Parse(__skillInfo["PlayerSkill_Coefficient"]);


        switch (middleUID)
        {
            //FindPlayerSkill.GetPlayerSkill(__skillInfo["PlayerSkill_Name"], __skillInfo["PlayerSkill_SkillUID"], _skillLevel)[0] * float.Parse(__skillInfo["PlayerSkill_Coefficient"])
            case 000:
                switch (lastUID)
                {
                    case 0000:
                        // Increase Weapon Damage
                        if (playerSkillManager.status0_Level > statusMaxLevel) { return; }
                        addAttack = FindPlayerSkill.GetPlayerSkill(_name, _skillUID, _skillLevel)[0] * _coefficient;

                        //Debug.Log("Add Attack: " + addAttack);

                        break;
                    case 0001:
                        // Increase Player Armor
                        if (playerSkillManager.status1_Level > statusMaxLevel) { return; }
                        addArmour = FindPlayerSkill.GetPlayerSkill(_name, _skillUID, _skillLevel)[0] * _coefficient;

                        //Debug.Log("Add Armour: " + addArmour);

                        break;
                    case 0002:
                        // Increase Player Max HP
                        if (playerSkillManager.status2_Level > statusMaxLevel) { return; }
                        addHP = FindPlayerSkill.GetPlayerSkill(_name, _skillUID, _skillLevel)[0] * _coefficient;
                        //Debug.Log("___Max Hp: " + maxHp);
                        HPGaugeChange();
                        break;
                    default:
                        //Debug.Log("__Wrong UID Input__");

                        break;
                }

                break;
            case 001:
                switch (lastUID)
                {
                    case 0000:
                        // Incrase Max Carry Bullet
                        if (playerSkillManager.ability0_Level > abilityMaxLevel) { return; }
                        incCarryBullet = FindPlayerSkill.GetPlayerSkill(_name, _skillUID, _skillLevel)[0] * _coefficient;
                        //Debug.Log(playerSkillManager.status0_Level);
                        //Debug.Log("___Inc Carry Bullet: " + incCarryBullet + "___");

                        break;
                    case 0001:
                        // Increase Weapon Attack Speed
                        if (playerSkillManager.ability1_Level > abilityMaxLevel) { return; }
                        incAttackSpeed = FindPlayerSkill.GetPlayerSkill(_name, _skillUID, _skillLevel)[0] * _coefficient;
                        //Debug.Log(playerSkillManager.status0_Level);
                        //Debug.Log("___Inc Attack Speed: " + incAttackSpeed + "___");

                        break;
                    case 0002:
                        // Increase Item Healing Point
                        if (playerSkillManager.ability0_Level > abilityMaxLevel) { return; }
                        playerAction.incHealingPoint = FindPlayerSkill.GetPlayerSkill(_name, _skillUID, _skillLevel)[0] * _coefficient;
                        //Debug.Log("___INC HEALING POINT: " + playerAction.incHealingPoint + "___");

                        break;
                    case 0003:
                        // Increase Healing Item Use Speed
                        if (playerSkillManager.ability1_Level > abilityMaxLevel) { return; }
                        playerAction.incHealingSpeed = FindPlayerSkill.GetPlayerSkill(_name, _skillUID, _skillLevel)[0] * _coefficient;
                        //Debug.Log("___INC HEALING SPEED: " + playerAction.incHealingSpeed + "___");

                        break;
                    case 0004:
                        // Increase Build Speed
                        if (playerSkillManager.ability0_Level > abilityMaxLevel) { return; }
                        playerAction.incBuildSpeed = FindPlayerSkill.GetPlayerSkill(_name, _skillUID, _skillLevel)[0] * _coefficient;
                        //Debug.Log("___INC BUILD SPEED: " + playerAction.incBuildSpeed + "____");

                        break;
                    case 0005:
                        // Increase Repair Speed
                        if (playerSkillManager.ability1_Level > abilityMaxLevel) { return; }
                        playerAction.incRepairSpeed = FindPlayerSkill.GetPlayerSkill(_name, _skillUID, _skillLevel)[0] * _coefficient;
                        //Debug.Log("___INC REPAIR SPEED: " + playerAction.incRepairSpeed + "___");

                        break;
                    default:
                        //Debug.Log("__Wrong UID Input__");

                        break;
                }

                break;
            case 002:
                switch (lastUID)
                {
                    case 0000:
                        if (playerSkillManager.perk0_Level > perkMaxLevel) { return; }
                        // Increase Attack Speed Perk
                        incAttackSpeed_Perk = FindPlayerSkill.GetPlayerSkill(_name, _skillUID, _skillLevel)[0] * _coefficient;
                        StartCoroutine(weaponManager.WeaponStatusSetting());
                        //Debug.Log("___INC ATTACK SPEED PERK: " + incAttackSpeed_Perk + "___");

                        break;
                    case 0001:
                        // Dont Use Bullet Perk
                        if (playerSkillManager.perk1_Level > perkMaxLevel) { return; }
                        float _value = (FindPlayerSkill.GetPlayerSkill(_name, _skillUID, _skillLevel)[0]);
                        weaponManager.dontUseBulletPercent = _value * _coefficient;
                        weaponManager.dontUseBullet = (_value == 1f * _skillLevel) ? true : false;

                        //Debug.Log("___Dont Use Bullet: " + weaponManager.dontUseBullet + "___");

                        break;
                    case 0002:
                        // Increase Weapon Damage Perk
                        if (playerSkillManager.perk2_Level > perkMaxLevel) { return; }
                        List<float> _list = FindPlayerSkill.GetPlayerSkill(_name, _skillUID, _skillLevel);
                        attackPerk_Percent = _list[0];
                        addAttack_Perk = _list[1] * _coefficient;

                        //Debug.Log("___Attack Perk: " + addAttack_Perk + "____");


                        break;
                    case 0003:
                        // Increase Healing Point Perk
                        if (playerSkillManager.perk0_Level > perkMaxLevel) { return; }
                        // 회복 아이템의 회복량 증가
                        playerAction.incHealingPoint_Perk = FindPlayerSkill.GetPlayerSkill(_name, _skillUID, _skillLevel)[0] * _coefficient;

                        //Debug.Log("____ Healing Point Perk Increase ____");

                        break;
                    case 0004:
                        // Increase Healing Speed Perk 
                        if (playerSkillManager.perk1_Level > perkMaxLevel) { return; }
                        playerAction.incHealingSpeed_Perk = FindPlayerSkill.GetPlayerSkill(_name, _skillUID, _skillLevel)[0] * _coefficient;

                        //Debug.Log("____ Healing Speed Perk Increase ____");

                        break;
                    case 0005:
                        // Dont Use Healing Item Perk
                        if (playerSkillManager.perk2_Level > perkMaxLevel) { return; }
                        playerAction.dontUseHealingItem_Percent = FindPlayerSkill.GetPlayerSkill(_name, _skillUID, _skillLevel)[0] * _coefficient;

                        //Debug.Log("____ Healing Item Dont Use ____");

                        break;
                    case 0006:
                        // Increase Build Speed Perk
                        if (playerSkillManager.perk0_Level > perkMaxLevel) { return; }
                        playerAction.incBuildSpeed_Perk = FindPlayerSkill.GetPlayerSkill(_name, _skillUID, _skillLevel)[0] * _coefficient;

                        //Debug.Log("____ Build Speed Up Perk ____" + playerAction.incBuildSpeed_Perk);

                        break;
                    case 0007:
                        // Increase Building Max Health Point Perk
                        if (playerSkillManager.perk1_Level > perkMaxLevel) { return; }
                        playerAction.incBuildMaxHealthPoint = FindPlayerSkill.GetPlayerSkill(_name, _skillUID, _skillLevel)[0] * _coefficient;

                        //Debug.Log("___ Building Max Health Point Incrase ____" + playerAction.incBuildMaxHealthPoint);

                        break;
                    case 0008:
                        // Building Auto Repair Perk
                        if (playerSkillManager.perk2_Level > perkMaxLevel) { return; }
                        playerAction.buildingAutoRepair = (FindPlayerSkill.GetPlayerSkill(_name, _skillUID, _skillLevel)[0] == 1);

                        //Debug.Log("____ Building Auto Repair ____" + playerAction.buildingAutoRepair);

                        break;
                    default:
                        //Debug.Log("__Wrong UID Input__");

                        break;
                }

                break;
            default:
                //Debug.Log("__Wrong UID Input__");
                //Debug.Log("Middle UID: " + middleUID);
                //Debug.Log("Last UID: " + lastUID);

                break;
        }

    }

    /// <summary>
    /// 동일한 UID를 가지는 스킬의 Level을 증가시키는 함수
    /// </summary>
    /// <param name="_skillUID">Level을 증가시킬 스킬의 UID</param>
    internal void SkillLevelUp(string _skillUID)
    {

        // 동일한 이름의 UID를 찾고
        // 각 스킬이 스킬의 최대 레벨이 아닐 경우 레벨을 증가시키고 증가 값에 따라 세팅을 한다.
        if (_skillUID == classDict["StatusSkill0_UID"])
        {
            if (playerSkillManager.status0_Level < statusMaxLevel) { playerSkillManager.status0_Level++; }
            PlayerSkillSetting(classDict["StatusSkill0_UID"], playerSkillManager.status0_Level);
        }
        else if (_skillUID == classDict["StatusSkill1_UID"])
        {
            if (playerSkillManager.status1_Level < statusMaxLevel) { playerSkillManager.status1_Level++; }
            PlayerSkillSetting(classDict["StatusSkill1_UID"], playerSkillManager.status1_Level);
        }
        else if (_skillUID == classDict["StatusSkill2_UID"])
        {
            if (playerSkillManager.status2_Level < statusMaxLevel) { playerSkillManager.status2_Level++; }
            PlayerSkillSetting(classDict["StatusSkill2_UID"], playerSkillManager.status2_Level);
        }
        else if (_skillUID == classDict["AbilitySkill0_UID"])
        {
            if (playerSkillManager.ability0_Level < abilityMaxLevel) { playerSkillManager.ability0_Level++; }
            PlayerSkillSetting(classDict["AbilitySkill0_UID"], playerSkillManager.ability0_Level);
        }
        else if (_skillUID == classDict["AbilitySkill1_UID"])
        {
            if (playerSkillManager.ability1_Level < abilityMaxLevel) { playerSkillManager.ability1_Level++; }
            PlayerSkillSetting(classDict["AbilitySkill1_UID"], playerSkillManager.ability1_Level);
        }
        else if (_skillUID == classDict["Perk0_UID"])
        {
            if (playerSkillManager.perk0_Level < perkMaxLevel) { playerSkillManager.perk0_Level++; }
            PlayerSkillSetting(classDict["Perk0_UID"], playerSkillManager.perk0_Level);
        }
        else if (_skillUID == classDict["Perk1_UID"])
        {
            if (playerSkillManager.perk1_Level < perkMaxLevel) { playerSkillManager.perk1_Level++; }
            PlayerSkillSetting(classDict["Perk1_UID"], playerSkillManager.perk1_Level);
        }
        else if (_skillUID == classDict["Perk2_UID"])
        {
            if (playerSkillManager.perk2_Level < perkMaxLevel) { playerSkillManager.perk2_Level++; }
            PlayerSkillSetting(classDict["Perk2_UID"], playerSkillManager.perk2_Level);
        }

        _select_SkillList.Clear();

        if (skillPoint >= 1)
        {
            // 스킬 포인트가 남아있으면 다시 스킬 선택 UI를 표시하게 한다.
            StartCoroutine(SelectSkill());
        }

    }


    // 플레이어의 스킬 중에 최대 레빌이 아닌 3개의 스킬을 랜덤으로 뽑아서 List<string> 형식으로 UID 값을 반환한다.
    internal IEnumerator SelectSkill()
    {
        if (playerSkillManager.skillSettingComplete == true)
        {
            yield break;
        }

        // 레벨 업 가능한 스킬 리스트
        List<int> _skillList = new List<int>();
        // 선택한 스킬 리스트
        List<int> _selectList = new List<int>();

        if (playerSkillManager.status0_Level < statusMaxLevel)
        {
            _skillList.Add(0);
        }
        if (playerSkillManager.status1_Level < statusMaxLevel)
        {
            _skillList.Add(1);
        }
        if (playerSkillManager.status2_Level < statusMaxLevel)
        {
            _skillList.Add(2);
        }
        if (playerSkillManager.ability0_Level < abilityMaxLevel)
        {
            _skillList.Add(3);
        }
        if (playerSkillManager.ability1_Level < abilityMaxLevel)
        {
            _skillList.Add(4);
        }

        // Perk은 레벨 증가로 활성화하는 스킬이 아니다.
        /*
        if (playerSkillManager.perk0_Level < perkMaxLevel)
        {
            _skillList.Add(5);
        }
        if (playerSkillManager.perk1_Level < perkMaxLevel)
        {
            _skillList.Add(6);
        }
        if (playerSkillManager.perk2_Level < perkMaxLevel)
        {
            _skillList.Add(7);
        }
        */

        for (int i = 0; i < 3; i++)
        {
            int _rand = _skillList[UnityEngine.Random.Range(0, _skillList.Count)];
            switch (_rand)
            {
                case 0:
                    // Status0 스킬 UID 추가
                    _select_SkillList.Add(classDict["StatusSkill0_UID"]);

                    break;
                case 1:
                    // Status1 스킬 UID 추가
                    _select_SkillList.Add(classDict["StatusSkill1_UID"]);

                    break;
                case 2:
                    // Status2 스킬 UID 추가
                    _select_SkillList.Add(classDict["StatusSkill2_UID"]);

                    break;
                case 3:
                    // Ability0 스킬 UID 추가
                    _select_SkillList.Add(classDict["AbilitySkill0_UID"]);

                    break;
                case 4:
                    // Ability1 스킬 UID 추가
                    _select_SkillList.Add(classDict["AbilitySkill1_UID"]);

                    break;
                // Perk은 레벨 증가로 활성화하는 스킬이 아니다.
                /*
            case 5:
                // Perk0 스킬 UID 추가
                _select_SkillList.Add(classDict["Perk0_UID"]);

                break;
            case 6:
                // Perk1 스킬 UID 추가
                _select_SkillList.Add(classDict["Perk1_UID"]);

                break;
            case 7:
                // Perk 스킬 UID 추가
                _select_SkillList.Add(classDict["Perk2_UID"]);

                break;
                */
                default:
#if UNITY_EDITOR
                    //Debug.LogWarning("____Out of Range____");
#endif
                    break;
            }
            yield return null;
        }

        foreach (var _skill in _select_SkillList)
        {

            //Debug.Log($"Skill List: {_skill}");
        }

        playerSkillManager.skillSettingComplete = true;

        StartCoroutine(playerSkillManager.SelectSkillSetting());
    }



    #endregion

    #region 플레이어의 움직임을 컨트롤 하는 영역
    /// <summary>
    /// 플레이어가 앉는 동작을 실행(시도)하는 함수
    /// 사용하지 않는 함수.
    /// </summary>
    private void TryCrouch()
    {
        // Left Ctrl 키가 눌리면 실행, doCrouch가 true일 경우 실행
        if (Input.GetKey(KeyCode.LeftControl) || doCrouch)
        {
            // doCrouch를 false로
            doCrouch = false;
            // 앉아있으면 서고 서있으면 앉게 변경한다.
            isCrouch = !isCrouch;

            // 앉아 있으면
            if (isCrouch)
            {
                // 앉아있을 때의 속도로 설정
                changedSpeed = crouchSpeed;
                //cameraPosition.y -= cameraMoveValue;
            }
            // 서 있으면
            else
            {
                changedSpeed = walkSpeed;
                //cameraPosition.y += cameraMoveValue;
            }

        }
    }

    /// <summary>
    /// 플레이어의 움직임을 조절하는 함수
    /// </summary>
    private void PlayerMove()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // 왼쪽 shift가 눌리면 달린다.
        if (Input.GetKey(KeyCode.LeftShift))
        {
            //// 앉아있으면 일어서게 한다.
            //// 사용하지 않게 되었다.
            //if (isCrouch == true)
            //{
            //    doCrouch = true;
            //    TryCrouch();
            //}
            // 일어서기만 하면 이동 속도가 walkSpeed가 되기때문에
            // 이동속도를 runSpeed로 설정한다.
            changedSpeed = runSpeed;

        }
        // 왼쪽 shift가 눌리지 않았고 일어서있는 경우 이동속도를 walkSpeed로 설정한다.
        else if (isCrouch == false)
        {
            // 걷는 속도로 변경한다.
            changedSpeed = walkSpeed;
        }

        // 이동하는 방향 벡터를 설정.
        Vector3 dir = new Vector3(h, 0f, v);
        // dir을 크기가 1인 벡터로 만든다.
        dir.Normalize();

        playerAnim.SetFloat("Horizontal", dir.x);
        playerAnim.SetFloat("Vertical", dir.z);

        // 플레이어가 움직이는 속도가 변화했을 때 바로 변하는게 아니라
        // 선형 보간을 통해서 천천히 변화한다.
        useSpeed = Mathf.Lerp(useSpeed, changedSpeed, Time.deltaTime * motionChangeSpeed);
        // 사용하는 속도와 목표 속도가 거의 동일하면 useSpeed를 changedSpeed로 맞춰준다.
        if (useSpeed != changedSpeed && Mathf.Abs((useSpeed - changedSpeed) / changedSpeed) <= 0.1f)
        {
            useSpeed = changedSpeed;
        }

        // 이거 찾아봐야 하고
        dir = transform.TransformDirection(dir);

        //dir *= useSpeed;
        dir *= useSpeed * Time.deltaTime;

        // dir의 크기가 0.01보다 작으면 움직이는게 아니고
        // 0.01보다 크면 움직이는 것이다.
        // 위에서 dir을 크기가 1인 벡터로 만들도록 했으므로 방향키를 누르면 일단 움직인다고 판단한다.
        if (dir.magnitude >= 0.01f) { isMove = true; }
        else { isMove = false; }

        // Animation 변경에 필요한 값을 Animator로 Set
        playerAnim.SetBool("IsMove", isMove);
        playerAnim.SetBool("IsCrouch", isCrouch);
        playerAnim.SetFloat("Speed", useSpeed);

        //Debug.Log(dir);
        dir.y = gravity;
        // 이거 찾아봐야 하고
        controller.Move(dir);
    }

    /// <summary>
    /// Player 회전 함수
    /// </summary>
    private void PlayerRotation()
    {
        // 마우스의 회전 값을 입력 (값 >> - 1, 0, 1
        float yRotation = Input.GetAxisRaw("Mouse X");
        // 캐릭터가 회전하는 값을 LookSensitivity를 곱하여 설정
        Vector3 characterRotationY = new Vector3(0f, yRotation, 0f) * lookSensitivity;

        // 자기 자신을 기준으로 회전
        tr.Rotate(characterRotationY, Space.Self);

        //myRb.MoveRotation(myRb.rotation * Quaternion.Euler(characterRotationY));
    }

    /// <summary>
    /// 플레이어 상체 회전(위 아래) 함수
    /// </summary>
    private void UpperBodyRotation()
    {
        // 마우스의 회전 값을 입력
        float rotation = Input.GetAxisRaw("Mouse Y");
        // 회전하는 값을 lookSensitivity를 곱하여 설정
        float bodyRotation = rotation * lookSensitivity / 2.32f;

        // 현재 회전 값에서 마우스가 이동한 값만큼 이동
        upperBodyRotation -= bodyRotation;
        // 이동의 최대치와 최소치를 upperBodyRoationLimit로 설정
        upperBodyRotation = Mathf.Clamp(upperBodyRotation, -upperBodyRotationLimit, upperBodyRotationLimit);
        // upperBodyRotation으로 넣으면 상 하 반전 있음.
        playerAnim.SetFloat("Looking", -upperBodyRotation);

        //playerCameraTr.localRotation = Quaternion.Euler(new Vector3(upperBodyRotation * 0.75f, playerCameraTr.rotation.y, playerCameraTr.localRotation.z));
        // 자연스럽게 카메라가 위 아래를 보도록 값을 보정
        playerCameraTr.localRotation = Quaternion.Euler(new Vector3(upperBodyRotation * 0.85f, -12.5f, 0));

    }

    /// <summary>
    /// 플레이어의 카메라가 움직이는 함수(앉았다 일어설 때)<br/>
    /// 기능 제거
    /// </summary>
    private void PlayerCameraMove()
    {
        // Slerp, 구형 보간, Lerp, 선형 보간
        //playerCameraTr.localPosition = Vector3.Lerp(playerCameraTr.localPosition, cameraPosition, Time.deltaTime * cameraMoveSpeed);
    }

    #endregion

    #region 플레이어가 피해를 받을 때 발생하는 함수 영역
    /// <summary>
    /// 피해를 받을 때 호출되는 함수
    /// </summary>
    /// <param name="damage">받는 데미지</param>
    /// <param name="hitPoint">공격 받은 위치</param>
    /// <param name="hitNormal">공격 받은 위치의 노말 벡터</param>
    public override float Damaged(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        base.Damaged(damage - addArmour, hitPoint, hitNormal);

        StartCoroutine(ShowBloodScreen()); // 피격시 피격했다는 의미로 붉은 테두리가 깜빡인다.
        HPGaugeChange(); // HP 게이지를 변경시켜준다.
        if (currHP <= 0)
        {
            currHP = 0f;

        }
        return 0;
    }


    /// <summary>
    /// 피격시 붉은 테두리가 잠깐 생겼다가 사라진다.
    /// </summary>
    /// <returns></returns>
    IEnumerator ShowBloodScreen()
    {
        //Debug.Log("____SHOW BLOOD SCREEN____");
        //  BloodScreen 텍스처의 알파값을 불규칙하게 변경
        bloodScreen.color = new Color(1, 0, 0, UnityEngine.Random.Range(0.25f, 0.4f));
        yield return new WaitForSeconds(0.1f);
        //  BloodScreen 텍스처의 색상을 모두 0으로 변경
        bloodScreen.color = Color.clear;
    }


    /// <summary>
    /// 플레이어의 체력이 변경될 경우 체력 게이지를 변경시켜주는 함수<br/>
    /// 체력 숫자 값도 같이 변경시켜준다.
    /// </summary>
    private void HPGaugeChange()
    {
        playerHpImage.fillAmount = currHP / maxHp;
        playerHpText.text = string.Format($"<b>{currHP}</b>");

    }

    #endregion


    #region 아이템 세팅
    public bool ItemSetting(string _itemUid)
    {
        // 원래는 UID를 통해서 DBManager를 거쳐서 어떤 아이템을 가지고 있는지 확인을 하고
        // 아이템의 종류에 따라서 itemImg Sprite를 변경해주는게 맞는 것 같다.
        // 지금은 어떤 종류의 아이템을 가지고 있는지도 전부 bool 값을 사용해서 처리하고 있다.
        // 나중에 아이템을 사용할 때 어떤 아이템을 가지고 있는지 아는게 편리하므로 bool 값으로 설정하는 것이 맞을수도 있다.
        // 상황에 따라 다를 듯.

        _haveItemUID = _itemUid;
        int firstUID = 0;
        int middleUID = 0;
        int lastUID = 0;
        try
        {
            firstUID = int.Parse(_itemUid.Substring(0, 2));
            middleUID = int.Parse(_itemUid.Substring(2, 3));
            lastUID = int.Parse(_itemUid.Substring(5, 4));
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"____ Input item uid is Wrong {_itemUid} ____");
        }

        switch (firstUID)
        {
            // 아이템 타입
            case 6:
                switch (middleUID)
                {
                    // 아이템
                    case 0:
                        switch (lastUID)
                        {
                            // 회복 아이템
                            case 0:
                                //isHaveItem = true;
                                haveMedikit = true;

                                playerUI.ItemUISetting(lastUID);
                                break;
                            // 탄 보급
                            case 1:
                                weaponManager.currGun.carryBullet += weaponManager.currGun.maxCarryBullet;

                                break;
                            // 벙커 수리
                            case 2:
                                GameManager.instance.bunkerDoor.GetComponent<BunkerDoor>().Repair();

                                break;
                            default:

                                break;
                        }
                        break;
                    // Perk
                    case 1:
                        // Perk을 구매하면 Game Manager를 통해서 모든 "플레이어"를 찾아서 플레이어의 각 Perk을 활성화해야한다.
                        switch (lastUID)
                        {
                            // 1단계 특전 활성화
                            case 0:
                                if (playerSkillManager.perk0_Level >= 1)
                                    return false;
                                GameManager.instance.perk0_Active = true;
                                SkillLevelUp(classDict["Perk0_UID"]);

                                break;
                            // 2단계 특전 활성화
                            case 1:
                                if (playerSkillManager.perk1_Level >= 1)
                                    return false;
                                GameManager.instance.perk1_Active = true;
                                SkillLevelUp(classDict["Perk1_UID"]);

                                break;
                            // 3단계 특전 활성화
                            case 2:
                                if (playerSkillManager.perk2_Level >= 1)
                                    return false;
                                GameManager.instance.perk2_Active = true;
                                SkillLevelUp(classDict["Perk2_UID"]);

                                break;
                            default:
                                Debug.LogWarning("____ Wrong UID input: " + _itemUid);
                                break;

                        }

                        break;
                    default:


                        break;
                }
                break;
            // 방어물자 타입
            case 7:
                switch (middleUID)
                {
                    // 방어 물자
                    case 0:
                        //isHaveItem = true;
                        haveDefStruct = true;

                        Debug.Log("____ Last UID: " + lastUID + " ____");

                        // 마지막 UID 값을 넘겨주기 때문에 어떤 아이템인지 파악해서 처리할 수 있다.
                        // 이후 다른 아이템이 추가되면 코드가 전체적으로 바뀌어야할 수도 있다.
                        playerUI.ItemUISetting(lastUID);

                        break;
                    default:

                        break;

                }
                break;
            default:
                // 아이템을 사용하고 나면 이쪽으로 올 것이다.
                playerUI.ItemUISetting(0);

                break;
        }

        return true;
    }

    #endregion


    /// <summary>
    /// 체력 회복할 때 실행하는 함수
    /// </summary>
    /// <param name="_healingPoint">회복할 양</param>
    public void Healing(float _healingPoint)
    {
        //isHaveItem = false;

        // 회복할 양만큼 증가시킨 다음 
        currHP += _healingPoint;
        // 최대체력보다 많으면 최대체력으로 설정해준다.
        if (currHP >= maxHp)
        {
            currHP = maxHp;
        }

        HPGaugeChange();
    }


    // 이러한 방식으로 PlayerCtrl을 거쳐서 PlayerUI가 가지고 있는
    // PlayerActionTextSetting에 접근하게 한다.
    // 지금은 대부분이 public 으로 되어있어서 큰 차이가 없으나 playerUI의 경우 PlayerCtrl과 동일한 위치에 있으므로
    // getCompenent를 ㅌ옹해서도 받을 수 있다는 점을봤을 때
    // 이후 보안?을 높일때 private로 변경 후 진행할 수 있기 때문이다.
    public void ActionTextSetting(string _text)
    {
        playerUI.PlayerActionTextSetting(string.Format($"{_text.ToString()}"));
    }


    public void CheckLevelUp()
    {
        targetExp = (level == 1 ? 50f : 90f);


        if (level < 18 && _playerExp >= targetExp)
        {
            _playerExp -= targetExp;
            level += 1;
            skillPoint += 1;


            // 레벨이 올랐을 때 어떤 스킬을 획득할지 표시해주는 코루틴
            // 스킬을 획득하지 않은 상태이면 동작하지 않는다.
            StartCoroutine(SelectSkill());

            // having Skill Point 코루틴이 실행되고 있지 않을 때만 실행한다.
            if (playerUI.havingSkillPoint_isRunning == false)
            {
                //Debug.Log("Skill Point is Start");
                StartCoroutine(playerUI.HaveSkillPoint());
            }
        }

        playerUI.ExpUISetting();

    }


    protected override void Down()
    {
        isUIOpen = true;
        // 멀티 플레이가 되면 다른 플레이어의 상태를 체크하고 진행
        OnDeath();


    }

    public override void OnDeath()
    {
        base.OnDeath();
        dead = true;
        GameManager.instance.GameOver();

    }

    public void MenuOpen()
    {
        playerUI.menuPanel.gameObject.SetActive(!playerUI.menuPanel.gameObject.activeSelf);
        isUIOpen = playerUI.menuPanel.gameObject.activeSelf;
        CursorState.CursorLockedSetting(!isUIOpen);
    }

}
