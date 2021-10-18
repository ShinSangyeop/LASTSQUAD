using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


// Weapon Controller
public class WeaponManager : MonoBehaviour
{
    public PlayerCtrl playerCtrl; // 플레이어 컨트롤 스크립트
    public Transform playerCameraTr; // 플레이어 카메라 Transform
    public Transform leftHandTr; // 플레이어 왼손 위치
    private Transform weaponTr; //weapon Transform

    public CameraRaycast cameraRaycast; // 플레이어 카메라의 CameraRaycast

    private GameObject currWeaponObj; // 현재 총
    public Guns currGun; // 현재 총이 가지고 있는 Gun Script

    private bool isReload; // 플레이어가 재장전을 하고 있는가?

    private Dictionary<string, string> weaponDict = null; // 현재 무기의 정보를 저장하고 있는 dictionary
    private string weaponPath; // Resources폴더의 weapon이 있는 Path.

    public Animator anim;

    bool isFire = false; // 플레이어가 총을 발사하면 true가 된다.

    public float dontUseBulletPercent = 0f;
    // 총알 사용 안하는 스킬 활성화 여부
    public bool dontUseBullet = false;

    [Space(5)]
    [Header("Weapon Info")]
    [Space(2)]
    public Text weaponBulletText; // 현재 무기의 잔탄 텍스트
    public Text weaponNameText; // 현재 무기의 이름 텍스트


    #region LayerMask
    // Target으로 사용할 Layer
    LayerMask alllTargetLayerMask;

    #endregion


    private void Awake()
    {
        weaponPath = "Weapons/";
    }

    // Start is called before the first frame update
    void Start()
    {
        weaponTr = GetComponent<Transform>(); // 무기의 위치

        isReload = false; // 재장전 상태 false
                          //WeaponChange(string.Format("00000000")); // 무기 변경(기본 무기 설정)

        LayerMask enemyLayer = LayerMask.NameToLayer("ENEMY");
        LayerMask wallLayer = LayerMask.NameToLayer("WALL");
        LayerMask uiLayer = LayerMask.NameToLayer("UI");

        //alllTargetLayerMask = ((1 << enemyLayer) | (1 << wallLayer) | (1 << uiLayer));
        alllTargetLayerMask = ((1 << enemyLayer) | (1 << wallLayer));

    }

    // Update is called once per frame
    void Update()
    {
        //weaponTr.forward = playerCameraTr.forward; // 총의 정면과 플레이어 카메라의 정면을 동일하게 설정(어색하지 않게)
        TryReload(); // 재장전 시도 함수
        TryFire(); // 발사 시도 함수

        try
        {
            weaponTr.LookAt(leftHandTr);
        }
        catch (Exception e)
        {

        }
    }

    #region 무기 변경
    /// <summary>
    /// 무기 변경에 사용하는 함수
    /// </summary>
    /// <param name="UIDCode">무기 UID</param>
    public void WeaponChange(string UIDCode)
    {
        //Debug.Log(UIDCode);
        // UID 코드에 맞게 무기 가져옴.
        // UID 코드를 넘겨줬으니까 UID 코드에 맞는 무기를 DBManager를 통해서 찾고
        // 거기의 Weapon_Name을 사용해서 Prefab을 찾아야 한다.
        Dictionary<string, string> _weaponDict = null;
        while (_weaponDict == null)
        {
            _weaponDict = DBManager.Instance.GetWeaponInfo(UIDCode);
        }
        weaponDict = _weaponDict;

        // 현재 가지고 있는 무기가 있을 경우 무기를 제거하고
        if (currWeaponObj != null)
        {
            Destroy(currWeaponObj.gameObject);
        }

        //Debug.Log(weaponPath + weaponDict["Weapon_Name"]);
        // 새로 생성한 무기를 현재 무기로 만들어준다.
        //Debug.Log("____ Weapon Name: " + weaponDict["Weapon_Name"] + " ____");
        currWeaponObj = (GameObject)Instantiate(Resources.Load(weaponPath + weaponDict["Weapon_Name"]), this.transform);

        //currWeapon = weaponTr.GetChild(0).gameObject;
        //Debug.Log(currWeapon.name);
        // 현재 무기의 Guns 컴포넌트를 받아온다.
        currGun = currWeaponObj.GetComponent<Guns>();

        // 현재 무기의 rotation을 현재 weaponPos의 rotation으로 맞춰준다.(왼손쪽을 보는 회전)
        currWeaponObj.transform.rotation = weaponTr.rotation;
        //currWeapon.transform.Translate(weaponTr.localPosition - currGun.handleTr.localPosition);
        // 현재 총의 손잡이 부분을 오른손의 중앙에 맞게 이동
        currWeaponObj.transform.Translate(-currGun.handleTr.localPosition);


        //weaponTr.LookAt(leftHandTr);
        // 받아온 무기 정보 딕셔너리를 현재 총에 넘겨준다.
        currGun.weaponDict = this.weaponDict;


        StartCoroutine(WeaponStatusSetting());
        WeaponNameChange();
        WeaponBulletChange();
    }


    public IEnumerator WeaponStatusSetting()
    {
        yield return new WaitForSeconds(0.15f);
        // 최대 보유 총알 수 설정
        currGun.maxCarryBullet = Mathf.RoundToInt(float.Parse(weaponDict["Weapon_CarryBullet"]) * (1 + (playerCtrl.incCarryBullet * 0.01f)));
        //Debug.Log("___ Max Carry Bullet: " + currGun.maxCarryBullet + " ___");
        currGun.carryBullet = currGun.maxCarryBullet - currGun.reloadBullet;
        // 무기 공격 속도 증가
        //Debug.Log("Fire Delay: " + currGun.fireDelay);
        currGun.fireDelay = ((60 / float.Parse(weaponDict["Weapon_AttackSpeed"])) * (1 - (playerCtrl.currIncAttackSpeed * 0.01f)));
        //Debug.Log("Fire Delay: " + currGun.fireDelay);



    }


    #endregion

    #region 재장전
    /// <summary>
    /// 재장전 시도 함수
    /// </summary>
    private void TryReload()
    {
        // 재장전을 하는 중이면
        if (isReload == true)
        {
            return;
        }
        // R키를 눌렀을 경우 재장전 시도
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
        }
    }

    IEnumerator Reload()
    {
        // 현재 총알 수가 재장전 총알 수보다 작은 경우
        if (currGun.currBullet < currGun.reloadBullet)
        {
            //Debug.Log("Reload");
            // 가지고 있는 총알이 0발보다 많은 경우
            if (currGun.carryBullet > 0)
            {
                playerCtrl.ActionTextSetting("재장전 중");
                isFire = false;
                anim.SetBool("IsFire", isFire);
                //Debug.Log("Reload");
                // isReload를 true로 변경
                isReload = true;
                // 재장전 동작이 끝날 때까지 기다린다.
                yield return new WaitForSeconds(currGun.reloadTime);

                // 재장전에 소모될 총알 수
                int _reloadBullet = (currGun.reloadBullet - currGun.currBullet);
                // 현재 가지고 있는 총알 수가 재장전에 필요한 수보다 많으면
                if (currGun.carryBullet >= _reloadBullet)
                {
                    // 가지고 있는 총알에서 재장전에 필요한 수만큼 뺀다.
                    currGun.carryBullet -= _reloadBullet;
                    // 장전된 총알에 재장전 한 총알 수를 더한다.
                    currGun.currBullet += _reloadBullet; ;
                }
                // 가지고 있는 총알 수가 재장전에 필요한 수보다 적을경우
                else
                {
                    // 가지고 있는 총알 수만큼 더한다.
                    currGun.currBullet += currGun.carryBullet;
                    // 가지고 있는 총알을 0으로 한다. (전부 소모될 테니까)
                    currGun.carryBullet = 0;

                }


                //// 총 탄 관련 UI 갱신

            }
        }

        // 재장전을 하고 있는 상태가 아니다.
        isReload = false;
        //WeaponBulletChange();
    }

    #endregion

    #region 발사
    /// <summary>
    /// 발사를 시도하는 함수
    /// </summary>
    private void TryFire()
    {
        WeaponBulletChange();

        // 플레이어가 상점을 오픈한 상태이면 총을 발사하는 동작을 하지 못하도록 한다.
        if (playerCtrl.isUIOpen == true || playerCtrl.doAction == true)
        {
            return;
        }

        // 미완성 상태일 때 계속 에러가 나서 에러 발생시 그냥 함수 종료하게 함.
        // 재장전 중이 아닐 때
        if (isReload == false)
        {
            try
            {
                // currGun이 null일 경우 NullException Error가 발생한다.
                // 그러면 catch로 이동해서 함수를 종료.
                // 총을 쏘고나서 지난 시간은 계속 증가한다.
                currGun.fireTime += Time.deltaTime;
            }
            catch (Exception e)
            {
                //Debug.LogWarning(e);
                return;
            }

            // 마우스 좌클릭을 누른 경우
            if (Input.GetMouseButtonDown(0))
            {
                //Debug.Log("Try Fire");
                // 총을 쏠 수 있는 상황이면 >> 첫 발사
                if (CheckCanFire())
                {
                    // fireTime을 0으로 초기화해서 fireTime을 다시 계산하게 한다.
                    currGun.fireTime = 0f;
                }
            }
            // 마우스 좌클릭이 유지된 경우 >> 연속적인 발사
            else if (Input.GetMouseButton(0))
            {
                // 총을 쏠 수 있는 상황이면
                if (CheckCanFire())
                {
                    // fireTime에서 delay만큼 감소시켜 공격 속도보다 빠른 발사를 할 수 없게 한다.
                    currGun.fireTime = 0f;

                }
            }
            // 마우스 좌클릭을 떼었을 경우
            else if (Input.GetMouseButtonUp(0))
            {
                // 발사가 끝났으므로 isFire은 false
                isFire = false;
                // animation에 bool값을 설정
                anim.SetBool("IsFire", isFire);
                // 마우스에서 손을 떼도 현재 fireTime을 그대로 유지
                //// fireTime을 fireDelay로 만들어서 바로 발사할 수 있도록 설정.
                //currGun.fireTime = 0f;
            }
        }
    }

    /// <summary>
    /// 발사를 시도했을 때 발사할 수 있는 상황인지 판단하는 함수
    /// </summary>
    /// <returns>true: 가능 false: 불가능</returns>
    private bool CheckCanFire()
    {
        bool canFire = false;

        // 현재 장전된 총알이 1발 이상인 경우
        if (currGun.currBullet > 0)
        {
            //Debug.Log("Fire Delay On FIre: " + currGun.fireDelay);
            // 총 발사 딜레이보다 총을 쏘고 지난 시간이 더 크거나 같을 경우
            if (currGun.fireDelay <= currGun.fireTime)
            {
                // 레이 캐스트를 확인해서 판정하는 함수
                if (Cursor.lockState == CursorLockMode.None && CheckRaycastUI() == true)
                {
                    // UI를 타겟으로 하고 있고
                    // 커서가 고정 상태가 아닐 경우
                    // 발사 불가능한 상태로 한다.
                    return false;
                }


                isFire = true; // 총 발사가 가능한 경우
                anim.SetBool("IsFire", isFire);
                canFire = true;
                int percent = 0;
                if (dontUseBullet)
                {
                    percent = UnityEngine.Random.Range(0, 100);
                    //Debug.Log("____Percent: " + percent + "____");
                }
                if (!(percent >= (100 - dontUseBulletPercent)))
                {
                    // 총 발사를 진행
                    currGun.currBullet -= 1;
                }
                else
                {
                    //Debug.Log("___Bullet Dont Use!!____");
                }

                // 발사 이펙트 생성
                currGun.BulletFire();

                CheckFireRaycast();
            }
            else
            {
                isFire = false;
                anim.SetBool("IsFire", isFire);
            }
        }
        else
        {
            isFire = false; // 발사 불가능한 경우
            anim.SetBool("IsFire", isFire);
            // 장전된 총알이 0발인 경우 재장전을 수행한다.
            // 재장전 중이 아니기 때문에 재장전 중이라는 것을 알려주고,
            // 재장전 코루틴을 시작하면 된다.
            isReload = true;
            StartCoroutine(Reload());
        }


        return canFire;
    }


    private void CheckFireRaycast()
    {
        // 무기 사거리 내의 타겟 정보를 가져온다.
        List<RaycastHit> hitTargets = cameraRaycast.GetWeaponRaycastTarget(currGun.attackDistance, alllTargetLayerMask, currGun.gunType);
        GameObject target;

        //Debug.Log("____ HIT COUNT: " + hitTargets.Count + " ____");

        foreach (RaycastHit hitTarget in hitTargets)
        {
            try
            {
                target = hitTarget.transform.gameObject;

#if UNITY_EDITOR
                //Debug.Log("______ TARGET NAME: " + target.name);
#endif
                //Debug.Log("____ Target Layer: " + LayerMask.LayerToName(target.layer));
            }
            catch (System.Exception e)
            {
#if UNITY_EDITOR
                Debug.LogWarning("____ Target is Null: " + e);
#endif
                return;
            }

            //if (target == null) { return; }

            // Raycast 했을 때 대상이 무엇인가
            if (target.CompareTag("ENEMY"))
            {
                //Debug.Log("____ Gun Damage: " + currGun.damage + "____");
                playerCtrl._playerExp += target.GetComponent<LivingEntity>().Damaged(currGun.damage + playerCtrl.currAddAttack, hitTarget.point, hitTarget.normal);
                //Debug.Log($"____ EXP: {playerCtrl._playerExp} ____");
                playerCtrl.CheckLevelUp();

                BloodEffectCtrl _effect = PlayerEffectCtrl.GetBloodEffect();
                _effect.transform.position = hitTarget.point;
                _effect.transform.rotation = Quaternion.LookRotation(hitTarget.normal);

                // hitTarget.normal을 이용해서 만약 피 튀기는 이펙트를 만드려면 생성 방향을 저쪽으로 해주면 될 것 같다.
                //Debug.DrawRay(hitTarget.point, hitTarget.normal, Color.red, 20f);
            }
            else if (target.CompareTag("WALL"))
            {
                SparkEffectCtrl _effect = PlayerEffectCtrl.GetSparkEffect();
                _effect.gameObject.transform.position = hitTarget.point;
                _effect.gameObject.transform.rotation = Quaternion.LookRotation(hitTarget.normal);
                //Debug.Log("____ TARGET TAG WALL");
            }
            //else if (target.CompareTag("UI"))
            // 대상이 없는 상태일 때
            else
            {

            }
        }


        return;
    }


    /// <summary>
    /// Ray의 Target이 UI인지 확인하는 함수
    /// </summary>
    /// <returns></returns>
    private bool CheckRaycastUI()
    {
        bool lookUI = false;

        RaycastHit hit;

        Vector3 mousePos = Input.mousePosition;
        Camera camera = Camera.main;
        mousePos.z = camera.farClipPlane; // 카메라가 보는 방향과 시야를 가져온다.
        Vector3 dir = camera.ScreenToWorldPoint(mousePos);
        // 마우스를 고정 시켜도 마우스 위치랑 카메라 정면이랑 동일히자 않다.
        //if (Physics.Raycast(transform.position, transform.forward, out hit, _raycastRange, targetLayerMasks))
        if (Physics.Raycast(transform.position, dir, out hit, (1 << LayerMask.NameToLayer("UI"))))
        {
            lookUI = true;

        }

        return lookUI;

    }


    #endregion

    #region UI Setting
    /// <summary>
    /// 무기의 이름을 설정하여 UI에 표시하는 함수
    /// </summary>
    private void WeaponNameChange()
    {
        weaponNameText.text = weaponDict["Weapon_Name"];
    }

    /// <summary>
    /// 무기의 총알 수를 UI에 표시하는 함수
    /// </summary>
    private void WeaponBulletChange()
    {
        try
        {
            weaponBulletText.text = string.Format($"<b>{currGun.currBullet}</b> / <b>{currGun.carryBullet}</b>");
        }
        catch (System.Exception e)
        {
#if UNITY_EDITOR
            Debug.LogWarning("____ Weapon Change Exception: " + e + " ____");
#endif
        }
    }

    #endregion

}
