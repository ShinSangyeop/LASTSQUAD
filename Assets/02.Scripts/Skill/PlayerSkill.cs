using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class PlayerSkill
{
    // 이거도 Invokde로 써야하나.

    /// <summary>
    /// 무기의 공격력이 증가하는 스킬<br/>
    /// 레벨 당 공격력이 1 증가한다.
    /// </summary>
    /// <param name="_skillLevel">Skill Level</param>
    /// <returns>Use.. Default weapon damage += return value</returns>
    public List<float> IncreaseWeaponDamage(string _skillName, int _skillLevel)
    {
        // 스킬 UID를 사용해서 Dictionary 형식으로 받아옴.
        // Dictionary<string, string> skill = GetPlayerSkillName(uid);
        // string skillName = skill["PlayerSkill_Name"];

        List<float> skillinfo = FindSkill.GetSkillInfo(_skillName);
        List<float> _return = new List<float>();
        _return.Add(skillinfo[0] * _skillLevel);

        // 데미지 증가량 반환
        return _return;
    }

    /// <summary>
    /// 방어력이 증가하는 스킬<br/>
    /// 레벨 당 방어력이 0.5 증가한다.
    /// </summary>
    /// <param name="_skillLevel">Skill Level</param>
    /// <returns>Use.. Default armour += return value</returns>
    public List<float> IncreasePlayerArmor(string _skillName, int _skillLevel)
    {
        // 스킬 UID를 사용해서 Dictionary 형식으로 받아옴.
        // Dictionary<string, string> skill = GetPlayerSkillName(uid);
        // string skillName = skill["PlayerSkill_Name"];

        List<float> skillinfo = FindSkill.GetSkillInfo(_skillName);
        List<float> _return = new List<float>();
        _return.Add(skillinfo[0] * _skillLevel);

        // 방어력 증가량 반환
        return _return;
    }

    /// <summary>
    /// 최대 체력이 증가하는 스킬<br/>
    /// 레벨 당 10 증가한다.
    /// </summary>
    /// <param name="_skillLevel"></param>
    /// <returns>Use.. Default max hp += return value</returns>
    public List<float> IncreasePlayerMaxHP(string _skillName, int _skillLevel)
    {
        // 스킬 UID를 사용해서 Dictionary 형식으로 받아옴.
        // Dictionary<string, string> skill = GetPlayerSkillName(uid);
        // string skillName = skill["PlayerSkill_Name"];

        List<float> skillinfo = FindSkill.GetSkillInfo(_skillName);
        List<float> _return = new List<float>();
        _return.Add(skillinfo[0] * _skillLevel);

        // 최대 체력 증가량 반환
        return _return;
    }

    /// <summary>
    /// 최대 보유 총알이 증가하는 스킬<br/>
    /// 레벨 당 15% 증가한다.
    /// </summary>
    /// <param name="_skillLevel"></param>
    /// <returns>Use.. Default max carry bullet *= (1 + (return value / 100f))</returns>
    public List<float> IncraseMaxCarryBullet(string _skillName, int _skillLevel)
    {
        // 스킬 UID를 사용해서 Dictionary 형식으로 받아옴.
        // Dictionary<string, string> skill = GetPlayerSkillName(uid);
        // string skillName = skill["PlayerSkill_Name"];

        List<float> skillinfo = FindSkill.GetSkillInfo(_skillName);
        List<float> _return = new List<float>();
        _return.Add(skillinfo[0] * _skillLevel);

        // 보유 총알 증가량 반환
        return _return;
    }

    /// <summary>
    /// 무기 공격 속도 증가하는 스킬<br/>
    /// 레벨 당 10% 증가한다.
    /// </summary>
    /// <param name="_skillLevel">Skill level</param>
    /// <returns>Use.. Default weapon attack speed *= (1 - (return value / 100f))</returns>
    public List<float> IncreaseWeaponAttackSpeed(string _skillName, int _skillLevel)
    {
        // 스킬 UID를 사용해서 Dictionary 형식으로 받아옴.
        // Dictionary<string, string> skill = GetPlayerSkillName(uid);
        // string skillName = skill["PlayerSkill_Name"];

        List<float> skillinfo = FindSkill.GetSkillInfo(_skillName);
        List<float> _return = new List<float>();
        _return.Add(skillinfo[0] * _skillLevel);

        // 보유 총알 증가량 반환
        return _return;
    }

    /// <summary>
    /// 회복 아이템 회복량 증가 스킬<br/>
    /// 레벨 당 15% 증가한다.
    /// </summary>
    /// <param name="_skillLevel"></param>
    /// <returns>Use.. Default healing point *= (1 + (return value / 100f))</returns>
    public List<float> IncreaseItemHealingPoint(string _skillName, int _skillLevel)
    {
        // 스킬 UID를 사용해서 Dictionary 형식으로 받아옴.
        // Dictionary<string, string> skill = GetPlayerSkillName(uid);
        // string skillName = skill["PlayerSkill_Name"];

        List<float> skillinfo = FindSkill.GetSkillInfo(_skillName);
        List<float> _return = new List<float>();
        _return.Add(skillinfo[0] * _skillLevel);

        // 보유 총알 증가량 반환
        return _return;
    }

    /// <summary>
    /// 회복 아이템 사용 속도 증가 스킬<br/>
    /// 레벨 당 10% 증가한다.
    /// </summary>
    /// <param name="_skillLevel"></param>
    /// <returns>Use.. Default healing speed *= (1 - (return value / 100f))</returns>
    public List<float> IncreaseHealingItemUseSpeed(string _skillName, int _skillLevel)
    {
        // 스킬 UID를 사용해서 Dictionary 형식으로 받아옴.
        // Dictionary<string, string> skill = GetPlayerSkillName(uid);
        // string skillName = skill["PlayerSkill_Name"];

        List<float> skillinfo = FindSkill.GetSkillInfo(_skillName);
        List<float> _return = new List<float>();
        _return.Add(skillinfo[0] * _skillLevel);

        // 보유 총알 증가량 반환
        return _return;
    }

    /// <summary>
    /// 건설 속도 증가하는 스킬<br/>
    /// 레벨 당 15% 증가한다.
    /// </summary>
    /// <param name="_skillLevel"></param>
    /// <returns>Use.. Default build speed *= (1 - (return value / 100f))</returns>
    public List<float> IncreaseBuildSpeed(string _skillName, int _skillLevel)
    {
        // 스킬 UID를 사용해서 Dictionary 형식으로 받아옴.
        // Dictionary<string, string> skill = GetPlayerSkillName(uid);
        // string skillName = skill["PlayerSkill_Name"];

        List<float> skillinfo = FindSkill.GetSkillInfo(_skillName);
        List<float> _return = new List<float>();
        _return.Add(skillinfo[0] * _skillLevel);

        // 보유 총알 증가량 반환
        return _return;
    }

    /// <summary>
    /// 수리 속도 증가시키는 스킬<br/>
    /// 레벨 당 10% 증가한다.
    /// </summary>
    /// <param name="_skillLevel"></param>
    /// <returns>Use.. Defualt repair speed *= (1 - (return value / 100f))</returns>
    public List<float> IncreaseRepairSpeed(string _skillName, int _skillLevel)
    {
        // 스킬 UID를 사용해서 Dictionary 형식으로 받아옴.
        // Dictionary<string, string> skill = GetPlayerSkillName(uid);
        // string skillName = skill["PlayerSkill_Name"];

        List<float> skillinfo = FindSkill.GetSkillInfo(_skillName);
        List<float> _return = new List<float>();
        _return.Add(skillinfo[0] * _skillLevel);

        // 보유 총알 증가량 반환
        return _return;
    }

    /// <summary>
    /// 공격 속도를 증가시키는 Perk<br/>
    /// 공격 속도가 15% 증가한다.
    /// </summary>
    /// <param name="_skillLevel"></param>
    /// <returns>Use.. Default weapon attack speed *= (1 + (return value / 100f))</returns>
    public List<float> IncreaseAttackSpeed_Perk(string _skillName, int _skillLevel)
    {
        // 스킬 UID를 사용해서 Dictionary 형식으로 받아옴.
        // Dictionary<string, string> skill = GetPlayerSkillName(uid);
        // string skillName = skill["PlayerSkill_Name"];

        List<float> skillinfo = FindSkill.GetSkillInfo(_skillName);
        List<float> _return = new List<float>();
        _return.Add(skillinfo[0] * _skillLevel);

        // 보유 총알 증가량 반환
        return _return;
    }

    /// <summary>
    /// 총알을 사용하지 않는 확률을 생기게 해주는 Perk<br/>
    /// 획득하면 25% 확률로 총알을 소모하지 않게된다.
    /// </summary>
    /// <param name="_skillLevel"></param>
    /// <returns>Return don't use bullet percentage</returns>
    public List<float> DontUseBullet_Perk(string _skillName, int _skillLevel)
    {
        // 스킬 UID를 사용해서 Dictionary 형식으로 받아옴.
        // Dictionary<string, string> skill = GetPlayerSkillName(uid);
        // string skillName = skill["PlayerSkill_Name"];

        List<float> skillinfo = FindSkill.GetSkillInfo(_skillName);
        List<float> _return = new List<float>();
        _return.Add(skillinfo[0] * _skillLevel);

        // 보유 총알 증가량 반환
        return _return;
    }

    /// <summary>
    /// 일정 확률로 높은 데미지의 총알을 발사하는 Perk<br/>
    /// 20% 확률로 데미지가 20 높은 총알을 발사한다.
    /// </summary>
    /// <param name="_skillLevel"></param>
    /// <returns>Skill probabilty of occurrence and increase bullet damage</returns>
    public List<float> IncreaseWeaponDamage_Perk(string _skillName, int _skillLevel)
    {
        // 스킬 UID를 사용해서 Dictionary 형식으로 받아옴.
        // Dictionary<string, string> skill = GetPlayerSkillName(uid);
        // string skillName = skill["PlayerSkill_Name"];

        List<float> skillinfo = FindSkill.GetSkillInfo(_skillName);

        float percent = 20f;
        float incDamage = skillinfo[0] * _skillLevel;

        List<float> _return = new List<float>();
        _return.Add(percent);
        _return.Add(incDamage);

        // 보유 총알 증가량 반환
        return _return;
    }

    /// <summary>
    /// 획득시 회복 아이템의 회복량이 증가하는 Perk<br/>
    /// 회복량이 15% 증가한다.
    /// </summary>
    /// <param name="_skillLevel"></param>
    /// <returns>Use.. Use healing item function(Return value)</returns>
    /// /// Use healing item function >> use item defualt value += return value
    public List<float> IncreaseHealingPoint_Perk(string _skillName, int _skillLevel)
    {
        // 스킬 UID를 사용해서 Dictionary 형식으로 받아옴.
        // Dictionary<string, string> skill = GetPlayerSkillName(uid);
        // string skillName = skill["PlayerSkill_Name"];

        List<float> skillinfo = FindSkill.GetSkillInfo(_skillName);
        List<float> _return = new List<float>();
        _return.Add(skillinfo[0] * _skillLevel);

        // 보유 총알 증가량 반환
        return _return;
    }

    /// <summary>
    /// 획득시 회복 아이템 사용 속도를 증가시켜주는 Perk<br/>
    /// 사용 속도를 30% 올려준다.
    /// </summary>
    /// <param name="_skillLevel"></param>
    /// <returns>Use.. Default healing speed += (healing speed * return value)</returns>
    public List<float> IncreaseHealingSpeed_Perk(string _skillName, int _skillLevel)
    {
        // 스킬 UID를 사용해서 Dictionary 형식으로 받아옴.
        // Dictionary<string, string> skill = GetPlayerSkillName(uid);
        // string skillName = skill["PlayerSkill_Name"];

        List<float> skillinfo = FindSkill.GetSkillInfo(_skillName);
        List<float> _return = new List<float>();
        _return.Add(skillinfo[0] * _skillLevel);

        // 보유 총알 증가량 반환
        return _return;
    }

    /// <summary>
    /// 획득시 회복 아이템을 일정 확률로 사용하지 않게 해주는 Perk<br/>
    /// 20%의 확률로 사용하지 않게 된다.
    /// </summary>
    /// <param name="_skillLevel"></param>
    /// <returns>Return don't use item percentage</returns>
    public List<float> DontUseHealingItem_Perk(string _skillName, int _skillLevel)
    {
        // 스킬 UID를 사용해서 Dictionary 형식으로 받아옴.
        // Dictionary<string, string> skill = GetPlayerSkillName(uid);
        // string skillName = skill["PlayerSkill_Name"];

        List<float> skillinfo = FindSkill.GetSkillInfo(_skillName);
        List<float> _return = new List<float>();
        _return.Add(skillinfo[0] * _skillLevel);

        // 보유 총알 증가량 반환
        return _return;
    }

    /// <summary>
    /// 획득시 건설 속도가 증가하는 Perk<br/>
    /// 15% 증가한다.
    /// </summary>
    /// <param name="_skillLevel"></param>
    /// <returns>Use.. Default build speed *= (1- (return value / 100f))</returns>
    public List<float> IncreaseBuildSpeed_Perk(string _skillName, int _skillLevel)
    {
        // 스킬 UID를 사용해서 Dictionary 형식으로 받아옴.
        // Dictionary<string, string> skill = GetPlayerSkillName(uid);
        // string skillName = skill["PlayerSkill_Name"];

        List<float> skillinfo = FindSkill.GetSkillInfo(_skillName);
        List<float> _return = new List<float>();
        _return.Add(skillinfo[0] * _skillLevel);

        // 보유 총알 증가량 반환
        return _return;
    }

    /// <summary>
    /// 획득시 건설하는 방어물자의 최대 체력이 증가하게되는 스킬<br/>
    /// 최대 체력이 70 증가하게 된다.
    /// </summary>
    /// <param name="_skillLevel"></param>
    /// <returns>Use.. Structure max hp += return value</returns>
    public List<float> IncreaseBuildingMaxHealthPoint_Perk(string _skillName, int _skillLevel)
    {
        // 스킬 UID를 사용해서 Dictionary 형식으로 받아옴.
        // Dictionary<string, string> skill = GetPlayerSkillName(uid);
        // string skillName = skill["PlayerSkill_Name"];

        List<float> skillinfo = FindSkill.GetSkillInfo(_skillName);

        List<float> _return = new List<float>();
        _return.Add(skillinfo[0] * _skillLevel);

        // 보유 총알 증가량 반환
        return _return;
    }

    /// <summary>
    /// 획득시 건설한 방어물자에 자동 회복 기능을 추가하는 스킬<br/>
    /// 시간 당 5의 체력을 회복하게 된다.
    /// </summary>
    /// <param name="_skillLevel"></param>
    /// <returns>If having this skill return 1f</returns>
    public List<float> BuildingAutoRepair_Perk(string _skillName, int _skillLevel)
    {
        List<float> _return = new List<float>();
        if (_skillLevel >= 1)
        {
            _return.Add(1f);
            return _return;
        }
        else
        {
            _return.Add(0f);
            return _return;
        }
    }

}

public class FindPlayerSkill
{
    // 사실 여기서 Skill UID받고 PlayerSkill 쪽에서 SkillName을 받아서 SkillName을 콜하는게 정석일 것 같은데
    // 문자열로 다 때려 박아 뒀네...
    public static List<float> GetPlayerSkill(string _playerSkillName, string skillUID, int _skillLevel)
    {
        //Debug.Log("____FIND PLAYER SKILL____");

        string _skillName = DBManager.Instance.GetSkillInfo(skillUID)["Skill_Name"];

        System.Type tp = typeof(PlayerSkill);
        MethodInfo playerSkillMethod = tp.GetMethod(_playerSkillName);

        PlayerSkill playerSkill = new PlayerSkill();

        //Debug.Log("____Skill Name: " + _skillName + "____");

        object playerSkillObj = playerSkillMethod.Invoke(playerSkill, new object[] { _skillName, _skillLevel });

        //Debug.Log("___Object: " + playerSkillObj + "___");

        //Debug.Log("____END FIND PLAYER SKILL____");

        return (List<float>)playerSkillObj;
    }
}