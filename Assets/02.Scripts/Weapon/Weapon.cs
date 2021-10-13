using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    protected string UID = null; // 무기 UID
    protected string weaponName = null; // 무기  이름

    public int currBullet = 0; // 현재 장전된 총알 수
    public int reloadBullet = 0; // 재정전 총알
    public int carryBullet = 0; // 현재 가지고 있는 총알 수
    public int maxCarryBullet = 0; // 최대 가지고 있을 수 있는 총알 수

    public int MaxCarryBullet { get; set; }

    public float damage = 0f; // 무기 데미지
    public float reloadTime = 0f; // 재장전 시간
    public float attackDistance = 0f; // 무기 공격 사거리
    protected float attackRange = 0f; // 공격 범위(원형)

    public float fireDelay = 0f; // 발사 딜레이 == attackSpeed
    public float fireTime = 0f; // 발사하고 지난 시간.

    // Weapon_UID / Weapon_Name / Weapon_Damage / Weapon_AttackSpeed / WeaponAttackDistance / Weapon_ReloadBullet
    // Weapon_CarryBullet / Weapon_ReloadTime / Weapon_AttackRange
    public Dictionary<string, string> weaponDict = null;

    protected virtual void Awake()
    {
        StartCoroutine(WeaponSetting());
    }

    /// <summary>
    /// 초기 무기 상태 값을 설정하는 코루틴<br/>
    /// </summary>
    /// <returns></returns>
    IEnumerator WeaponSetting()
    {
        while (weaponDict == null) { yield return null; }

        UID = weaponDict["Weapon_UID"];
        name = weaponDict["Weapon_Name"];

        reloadBullet = int.Parse(weaponDict["Weapon_ReloadBullet"]);
        maxCarryBullet = int.Parse(weaponDict["Weapon_CarryBullet"]);
        currBullet = reloadBullet;
        carryBullet = maxCarryBullet - currBullet;

        // float.TryParse(weaponDict["Weapon_Damage"], out damage); // 시도해서 파싱에 실패하면 false 반환 성공하면 true반환 , out 값
        damage = float.Parse(weaponDict["Weapon_Damage"]); // 바로 값을 넣는것
        reloadTime = float.Parse(weaponDict["Weapon_ReloadTime"]);
        attackDistance = float.Parse(weaponDict["Weapon_AttackDistance"]);
        attackRange = float.Parse(weaponDict["Weapon_AttackRange"]);

        //Debug.Log("Fire Delay On Weapon Scr: " + fireDelay);
        fireDelay = 60 / float.Parse(weaponDict["Weapon_AttackSpeed"]);
        //Debug.Log("Fire Delay On Weapon Scr: " + fireDelay);
        //Debug.Log(fireDelay);

        yield return null;

    }



}
