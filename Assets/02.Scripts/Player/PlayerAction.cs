using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 커서 상태를 설정하기 위해서 만든 클래스
/// </summary>
internal class CursorState
{
    /// <summary>
    /// 커서를 고정시키는 함수
    /// </summary>
    /// <param name="_state">true는 고정, false는 고정 해제</param>
    public static void CursorLockedSetting(bool _state)
    {    // Cursor.lockState = CorsorLockMode.None; // 커서 고정을 끈다
         // Cursor.lockState = CorsorLockMode.Confine; // 커서를 경계를 벗어나지 못하게 한다.
         //Cursor.visible = true; // 커서를 보이게 한다.
        if (_state)
        {
            Cursor.lockState = UnityEngine.CursorLockMode.Locked; // 커서를 고정한다.
            Cursor.visible = false; // 커서 보이지 않게 한다
        }
        else
        {
            Cursor.lockState = UnityEngine.CursorLockMode.None; // 커서 고정을 끈다
            Cursor.visible = true; // 커서를 보이게 한다.

        }
    }
}


public class PlayerAction : MonoBehaviour
{
    private Transform playerTr; // 플레이어의 Transform
    private PlayerCtrl playerCtrl; // 플레이어 Ctrl 스크립트

    ///<summary>
    /// CameraRaycast Script
    ///</summary>
    private CameraRaycast cameraRaycast; // 카메라 Transform

    #region 플레이어의 현재 동작 관련 변수
    public bool isBuild; // 플레이어가 빌딩을 건설하고 있는가?
    public bool isBuy; // 플레이어가 물건을 사고 있는가? 상점을 이용하고 있는가?
    public bool isHeal; // 플레이어가 회복 동작을 수행하고 있는가?
    public bool isRepair; // 플레이어가 수리 동작을 하고 있는가?

    private float healingPoint; // 회복 아이템 사용시 회복되는 기본량
    public float incHealingPoint; // 증가한 회복량
    public float incHealingPoint_Perk; // 증가한 회복량
    /// <summary>
    /// 회복 아이템 사용시 회복되는 현재 수치
    /// </summary>
    public float currHealingPoint { get { return healingPoint * (1 + ((incHealingPoint + incHealingPoint_Perk) * 0.01f)); } }

    private float healingSpeed; // 회복 아이템 사용시 걸리는 시간.
    public float incHealingSpeed; // 증가한 회복 아이템 사용 속도
    public float incHealingSpeed_Perk; // 증가한 회복 아이템 사용 속도
    /// <summary>
    /// 회복 아이템 사용 속도의 현재 수치
    /// </summary>
    public float currHealingSpeed
    {
        get
        {
            float _value = healingSpeed * (1 - ((incHealingSpeed + incHealingSpeed_Perk) * 0.01f));
            return ((_value >= 0.5f) ? _value : 0.5f);
        }
    }

    // if(dontUseHealingItem_Percent !=0)이면 실행
    // 회복 아이템을 사용하지 않을 확률
    public float dontUseHealingItem_Percent = 0f;

    private float buildSpeed; // 현재 건물 건설에 걸리는 시간
    public float incBuildSpeed; // 증가한 건물 건설 속도
    public float incBuildSpeed_Perk = 0f; // 증가한 방어물자 건설 속도 Perk
    /// <summary>
    /// 방어 물자 건설 시 걸리는 시간
    /// </summary>
    public float currBuildSpeed
    {
        get
        {
            float _value = buildSpeed * (1 - ((incBuildSpeed + incBuildSpeed_Perk) * 0.01f));

            return ((_value >= 2.5f) ? _value : 2.5f);
        }
    }

    private float repairSpeed; // 방어물자 수리 속도
    public float incRepairSpeed; // 증가한 건물 건설 속도
    /// <summary>
    /// 현재 방어물자 수리 속도
    /// </summary>
    public float currRepariSpeed
    {
        get
        {
            float _value = repairSpeed * (1 - (incRepairSpeed * 0.01f));
            return ((_value >= 0.5f) ? _value : 0.5f);
        }
    }

    // 건설할 방어물자의 증가할 최대 체력
    public float incBuildMaxHealthPoint = 0f;

    public bool buildingAutoRepair = false;

    #endregion


    private Animator playerAnim; // 플레이어의 애니메이터

    private float searchTime = 0f;
    private float searchDelay = 0.05f;
    private GameObject target = null; // 보고 있는 타겟
    string targetTag = null; // 보고 있는 타겟의 태그

    [Header("Target Information")]
    /// <summary>
    /// Player가 보고 있는 타겟의 정보를 표시해줄 Text
    /// </summary>
    public Text targetInfoText;
    /// <summary>
    /// Player가 보고 있는 타겟의 정보를 표시할 Panel
    /// </summary>
    public GameObject targetInfoPanel;

    [Space(5, order = 0)] // Inspector View 여백
    // Order >> Serialize중 출력 순서?
    [Header("Aim Point", order = 1)]
    public GameObject crosshair; // 플레이어 공격의 조준점
    public GameObject gaugeRing; // 특정 동작시 0에서 1까지 Gauge가 찰 Ring


    #region LayerMask
    // 타겟으로 사용할 Layer
    LayerMask allLayerMask;

    #endregion


    // Start is called before the first frame update
    void Start()
    {
        playerTr = transform.parent.GetComponent<Transform>();
        playerCtrl = playerTr.GetComponent<PlayerCtrl>();
        cameraRaycast = GetComponent<CameraRaycast>();

        isBuild = false;
        isBuy = false;
        isHeal = false;

        healingPoint = 50f;
        incHealingPoint = 0f;
        healingSpeed = 5f;
        incHealingSpeed = 0f;
        buildSpeed = 7.5f;
        incBuildSpeed = 0f;
        repairSpeed = 5f;
        incRepairSpeed = 0f;


        CursorState.CursorLockedSetting(true);

        LayerMask blueprint = LayerMask.NameToLayer("BLUEPRINT");
        LayerMask enemyLayer = LayerMask.NameToLayer("ENEMY");
        LayerMask defensiveGoodsLayer = LayerMask.NameToLayer("DEFENSIVEGOODS");
        LayerMask storeLayer = LayerMask.NameToLayer("STORE");
        LayerMask playerLayer = LayerMask.NameToLayer("PLAYER");
        LayerMask bunkerDoorLayer = LayerMask.NameToLayer("BUNKERDOOR");
        LayerMask wallLayer = LayerMask.NameToLayer("WALL");

        allLayerMask = (1 << blueprint) | (1 << enemyLayer) | (1 << defensiveGoodsLayer) | (1 << storeLayer) | (1 << playerLayer) | (1 << bunkerDoorLayer) | (1 << wallLayer);

    }

    // Update is called once per frame
    void Update()
    {
        if (playerCtrl.isUIOpen == false)
        {
            CheckLooking();
            Action();
        }
        else
        {
            if (targetInfoPanel.activeSelf == true)
            {
                targetInfoPanel.SetActive(false);
            }
        }


    }


    /// <summary>
    /// 플레이어가 보고 있는 위치게 어떤 물건이 있는지 확인하는 함수
    /// </summary>
    private void CheckLooking()
    {
        searchTime += Time.deltaTime;
        // searchDelay마다 보고 있는 대상 확인
        if (searchTime >= searchDelay)
        {
            searchTime -= searchDelay;

            try
            {
                target = (GameObject)cameraRaycast.GetRaycastTarget(10f, allLayerMask).transform.gameObject;
                targetTag = target.tag;
            }
            catch (NullReferenceException e)
            {
                //Debug.Log(e);
                targetTag = null;
                targetInfoPanel.SetActive(false);
                return;
            }
        }
    }

    /// <summary>
    /// target의 Tag에 따라 어떤 동작을 할지 결정하는 함수
    /// </summary>
    private void Action()
    {
        // 보고 있는 대상이 없는 경우 함수 바로 종료
        if (target == null)
        {
            GaugeClear();
            return;
        }

        //Debug.Log(target);

        // 방어 물자가 건설되어 있지 않은 상태이면 건설할 수 있다고 표시해주어야 한다.
        if (targetTag == "BLUEPRINT")
        {
            // 각각의 상황에 따로 SetActive를 처리하는 이유는 Raycast를 통해서 무언가를 비추고는 있는데
            // 내가 원하는 타겟이 아닌 경우가 있기 때문에 원하는 타겟일 경우에만 표시하도록 처리한 것.
            if (targetInfoPanel.activeSelf == false) { targetInfoPanel.SetActive(true); }


            bool canBuild = !target.GetComponent<Blueprint>().isBuild;

            if (playerCtrl.haveDefStruct == false)
            {
                targetInfoText.text = TargetInfoTextSetting("아이템이 필요합니다.");
            }
            else
            {
                if (target.GetComponent<Blueprint>()._uid != playerCtrl._haveItemUID)
                {
                    targetInfoText.text = TargetInfoTextSetting("아이템이 필요합니다.");
                }
                else
                {
                    // 건설된 상태인지 확인을 하고 건설된 상태가 아니면 건설 가능하다고 표시해준다.
                    targetInfoText.text = (TargetInfoTextSetting(canBuild ? "건설 가능" : "건설 완료"));
                    // 방어물자가 건설되어 있지 않은 상태이면 건설할 수 있다고 표시
                    // 건설된 상태이면? 수리 가능한 상황일 경우 따로 표시를 해서 수리 진행하게 처리.
                    //Debug.Log("Defensive Goods State");

                    if (canBuild)
                    {
                        // E를 누를경우 
                        if (Input.GetKey(KeyCode.E))
                        { // Build 과정 진행
                            isBuild = true;
                            // 건설할 수 있는 상황이면 진행
                            if (FillGauge(currBuildSpeed))
                            {
                                // 건설이 완료됐을 경우 실행.
                                // 가진 방어물자가 없다고 표시한다.
                                playerCtrl.haveDefStruct = false;
                                playerCtrl.ItemSetting("0"); // 가진 아이템이 없으므로 0을 넣어준다.

                                playerCtrl.ActionTextSetting("건설 완료");

                                target.GetComponent<Blueprint>().BuildingBuild();
                                // 건물 자동 회복 특전을 가지고 있을 경우
                                if (buildingAutoRepair == true)
                                {
                                    // 건물의 자동 회복을 활성화시킨다.
                                    target.GetComponent<Blueprint>().StartAutoRepair();
                                }

                            }
                        }
                        // 놓았을 경우
                        else if (Input.GetKeyUp(KeyCode.E))
                        {
                            isBuild = false;
                            GaugeClear();
                        }
                    }
                    else
                    {
                        // 건설이 완료된 경우 따로 정보를 표시할 것이 없다.
                        GaugeClear();
                        if (targetInfoPanel.activeSelf == true)
                        {
                            targetInfoPanel.SetActive(false);
                        }
                    }
                }
            }
        }
        else if (targetTag == "FENCE" || targetTag == "BARBEDWIRE")
        {
            // 각각의 상황에 따로 SetActive를 처리하는 이유는 Raycast를 통해서 무언가를 비추고는 있는데
            // 내가 원하는 타겟이 아닌 경우가 있기 때문에 원하는 타겟일 경우에만 표시하도록 처리한 것.
            if (targetInfoPanel.activeSelf == false) { targetInfoPanel.SetActive(true); }

            DefensiveStructure defSturct = target.GetComponent<DefensiveStructure>();
            // 수리할 수 있는 상황이면
            if (defSturct.currHP < defSturct.startHp)
            {
                targetInfoText.text = TargetInfoTextSetting("수리 필요");
                // 상호작용키 E를 누르고 있으면
                if (Input.GetKey(KeyCode.E))
                {
                    isRepair = true;
                    if (FillGauge(currRepariSpeed))
                    { // 수리 과정 진행
                        playerCtrl.ActionTextSetting("수리 완료");
                        defSturct.Repair();
                    }
                }
                else if (Input.GetKeyUp(KeyCode.E))
                {
                    isRepair = false;
                    GaugeClear();
                }
            }
            else
            {
                // 수리가 완료된 상태면 따로 표시할 것이 없다.
                GaugeClear();
                if (targetInfoPanel.activeSelf == true)
                {
                    targetInfoPanel.SetActive(false);
                }
            }
        }

        // 상점에 다가가면 상점이라고 표시가 뜨고 키를 누르면 상점이 열린다.
        else if (targetTag == "STORE" && Vector3.Distance(this.transform.position, target.transform.position) <= 5f)
        {
            if (targetInfoPanel.activeSelf == false) { targetInfoPanel.SetActive(true); }
            targetInfoText.text = TargetInfoTextSetting(targetTag);
            if (Input.GetKeyDown(KeyCode.E))
            {
                // Modify
                // OpenStore에서 열렸나 안열렸나로 반환값을 주는데 이때
                // 결과값을 이용해서 현재 동작에 대한 결과를 UI에 텍스트로 표시해주는 것이 필요한 것 같다.
                // 대부분의 동작에 UI로 표시해주는 과정이 있으면 보기 편하다.

                target.GetComponent<Store>().OpenStore(playerTr);
            }
        }
        // 플레이어에게 다가가면 플레이어의 이름이 표시된다.
        else if (targetTag == "PLAYER")
        {
            PlayerCtrl _targetPlayer = target.GetComponent<PlayerCtrl>();
            if (_targetPlayer.currHP < _targetPlayer.maxHp)
            {
                if (targetInfoPanel.activeSelf == false) { targetInfoPanel.SetActive(true); }
                targetInfoText.text = TargetInfoTextSetting("동료 회복 가능");
                if (Input.GetKey(KeyCode.E))
                {
                    isHeal = true;
                    if (FillGauge(healingSpeed))
                    {
                        playerCtrl.ActionTextSetting("회복 완료");
                        // 회복 아이템을 사용했으므로 haveMedikit을 false로
                        playerCtrl.haveMedikit = false;
                        // 아이템이 없으므로 넘기는 UID 값을 0으로 한다.
                        playerCtrl.ItemSetting("0");

                        // Healing 함수 안에 넣으면 회복되는 대상의 아이템을 사용해버리는 문제가 생긴다.
                        // 회복시키는 대상의 아이템을 사용해야한다.
                        //Debug.Log("____ 회복 ____");
                        _targetPlayer.Healing(currHealingPoint);


                    }
                }
                else if (Input.GetKeyUp(KeyCode.E))
                {
                    isHeal = false;
                    GaugeClear();
                }

            }
            //Debug.Log("Player Live State");
        }
        //else if(targetTag == "MAINDOOR")
        //{

        //}
        // 보고있는 대상은 있는데 그 대상이 내가 원하는 대상이 아닐 경우 정보 표시가 필요없다.
        else
        {
            // 보고 있는 대상이 없는데 체력이 최대 체력보다 낮을 경우
            // 회복 아이템이 있는 경우
            if (playerCtrl.haveMedikit && playerCtrl.currHP < playerCtrl.maxHp)
            {
                if (targetInfoPanel.activeSelf == false) { targetInfoPanel.SetActive(true); }
                targetInfoText.text = TargetInfoTextSetting("회복 가능");
                if (Input.GetKey(KeyCode.E))
                {
                    isHeal = true;
                    if (FillGauge(healingSpeed))
                    {
                        playerCtrl.ActionTextSetting("회복 완료");
                        // 회복 아이템을 사용했으므로 haveMedikit을 false로
                        playerCtrl.haveMedikit = false;
                        // 아이템이 없으므로 넘기는 UID 값을 0으로 한다.
                        playerCtrl.ItemSetting("0");

                        //Debug.Log("____ 회복 ____");
                        playerCtrl.Healing(currHealingPoint);
                    }
                }
                else if (Input.GetKeyUp(KeyCode.E))
                {
                    isHeal = false;
                    GaugeClear();
                }

            }
            else
            {
                GaugeClear();
                if (targetInfoPanel.activeSelf == true)
                {
                    targetInfoPanel.SetActive(false);
                }
            }
        }

    }

    #region Target Information UI
    /// <summary>
    /// Target의 Information Text를 Setting할 때 사용하는 함수
    /// </summary>
    /// <param name="_text">Change this text to bold</param>
    /// <returns>필요한 부분을 Bold체로 변경한 값을 반환</returns>
    private string TargetInfoTextSetting(string _text)
    {
        string _string = string.Format($"<b>{_text}</b>");
        return _string;
    }


    #endregion

    #region Gauge UI Control Function
    /// <summary>
    /// 특정 동작시 게이지를 채우기 위해서 동작하는 함수
    /// </summary>
    /// <param name="_chargingTime">게이지를 채우는데 걸리는 시간</param>
    /// <returns>게이지가 100%가 되면 true 아닐경우 false</returns>
    private bool FillGauge(float _chargingTime)
    {
        //Debug.Log("____FILL GAUGE____");
        if (crosshair.activeSelf == true) { crosshair.SetActive(false); }
        if (gaugeRing.activeSelf == false) { gaugeRing.SetActive(true); }
        Image ringImgae = gaugeRing.GetComponent<Image>();
        ringImgae.fillAmount += (1 / _chargingTime) * Time.deltaTime;

        if (ringImgae.fillAmount >= 1f) { return true; }

        return false;
    }

    /// <summary>
    /// FillGauge를 실행하던 중 취소되거나 완료 시 실행되는 함수.
    /// </summary>
    private void GaugeClear()
    {
        //Debug.Log("____GAUGE CLEAR____");
        // ring이 표시되어있는 상태일 경우 fillAmount를 0으로 해서 다음에 시도할때 0부터 차도록 한다.
        if (gaugeRing.activeSelf == true)
        {
            isHeal = false;
            isRepair = false;
            isBuild = false;

            gaugeRing.GetComponent<Image>().fillAmount = 0f;
            gaugeRing.SetActive(false);
        }
        // 마우스의 기본 표시는 crosshair이므로 기본으로 되돌려준다.
        if (crosshair.activeSelf == false) { crosshair.SetActive(true); }

    }
    #endregion



}
