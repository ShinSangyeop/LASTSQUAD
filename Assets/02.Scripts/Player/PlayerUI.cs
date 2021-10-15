using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerUI : MonoBehaviour, UnityEngine.EventSystems.IPointerClickHandler
{
    public PlayerCtrl playerCtrl;
    public WeaponManager weaponManager;
    public PlayerAction playerAction;
    public PlayerSkillManager playerSkillManager;

    #region Status UI
    [Header("Player Status UI")]
    // 플레이어 스탯 UI
    public GameObject playerStatusUI;
    // 플레이어 스탯 UI가 열려있는가
    private bool statusUIisOpen = false;


    // 플레이어 이름 텍스트
    public Text nameText;
    // 플레이어 currHp / maxHp 텍스트
    public Text hpText;
    // 플레이어 직업 텍스트
    public Text classText;
    // 플레이어 레벨 텍스트
    public Text levelText;
    // 플레이어 방어력 텍스트
    public Text armourText;

    // 무기 이름 텍스트
    public Text weaponText;
    // 공격력 텍스트 weaponManage.currGun.damage + playerCtrl.incDamage
    public Text damageText;
    // 공격 속도 텍스트 weaponManager.currGun.fireDelay
    public Text attackSpeedText;

    // 아이템 회복량 playerAction.healingPoint
    public Text healingPointText;
    // 아이템 회복 속도 playerAction.healingSpeed
    public Text healingSpeedText;
    // 건설 속도 playerActioin.buildSpeed
    public Text buildSpeedText;
    // 수리 속도 playerAction.repairSpeed
    public Text repairSpeedText;
    // 방어 물자 자동 회복 playerAction.autoRepair
    public Text autoRepairText;

    Coroutine coUIUpdate;

    public Text pointText; // 보유 포인트 텍스트

    public Image expImage; // 플레이어 경험치 이미지

    #endregion

    #region Player Skill
    [Header("Player SKill")]
    // 스킬 포인트를 가지고 있다는 것을 알려주는 오브젝트
    public GameObject skillPointInfoObj;
    public bool havingSkillPoint_isRunning = false;

    // 획득할 수 있는 스킬을 표시해주는 오브젝트
    public GameObject skillSelectObj;
    public bool selectObjisOpen = false;

    // 획득할 수 있는 스킬의 정보를 표시해주는 Object 리스트
    public List<GameObject> skillInfo_ObjList = new List<GameObject>();

    // 획득할 수 있는 스킬의 정보를 표시해줄 Image와 Text
    public List<UnityEngine.UI.Image> skillInfo_ImageList = new List<Image>();
    public List<UnityEngine.UI.Text> skillInfo_TextList = new List<Text>();

    #endregion

    #region Player Item UI
    [Header("Player Item UI")]
    // 아이템이 있을 때 활성화 되는 Panel
    public Image itemPanel;
    // 아이템의 이미지
    public Image itemImg;

    #endregion

    #region Player Action UI
    [Header("Player Action UI")]
    // SerializeField를 사용한 Private
    // 혹은 private를 기반으로 Awake에서 프리팹 내부의 오브젝트를 찾아서 진행하는것이 일반적.
    public Image playerActionPanel;
    public Text playerActionText;

    IEnumerator IEnumActionText = null;

    #endregion

    #region Menu UI
    [Header("Menu UI")]
    public Image menuPanel;
    #endregion

    private void Start()
    {
        playerSkillManager = GetComponent<PlayerSkillManager>();

        statusUIisOpen = false;
        //statusUIisOpen = playerStatusUI.activeSelf;
        //StartCoroutine(StatusUIActive());

    }

    private void Update()
    {

        // O(o) 키를 누르면 스탯 UI 창을 켰다 껐다한다.
        if (Input.GetKeyDown(KeyCode.O))
        {
            statusUIisOpen = !statusUIisOpen;

            try
            {
                playerStatusUI.SetActive(statusUIisOpen);
            }
            catch (System.Exception e)
            {
#if UNITY_EDITOR
                Debug.LogWarning(e.GetType());
#endif
            }

            if (statusUIisOpen == true)
                coUIUpdate = StartCoroutine(StatusUIActive());
        }
        // 스킬 포인트를 가지고 있을 경우 스킬 선택 UI를 켜고
        // 스킬 선택 UI가 켜져있을 경우 끌 수 있게 한다.
        else if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (playerCtrl.skillPoint >= 1 && skillSelectObj.activeSelf == false)
            {
                //Cursor.lockState = UnityEngine.CursorLockMode.None; // 커서 고정을 끈다
                CursorState.CursorLockedSetting(false); // 커서 고정을 끈다.
                skillSelectObj.SetActive(true);
            }
            else if (skillSelectObj.activeSelf == true && playerCtrl.isUIOpen == false)
            {
                //Cursor.lockState = CursorLockMode.Locked; // 커서를 고정한다.
                CursorState.CursorLockedSetting(true); // 커서를 고정한다.
                skillSelectObj.SetActive(false);
            }

            selectObjisOpen = skillSelectObj.activeSelf;
        }
    }

    #region Skill Point UI

    /// <summary>
    /// 스킬 포인트를 가지고 있을 경우 Level 표시 근처에 켜졌다 꺼졌다하는 표시 갱신 코루틴. 
    /// <br/>스킬 포인트를 획득하면 실행된다.
    /// </summary>
    /// <returns></returns>
    public IEnumerator HaveSkillPoint()
    {
        float _alphaChange = 0.1f;
        havingSkillPoint_isRunning = true;

        Image _image = skillPointInfoObj.GetComponent<UnityEngine.UI.Image>();
        Color _color = _image.color;

        while (playerCtrl.skillPoint >= 1)
        {
            // UI Image의 color 값을 가져와서 alpha 값으 변경해준뒤 다시 대입하는 방식.
            // 직접 대입하는 방식이 되지 않는 것으로 알고 있다.
            _color.a += _alphaChange;
            _image.color = _color;

            // alpha 값은 0~1이고 최대로 차거나 최소로 줄어들면 증가하는 값이 반대로 바뀐다.
            if (_color.a >= 1f || _color.a <= 0f)
            {
                _alphaChange *= -1f;
            }

            yield return new WaitForSeconds(0.15f);
        }

        _color.a = 0f;
        _image.color = _color;

        // havingSkillPoint Coroutine이 끝났으므로 false로 만들어 준다.
        havingSkillPoint_isRunning = false;


        //Debug.Log($"Skill Point: Done");
    }
    #endregion

    #region Status UI 
    /// <summary>
    /// 스탯 UI를 활성화하고 갱신하는 코루틴
    /// </summary>
    /// <returns></returns>
    public IEnumerator StatusUIActive()
    {
        while (statusUIisOpen)
        {
            try
            {
                nameText.text = $"{playerCtrl.playerName.ToString()}";
                hpText.text = $"{playerCtrl.currHP.ToString()} / {playerCtrl.maxHp.ToString()}";

                string classToKorean;

                switch (playerCtrl.playerClass)
                {
                    case PlayerClass.ePlayerClass.Soldier:
                        classToKorean = "소총병";
                        break;
                    case PlayerClass.ePlayerClass.Medic:
                        classToKorean = "의무병";
                        break;
                    case PlayerClass.ePlayerClass.Engineer:
                        classToKorean = "공병";
                        break;
                    default:
                        classToKorean = "오류 발생";
                        break;
                }

                classText.text = $"{classToKorean.ToString()}";

                levelText.text = $"{playerCtrl.level.ToString()}";
                armourText.text = $"{playerCtrl.addArmour.ToString()}";
                weaponText.text = $"{weaponManager.weaponNameText.text.ToString()}";
                damageText.text = $"{(weaponManager.currGun.damage + playerCtrl.addAttack).ToString("F2")}";
                attackSpeedText.text = $"{weaponManager.currGun.fireDelay.ToString("F2")}";
                healingPointText.text = $"{playerAction.currHealingPoint.ToString("F2")}";
                healingSpeedText.text = $"{playerAction.currHealingSpeed.ToString("F2")}";
                repairSpeedText.text = $"{playerAction.currRepariSpeed.ToString("F2")}";
                buildSpeedText.text = $"{playerAction.currBuildSpeed.ToString("F2")}";

                string autoRepair = playerAction.buildingAutoRepair ? "보유" : "미보유";

                autoRepairText.text = $"{autoRepair.ToString()}";
                pointText.text = $"{playerCtrl._point.ToString()}";
            }
            catch (System.Exception e)
            {
#if UNITY_EDITOR
                Debug.LogWarning(e.GetType());
#endif
            }
            yield return new WaitForSeconds(0.5f);

        }

        yield break;
    }


    public void ExpUISetting()
    {
        expImage.fillAmount = playerCtrl._playerExp / playerCtrl.targetExp;
    }


    #endregion

    #region Button 기능

    public void OnPointerClick(UnityEngine.EventSystems.PointerEventData eventData)
    {
        bool select = false;

        //Debug.Log("____Press Event: " + eventData.pointerCurrentRaycast.ToString());
        if (eventData.pointerCurrentRaycast.gameObject == skillInfo_ObjList[0])
        {
            // 스킬 포인트를 1감소 시킨다.
            playerCtrl.skillPoint -= 1;
            // 스킬 레벨을 올릴 것이므로 스킬 세팅이 되었는가를 false로 변경한다.
            playerSkillManager.skillSettingComplete = false;
            //Debug.Log("____ GameObject : " + eventData.pointerCurrentRaycast.ToString());
            playerCtrl.SkillLevelUp(playerCtrl._select_SkillList[0]);

            select = true;
        }
        else if (eventData.pointerCurrentRaycast.gameObject == skillInfo_ObjList[1])
        {
            // 스킬 포인트를 1감소 시킨다.
            playerCtrl.skillPoint -= 1;
            // 스킬 레벨을 올릴 것이므로 스킬 세팅이 되었는가를 false로 변경한다.
            playerSkillManager.skillSettingComplete = false;
            //Debug.Log("____ GameObject : " + eventData.pointerCurrentRaycast.ToString());
            playerCtrl.SkillLevelUp(playerCtrl._select_SkillList[1]);

            select = true;
        }
        else if (eventData.pointerCurrentRaycast.gameObject == skillInfo_ObjList[2])
        {
            // 스킬 포인트를 1감소 시킨다.
            playerCtrl.skillPoint -= 1;
            // 스킬 레벨을 올릴 것이므로 스킬 세팅이 되었는가를 false로 변경한다.
            playerSkillManager.skillSettingComplete = false;
            //Debug.Log("____ GameObject : " + eventData.pointerCurrentRaycast.ToString());
            playerCtrl.SkillLevelUp(playerCtrl._select_SkillList[2]);

            select = true;
        }

        if (select)
        {
            // 레벨을 올릴 스킬을 선택했으므로 스킬 선택 창을 끈다.
            skillSelectObj.SetActive(false);

            // 스킬 창을 껐으므로 커서를 중앙에 다시 고정시킨다.
            // 스킬 포인트가 없어서 스킬 획득 창이 더 표시가 되지 않을 경우에만 동작한다.
            if (playerCtrl.skillPoint <= 0)
            {
                if (playerCtrl.isUIOpen == false)
                {
                    CursorState.CursorLockedSetting(true);
                }
            }
        }

    }

    #endregion

    #region Item UI
    public void ItemUISetting(int _lastUID)
    {

        if (playerCtrl.isHaveItem == true)
        {
            // Item Panel active
            itemPanel.gameObject.SetActive(true);

            if (playerCtrl.haveMedikit == true)
            {
                // Medikit Image acitve
                itemImg.sprite = Resources.Load<Sprite>("Store/ItemImage/MedikitImg");
            }
            else if (playerCtrl.haveDefStruct == true)
            {
                // DefStruct Image active
                // 철책
                if (_lastUID == 0)
                {
                    // Iron Fence image active
                    itemImg.sprite = Resources.Load<Sprite>("Store/DefensiveStructureImage/Fence");
                }
                // 철조망
                else if (_lastUID == 1)
                {
                    // Barbed Wire image active
                    itemImg.sprite = Resources.Load<Sprite>("Store/DefensiveStructureImage/BarbedWire");
                }
            }
        }
        else
        {
            // Item Panel inactive
            itemPanel.gameObject.SetActive(false);
        }


    }
    #endregion

    #region Player Action UI


    public void PlayerActionTextSetting(string _text)
    {
        // 플레이어 액션 텍스트가 실행되고 있으면 실행중인 것을 멈추고 실행
        if (IEnumActionText != null)
        {
            StopCoroutine(IEnumActionText);
        }
        // 새로운 텍스트를 넣어서 코루틴을 실행시킨다.
        IEnumActionText = CoActionTextSetting(_text);
        StartCoroutine(IEnumActionText);
    }

    IEnumerator CoActionTextSetting(string _text)
    {
        // 액션 텍스트를 표시할 Panel이 비활성화 되어있으면 활성화시킨다.
        if (playerActionPanel.gameObject.activeSelf == false)
            playerActionPanel.gameObject.SetActive(true);
        playerActionText.text = string.Format($"{_text.ToString()}");
        yield return new WaitForSeconds(1f);
        playerActionText.text = string.Format("");
        // 액션 텍스트를 표시할 Panel이 활성화되어있으면 비활성화한다.
        if (playerActionPanel.gameObject.activeSelf == true)
            playerActionPanel.gameObject.SetActive(false);
        // 코루틴이 끝났으므로 실행할 때 사용하는 변수를 null로 되돌린다.
        IEnumActionText = null;

    }

    #endregion 



}
