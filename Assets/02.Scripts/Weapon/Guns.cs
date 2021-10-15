using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guns : Weapon
{
    GameObject fireMuzzle; // 불꽃 이펙트 오브젝트
    string muzzlePath; // 불꽃 이펙트 오브젝트 프리팹의 위치

    public Transform firePos; // 총구 Transform
    public Transform handleTr; // 오른손이 잡고 있어야 하는 손잡이 위치
    private Transform playerTr; // 플레이어 오브젝트의 Transform

    public ItemGun.GunRarity gunRarity { get; private set; } // 무기의 레어도
    public ItemGun.GunType gunType { get; private set; } // 무기의 타입


    // Start is called before the first frame update    
    protected override void Awake()
    {
        base.Awake();

    }
    private void OnEnable()
    {
        muzzlePath = "FireMuzzle/Prefabs/";
        fireMuzzle = Resources.Load<GameObject>(muzzlePath + "MuzzleFlash");
        playerTr = transform.parent.parent.GetComponent<Transform>(); // player의 Transform

        fireMuzzle = Instantiate(fireMuzzle, firePos);
        fireMuzzle.SetActive(false);

    }

    private void Start()
    {
        StartCoroutine(CoWeaponTypeSetting());
    }


    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator CoWeaponTypeSetting()
    {
        while (weaponDict == null) { yield return null; }
        yield return new WaitForSeconds(1f);
        // 2번 요소부터 3개 >> 무기 종류 구분한 곳
        string __type = UID.Substring(2, 3);

        if (__type == "000")
        {
            gunType = ItemGun.GunType.Rifle;
        }
        else if (__type == "001")
        {
            gunType = ItemGun.GunType.SMG;
        }
        else if (__type == "002")
        {
            gunType = ItemGun.GunType.SG;
        }

    }


    // 발사 조건의 확인은 WeaponManager에서 한다.
    // 발사 동작을 하면 WeaponManager에서 BulletFire()를 시행한다.
    // Effect 및 Sound 등을 재생할 장소.
    /// <summary>
    /// 총이 발사되면 실행되는 함수
    /// </summary>
    public void BulletFire()
    {
        // 이미 MuzzleActive 코루틴이 실행되고 있으면 패스 아니면 실행
        if (muzzleActive == null)
        {
            muzzleActive = MuzzleActive();
            StartCoroutine(muzzleActive);
        }

    }

    // 코루틴을 할당할 변수
    IEnumerator muzzleActive = null;
    // 미리 총구에 할당한 fireMuzzle을 활성화했다가 잠시후 비활성화하는 코루틴
    // muzzle GameObject에 Animation이 있어서 활용할 수 있으면 좋겠는데
    // 조금 힘들듯 하다.
    IEnumerator MuzzleActive()
    {
        fireMuzzle.SetActive(true);
        yield return new WaitForSeconds(0.04f);
        fireMuzzle.SetActive(false);
        muzzleActive = null;
    }

}
