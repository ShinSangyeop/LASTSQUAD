using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerClass;
using SimpleJSON;
using System;
using UnityEngine.UI;
using System.IO;
using LitJson;

using UnityEngine.Video;


public class DBManager : MonoBehaviour
{
    private static DBManager instance = null;
    public static DBManager Instance { get { return instance; } private set { instance = value; } }


    [Header("PHP URL String")]
    private string classUrl;
    private string allClassUrl;
    private string weaponUrl;
    private string allWeaponUrl;
    private string allSkillUrl;
    private string allPlayerSkillUrl;
    private string allDefensiveStructureUrl;
    private string allItemUrl;
    private string allMonsterUrl;
    private string allMonsterSkillUrl;
    private string allStageSpawnUrl;


    [Header("File Path")]
    private string resourcePath;
    private string streamingAssetPath;
    private string jsonPath;
    private string classPath;
    private string weaponPath;
    private string skillPath;
    private string playerSkillPath;
    private string defensiveStructurePath;
    private string itemPath;
    private string monsterPath;
    private string monsterSkillPath;
    private string stageSpawnPath;



    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }


        classUrl = "127.0.0.1/Unity/Class.php";
        allClassUrl = "127.0.0.1/Unity/AllClass.php";
        weaponUrl = "127.0.0.1/Unity/Weapon.php";
        allWeaponUrl = "127.0.0.1/Unity/AllWeapon.php";
        allSkillUrl = "127.0.0.1/Unity/AllSkill.php";
        allPlayerSkillUrl = "127.0.0.1/Unity/AllPlayerSkill.php";
        allDefensiveStructureUrl = "127.0.0.1/Unity/AllDefensiveStructure.php";
        allItemUrl = "127.0.0.1/Unity/AllItem.php";
        allMonsterUrl = "127.0.0.1/Unity/AllMonster.php";
        allMonsterSkillUrl = "127.0.0.1/Unity/AllMonsterSkill.php";
        allStageSpawnUrl = "127.0.0.1/Unity/AllStageSpawn.php";


        resourcePath = "/Resources/";
        streamingAssetPath = Application.streamingAssetsPath + "/";
        jsonPath = "JSON/";
        classPath = "Class/";
        weaponPath = "Weapon/";
        skillPath = "Skill/";
        playerSkillPath = "PlayerSkill/";
        defensiveStructurePath = "DefensiveStructure/";
        itemPath = "Item/";
        monsterPath = "Monster/";
        monsterSkillPath = "MonsterSkill/";
        stageSpawnPath = "StageSpawn/";


        StartCoroutine(GetAllClassCo());
        StartCoroutine(GetAllWeaponCo());
        StartCoroutine(GetAllSkillCo());
        StartCoroutine(GetAllDefensiveStructureCo());
        StartCoroutine(GetAllItemCo());
        StartCoroutine(GetAllStageSpawnCo());
        StartCoroutine(GetAllMonsterSkillCo());
        StartCoroutine(GetAllMonsterCo());
        StartCoroutine(GetAllPlayerSkillCo());

        //Debug.Log(streamingAssetPath); // Attsets/StreamingAssets/
    }


    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(Application.dataPath); // Assets
        //Debug.Log(Application.streamingAssetsPath); // >> 런타임중 작성 불가 Assets/StreamingAssets
        //Debug.Log(Application.persistentDataPath); // >> 로컬 폴더 // C:/Users/name/AppData/LocalLow/CompanyName/ProjectName



    }


    #region 실시간으로 Json 요청해서 원하는 값 반환하는 방식 ///실패
    //public void GetClassInfo(ePlayerClass playerClass)
    //{
    //    // playerClass를 받아서 String 값으로 전달하면서 코루틴 실행.
    //    //StartCoroutine(GetClassCo(playerClass.ToString()));
    //    // 플레이어의 직업을 받아서 실시간으로 정보를 넘겨주는 방식에서
    //    // 시작할 때 데이터를 받아서 json파일로 만들어 놓고
    //    // 요청이 들어오면 json 파일을 읽어서 Dictionary로 되돌려주는 방식으로 변경.



    //}

    //IEnumerator GetClassCo(string playerClass)
    //{
    //    // POST 방식의 요청
    //    WWWForm form = new WWWForm();

    //    form.AddField("Input_className", playerClass);

    //    // classUrl(php)에 form 값을 넘겨준다.
    //    WWW webRequest = new WWW(classUrl, form);

    //    yield return webRequest;

    //    // classUrl로 넘겨준 값에 error가 반환이 되는 것이 아니면
    //    if (string.IsNullOrEmpty(webRequest.error))
    //    {
    //        // 실행
    //        yield return GetClassJson(webRequest.text);
    //    }

    //}

    //private object GetClassJson(string _jsonData)
    //{
    //    // 입력받은 데이터를 Parsing 하는 단계
    //    var parseData = JSON.Parse(_jsonData);
    //    // {"results":[]} 형태의 파일
    //    var arrayData = parseData["results"];
    //    // []와 같이 데이터만 남는다.

    //    Dictionary<string, string> classDic = new Dictionary<string, string>();

    //    // 개수가 0개보다 많을 경우
    //    if (arrayData.Count > 0)
    //    {
    //        classDic.Add("ClassName", arrayData[0]["ClassName"].Value);
    //        classDic.Add("WeaponUID", arrayData[0]["WeaponUID"].Value);
    //        classDic.Add("StatusSkill0_UID", arrayData[0]["StatusSkill0_UID"].Value);
    //        classDic.Add("StatusSkill1_UID", arrayData[0]["StatusSkill1_UID"].Value);
    //        classDic.Add("StatusSkill2_UID", arrayData[0]["StatusSkill2_UID"].Value);
    //        classDic.Add("AbilitySkill0_UID", arrayData[0]["AbilitySkill0_UID"].Value);
    //        classDic.Add("AbilitySkill1_UID", arrayData[0]["AbilitySkill1_UID"].Value);
    //        classDic.Add("Perk0_UID", arrayData[0]["Perk0_UID"].Value);
    //        classDic.Add("Perk1_UID", arrayData[0]["Perk1_UID"].Value);
    //        classDic.Add("Perk2_UID", arrayData[0]["Perk2_UID"].Value);

    //    }
    //    else
    //    {
    //        Debug.Log("없는 직업입니다.");
    //    }

    //    dataText.text = "ClassName: " + classDic["ClassName"] + "\n";
    //    dataText.text += "WeaponUID: " + classDic["WeaponUID"] + "\n";
    //    dataText.text += "StatusSkill0_UID: " + classDic["StatusSkill0_UID"] + "\n";
    //    dataText.text += "StatusSkill1_UID: " + classDic["StatusSkill1_UID"] + "\n";
    //    dataText.text += "StatusSkill2_UID: " + classDic["StatusSkill2_UID"] + "\n";
    //    dataText.text += "AbilitySkill0_UID: " + classDic["AbilitySkill0_UID"] + "\n";
    //    dataText.text += "AbilitySkill1_UID: " + classDic["AbilitySkill1_UID"] + "\n";
    //    dataText.text += "Perk0_UID: " + classDic["Perk0_UID"] + "\n";
    //    dataText.text += "Perk1_UID: " + classDic["Perk1_UID"] + "\n";
    //    dataText.text += "Perk2_UID: " + classDic["Perk2_UID"] + "\n";

    //    return 0;
    //}

    #endregion


    #region 미리 Json을 만들어 놓고 요청하면 값 반환
    #endregion
    #region Class
    /// <summary>
    /// 플레이어의 직업을 받으면 그 직업에 관련된 Dictionary 데이터를 반환
    /// </summary>
    public Dictionary<string, string> GetClassInfo(ePlayerClass playerClass)
    {
        string jsonString = null;
        try
        {
            //jsonString = File.ReadAllText(Application.dataPath + resourcePath + jsonPath + classPath + playerClass.ToString() + ".json");
            jsonString = File.ReadAllText(streamingAssetPath + jsonPath + classPath + playerClass.ToString() + ".json");
        }
        catch (Exception e)
        {
            //Debug.LogWarning(e);
            return null;
        }

        JsonData classData = JsonMapper.ToObject(jsonString);

        //Debug.Log(classData["ClassName"].ToString());

        Dictionary<string, string> _classDict = new Dictionary<string, string>();


        _classDict.Add("ClassName", classData["ClassName"].ToString());
        _classDict.Add("WeaponUID", classData["WeaponUID"].ToString());
        _classDict.Add("StatusSkill0_UID", classData["StatusSkill0_UID"].ToString());
        _classDict.Add("StatusSkill1_UID", classData["StatusSkill1_UID"].ToString());
        _classDict.Add("StatusSkill2_UID", classData["StatusSkill2_UID"].ToString());
        _classDict.Add("AbilitySkill0_UID", classData["AbilitySkill0_UID"].ToString());
        _classDict.Add("AbilitySkill1_UID", classData["AbilitySkill1_UID"].ToString());
        _classDict.Add("Perk0_UID", classData["Perk0_UID"].ToString());
        _classDict.Add("Perk1_UID", classData["Perk1_UID"].ToString());
        _classDict.Add("Perk2_UID", classData["Perk2_UID"].ToString());

        return _classDict;
    }

    /// <summary>
    /// 모든 직업 관련 데이터를 받아오는 코루틴<br/>
    /// 받아올 데이터가 없을 경우 어떤 동작도 하지 않는다.
    /// </summary>
    IEnumerator GetAllClassCo()
    {
        // POST 방식의 요청
        WWWForm form = new WWWForm();

        // classUrl(php)에 form 값을 넘겨준다.
        WWW webRequest = new WWW(allClassUrl, form);

        yield return webRequest;

        // classUrl로 넘겨준 값에 error가 반환이 되는 것이 아니면
        if (string.IsNullOrEmpty(webRequest.error))
        {
            // 실행
            GetAllClassJson(webRequest.text);
        }
    }

    /// <summary>
    /// 직업 관련 데이터를 읽어서 JSON으로 저장하는 함수
    /// </summary>
    /// <param name="_jsonData"></param>
    private void GetAllClassJson(string _jsonData)
    {
        // 입력받은 데이터를 Parsing 하는 단계
        var parseData = JSON.Parse(_jsonData);
        // {"results":[]} 형태의 파일
        var arrayData = parseData["results"];
        // []와 같이 데이터만 남는다.

        Dictionary<string, string> classDic = new Dictionary<string, string>();

        // 개수가 0개보다 많을 경우
        if (arrayData.Count > 0)
        {
            for (int i = 0; i < arrayData.Count; i++)
            {
                classDic.Add("ClassName", arrayData[i]["ClassName"].Value);
                classDic.Add("WeaponUID", arrayData[i]["WeaponUID"].Value);
                classDic.Add("StatusSkill0_UID", arrayData[i]["StatusSkill0_UID"].Value);
                classDic.Add("StatusSkill1_UID", arrayData[i]["StatusSkill1_UID"].Value);
                classDic.Add("StatusSkill2_UID", arrayData[i]["StatusSkill2_UID"].Value);
                classDic.Add("AbilitySkill0_UID", arrayData[i]["AbilitySkill0_UID"].Value);
                classDic.Add("AbilitySkill1_UID", arrayData[i]["AbilitySkill1_UID"].Value);
                classDic.Add("Perk0_UID", arrayData[i]["Perk0_UID"].Value);
                classDic.Add("Perk1_UID", arrayData[i]["Perk1_UID"].Value);
                classDic.Add("Perk2_UID", arrayData[i]["Perk2_UID"].Value);


                string fileName = arrayData[i]["ClassName"].Value;
                JsonData classJson = JsonMapper.ToJson(classDic);

                //File.WriteAllText(Application.dataPath + resourcePath + jsonPath + classPath + fileName + ".json", classJson.ToString());
                File.WriteAllText(streamingAssetPath + jsonPath + classPath + fileName + ".json", classJson.ToString());
                classDic.Clear();
            }
        }
        else
        {
            Debug.Log("없는 직업입니다.");
        }
    }
    #endregion

    #region Weapon
    /// <summary>
    /// 무기 관련 정보를 받아 딕셔너리 형식으로 반환하는 함수<br/>
    /// 무기 UID를 매개변수로 사용한다.
    /// </summary>
    /// <param name="_weaponUID"></param>
    /// <returns></returns>
    public Dictionary<string, string> GetWeaponInfo(string _weaponUID)
    {
        //string jsonString = File.ReadAllText(Application.dataPath + resourcePath + jsonPath + weaponPath + _weaponUID.ToString() + ".json");
        string jsonString = File.ReadAllText(streamingAssetPath + jsonPath + weaponPath + _weaponUID.ToString() + ".json");


        JsonData weaponData = JsonMapper.ToObject(jsonString);

        //Debug.Log(weaponData["Weapon_UID"].ToString());

        Dictionary<string, string> _weaponDict = new Dictionary<string, string>();

        _weaponDict.Add("Weapon_UID", weaponData["Weapon_UID"].ToString());
        _weaponDict.Add("Weapon_Name", weaponData["Weapon_Name"].ToString());
        _weaponDict.Add("Weapon_Damage", weaponData["Weapon_Damage"].ToString());
        _weaponDict.Add("Weapon_AttackSpeed", weaponData["Weapon_AttackSpeed"].ToString());
        _weaponDict.Add("Weapon_AttackDistance", weaponData["Weapon_AttackDistance"].ToString());
        _weaponDict.Add("Weapon_ReloadBullet", weaponData["Weapon_ReloadBullet"].ToString());
        _weaponDict.Add("Weapon_CarryBullet", weaponData["Weapon_CarryBullet"].ToString());
        _weaponDict.Add("Weapon_ReloadTime", weaponData["Weapon_ReloadTime"].ToString());
        _weaponDict.Add("Weapon_AttackRange", weaponData["Weapon_AttackRange"].ToString());

        return _weaponDict;
    }

    /// <summary>
    /// DB에서 무기 관련 데이터를 모두 받아오는 코루틴<br/>
    /// 없을 경우 변화가 없다.
    /// </summary>
    /// <returns></returns>
    IEnumerator GetAllWeaponCo()
    {
        // POST 방식의 요청
        WWWForm form = new WWWForm();

        // classUrl(php)에 form 값을 넘겨준다.
        WWW webRequest = new WWW(allWeaponUrl, form);

        yield return webRequest;

        // classUrl로 넘겨준 값에 error가 반환이 되는 것이 아니면
        if (string.IsNullOrEmpty(webRequest.error))
        {
            // 실행
            GetAllWeaponJson(webRequest.text);
        }
    }

    /// <summary>
    /// 모든 무기 정보를 JSON 파일로 저장하는 함수
    /// </summary>
    /// <param name="_jsonData"></param>
    private void GetAllWeaponJson(string _jsonData)
    {
        // 입력받은 데이터를 Parsing 하는 단계
        var parseData = JSON.Parse(_jsonData);
        // {"results":[]} 형태의 파일
        var arrayData = parseData["results"];
        // []와 같이 데이터만 남는다.

        Dictionary<string, string> weaponDict = new Dictionary<string, string>();

        // 개수가 0개보다 많을 경우
        if (arrayData.Count > 0)
        {
            for (int i = 0; i < arrayData.Count; i++)
            {
                weaponDict.Add("Weapon_UID", arrayData[i]["Weapon_UID"].Value);
                weaponDict.Add("Weapon_Name", arrayData[i]["Weapon_Name"].Value);
                weaponDict.Add("Weapon_Damage", arrayData[i]["Weapon_Damage"].Value);
                weaponDict.Add("Weapon_AttackSpeed", arrayData[i]["Weapon_AttackSpeed"].Value);
                weaponDict.Add("Weapon_AttackDistance", arrayData[i]["Weapon_AttackDistance"].Value);
                weaponDict.Add("Weapon_ReloadBullet", arrayData[i]["Weapon_ReloadBullet"].Value);
                weaponDict.Add("Weapon_CarryBullet", arrayData[i]["Weapon_CarryBullet"].Value);
                weaponDict.Add("Weapon_ReloadTime", arrayData[i]["Weapon_ReloadTime"].Value);
                weaponDict.Add("Weapon_AttackRange", arrayData[i]["Weapon_AttackRange"].Value);

                string fileName = arrayData[i]["Weapon_UID"].Value;
                JsonData classJson = JsonMapper.ToJson(weaponDict);

                //File.WriteAllText(Application.dataPath + resourcePath + jsonPath + weaponPath + fileName + ".json", classJson.ToString());
                File.WriteAllText(streamingAssetPath + jsonPath + weaponPath + fileName + ".json", classJson.ToString());

                weaponDict.Clear();
            }
        }
        else
        {
            Debug.Log("무기 데이터가 없습니다.");
        }
    }

    #endregion

    #region Skill

    /// <summary>
    /// 스킬 정보를 받는게 이게 아닌 것 같은데
    /// 스킬 정보를 받으려면 이게 아니라 스킬 효과를 받아와야 할 것 같은데
    /// </summary>
    /// <param name="skillUID">스킬 UID</param>
    /// <returns>Skill Dict</returns>
    public Dictionary<string, string> GetSkillInfo(string skillUID)
    {
        string jsonString = null;
        try
        {
            //jsonString = File.ReadAllText(Application.dataPath + resourcePath + jsonPath + classPath + playerClass.ToString() + ".json");
            jsonString = File.ReadAllText(streamingAssetPath + jsonPath + skillPath + skillUID.ToString() + ".json");
        }
        catch (Exception e)
        {
#if UNITY_EDITOR
            Debug.LogWarning(e);

#endif
            return null;
        }

        JsonData skillData = JsonMapper.ToObject(jsonString);

        //Debug.Log(classData["ClassName"].ToString());

        Dictionary<string, string> _skillDict = new Dictionary<string, string>();

        _skillDict.Add("Skill_UID", skillData["Skill_UID"].ToString());
        _skillDict.Add("Skill_Name", skillData["Skill_Name"].ToString());

        return _skillDict;
    }

    /// <summary>
    /// DB에서 모든 Skill 정보를 받아오는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator GetAllSkillCo()
    {
        // POST 방식의 요청
        WWWForm form = new WWWForm();

        // classUrl(php)에 form 값을 넘겨준다.
        WWW webRequest = new WWW(allSkillUrl, form);

        yield return webRequest;

        // skillUrl로 넘겨준 값에 error가 반환이 되는 것이 아니면
        if (string.IsNullOrEmpty(webRequest.error))
        {
            // 실행
            GetAllSkillJson(webRequest.text);
        }
    }

    /// <summary>
    /// 스킬 정보를 Json파일로 만드는 함수
    /// </summary>
    /// <param name="_jsonData">Json Data</param>
    private void GetAllSkillJson(string _jsonData)
    {

        // 입력받은 데이터를 Parsing 하는 단계
        var parseData = JSON.Parse(_jsonData);
        // {"results":[]} 형태의 파일
        var arrayData = parseData["results"];
        // []와 같이 데이터만 남는다.

        Dictionary<string, string> skillDict = new Dictionary<string, string>();

        // 개수가 0개보다 많을 경우
        if (arrayData.Count > 0)
        {
            for (int i = 0; i < arrayData.Count; i++)
            {
                skillDict.Add("Skill_UID", arrayData[i]["Skill_UID"].Value);
                skillDict.Add("Skill_Name", arrayData[i]["Skill_Name"].Value);


                string fileName = arrayData[i]["Skill_UID"].Value;
                JsonData classJson = JsonMapper.ToJson(skillDict);

                //File.WriteAllText(Application.dataPath + resourcePath + jsonPath + weaponPath + fileName + ".json", classJson.ToString());
                File.WriteAllText(streamingAssetPath + jsonPath + skillPath + fileName + ".json", classJson.ToString());

                skillDict.Clear();
            }
        }
        else
        {
            Debug.Log("스킬 데이터가 없습니다.");
        }


    }

    #endregion

    #region PlayerSkill

    /// <summary>
    /// 플레이어의 직업을 받으면 그 직업에 관련된 Dictionary 데이터를 반환
    /// </summary>
    public Dictionary<string, string> GetPlayerSkill(string _skillUID)
    {
        string jsonString = null;
        try
        {
            //jsonString = File.ReadAllText(Application.dataPath + resourcePath + jsonPath + classPath + playerClass.ToString() + ".json");
            jsonString = File.ReadAllText(streamingAssetPath + jsonPath + playerSkillPath + _skillUID.ToString() + ".json");
        }
        catch (Exception e)
        {
            //Debug.LogWarning(e);
            return null;
        }

        JsonData playerSkillData = JsonMapper.ToObject(jsonString);

        //Debug.Log(classData["ClassName"].ToString());

        Dictionary<string, string> _playerSkillDict = new Dictionary<string, string>();


        _playerSkillDict.Add("PlayerSkill_UID", playerSkillData["PlayerSkill_UID"].ToString());
        _playerSkillDict.Add("PlayerSkill_Name", playerSkillData["PlayerSkill_Name"].ToString());
        _playerSkillDict.Add("PlayerSkill_SkillUID", playerSkillData["PlayerSkill_SkillUID"].ToString());
        _playerSkillDict.Add("PlayerSkill_Coefficient", playerSkillData["PlayerSkill_Coefficient"].ToString());

        return _playerSkillDict;
    }

    /// <summary>
    /// DB에서 Player Skill 정보를 받아오는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator GetAllPlayerSkillCo()
    {
        // POST 방식의 요청
        WWWForm form = new WWWForm();

        // classUrl(php)에 form 값을 넘겨준다.
        WWW webRequest = new WWW(allPlayerSkillUrl, form);

        yield return webRequest;

        // skillUrl로 넘겨준 값에 error가 반환이 되는 것이 아니면
        if (string.IsNullOrEmpty(webRequest.error))
        {
            // 실행
            GetAllPlayerSkillJson(webRequest.text);
        }
    }


    /// <summary>
    /// DB에서 받은 모든 데이터를 Json 파일로 저장하는 함수
    /// </summary>
    /// <param name="_jsonData">Player Skill Data가 저장된 json 형식의 string text</param>
    private void GetAllPlayerSkillJson(string _jsonData)
    {

        // 입력받은 데이터를 Parsing 하는 단계
        var parseData = JSON.Parse(_jsonData);
        // {"results":[]} 형태의 파일
        var arrayData = parseData["results"];
        // []와 같이 데이터만 남는다.

        Dictionary<string, string> playerSkillDict = new Dictionary<string, string>();

        // 개수가 0개보다 많을 경우
        if (arrayData.Count > 0)
        {
            for (int i = 0; i < arrayData.Count; i++)
            {
                playerSkillDict.Add("PlayerSkill_UID", arrayData[i]["PlayerSkill_UID"].Value);
                playerSkillDict.Add("PlayerSkill_Name", arrayData[i]["PlayerSkill_Name"].Value);
                playerSkillDict.Add("PlayerSkill_SkillUID", arrayData[i]["PlayerSkill_SkillUID"].Value);
                playerSkillDict.Add("PlayerSkill_Coefficient", arrayData[i]["PlayerSkill_Coefficient"].Value);

                string fileName = arrayData[i]["PlayerSkill_UID"].Value;
                JsonData classJson = JsonMapper.ToJson(playerSkillDict);

                //File.WriteAllText(Application.dataPath + resourcePath + jsonPath + weaponPath + fileName + ".json", classJson.ToString());
                File.WriteAllText(streamingAssetPath + jsonPath + playerSkillPath + fileName + ".json", classJson.ToString());

                playerSkillDict.Clear();
            }
        }
        else
        {
            Debug.Log("플레이어 스킬이 없습니다.");
        }
    }


    #endregion

    #region DefensiveStructure
    public Dictionary<string, string> GetDefInfo(string _defUID)
    {
        //string jsonString = File.ReadAllText(Application.dataPath + resourcePath + jsonPath + weaponPath + _weaponUID.ToString() + ".json");
        string jsonString = File.ReadAllText(streamingAssetPath + jsonPath + defensiveStructurePath + _defUID.ToString() + ".json");


        JsonData defData = JsonMapper.ToObject(jsonString);

        //Debug.Log(weaponData["Weapon_UID"].ToString());

        Dictionary<string, string> _defDict = new Dictionary<string, string>();

        _defDict.Add("Defensive_UID", defData["Defensive_UID"].ToString());
        _defDict.Add("Defensive_Name", defData["Defensive_Name"].ToString());
        _defDict.Add("Defensive_Hp", defData["Defensive_Hp"].ToString());
        _defDict.Add("Defensive_Damage", defData["Defensive_Damage"].ToString());
        _defDict.Add("Defensive_Passable", defData["Defensive_Passable"].ToString());

        return _defDict;
    }


    /// <summary>
    /// DB에서 모든 방어 물자 정보를 가져오는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator GetAllDefensiveStructureCo()
    {
        // POST 방식의 요청
        WWWForm form = new WWWForm();
        // classUrl(php)에 form 값을 넘겨준다.
        WWW webRequest = new WWW(allDefensiveStructureUrl, form);

        yield return webRequest;

        // skillUrl로 넘겨준 값에 error가 반환이 되는 것이 아니면
        if (string.IsNullOrEmpty(webRequest.error))
        {
            // 실행
            GetAllDefensiveStructureJson(webRequest.text);
        }
    }

    /// <summary>
    /// 받아온 정보를 Json 파일로 저장하는 함수
    /// </summary>
    /// <param name="_jsonData"></param>
    private void GetAllDefensiveStructureJson(string _jsonData)
    {

        // 입력받은 데이터를 Parsing 하는 단계
        var parseData = JSON.Parse(_jsonData);
        // {"results":[]} 형태의 파일
        var arrayData = parseData["results"];
        // []와 같이 데이터만 남는다.

        Dictionary<string, string> structDict = new Dictionary<string, string>();

        // 개수가 0개보다 많을 경우
        if (arrayData.Count > 0)
        {
            for (int i = 0; i < arrayData.Count; i++)
            {
                structDict.Add("Defensive_UID", arrayData[i]["Defensive_UID"].Value);
                structDict.Add("Defensive_Name", arrayData[i]["Defensive_Name"].Value);
                structDict.Add("Defensive_Hp", arrayData[i]["Defensive_Hp"].Value);
                structDict.Add("Defensive_Damage", arrayData[i]["Defensive_Damage"].Value);
                structDict.Add("Defensive_Passable", arrayData[i]["Defensive_Passable"].Value);

                string fileName = arrayData[i]["Defensive_UID"].Value;
                JsonData classJson = JsonMapper.ToJson(structDict);

                //File.WriteAllText(Application.dataPath + resourcePath + jsonPath + weaponPath + fileName + ".json", classJson.ToString());
                File.WriteAllText(streamingAssetPath + jsonPath + defensiveStructurePath + fileName + ".json", classJson.ToString());

                structDict.Clear();
            }
        }
        else
        {
            Debug.Log("방어물자 정보가 없습니다.");
        }
    }


    #endregion

    #region Item


    /// <summary>
    /// DB에서 모든 방어 물자 정보를 가져오는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator GetAllItemCo()
    {
        // POST 방식의 요청
        WWWForm form = new WWWForm();
        // classUrl(php)에 form 값을 넘겨준다.
        WWW webRequest = new WWW(allItemUrl, form);

        yield return webRequest;

        // skillUrl로 넘겨준 값에 error가 반환이 되는 것이 아니면
        if (string.IsNullOrEmpty(webRequest.error))
        {
            // 실행
            GetAllItemJson(webRequest.text);
        }
    }

    /// <summary>
    /// 받아온 정보를 Json 파일로 저장하는 함수
    /// </summary>
    /// <param name="_jsonData"></param>
    private void GetAllItemJson(string _jsonData)
    {

        // 입력받은 데이터를 Parsing 하는 단계
        var parseData = JSON.Parse(_jsonData);
        // {"results":[]} 형태의 파일
        var arrayData = parseData["results"];
        // []와 같이 데이터만 남는다.

        Dictionary<string, string> itemDict = new Dictionary<string, string>();

        // 개수가 0개보다 많을 경우
        if (arrayData.Count > 0)
        {
            for (int i = 0; i < arrayData.Count; i++)
            {
                itemDict.Add("Item_UID", arrayData[i]["Item_UID"].Value);
                itemDict.Add("Item_Name", arrayData[i]["Item_Name"].Value);
                itemDict.Add("Item_SkillUID", arrayData[i]["Item_SkillUID"].Value);
                itemDict.Add("Item_Coefficient", arrayData[i]["Item_Coefficient"].Value);

                string fileName = arrayData[i]["Item_UID"].Value;
                JsonData classJson = JsonMapper.ToJson(itemDict);

                //File.WriteAllText(Application.dataPath + resourcePath + jsonPath + weaponPath + fileName + ".json", classJson.ToString());
                File.WriteAllText(streamingAssetPath + jsonPath + itemPath + fileName + ".json", classJson.ToString());

                itemDict.Clear();
            }
        }
        else
        {
            Debug.Log("아이템 정보가 없습니다.");
        }
    }

    #endregion

    #region Monster


    /// <summary>
    /// 플레이어의 직업을 받으면 그 직업에 관련된 Dictionary 데이터를 반환
    /// </summary>
    public Dictionary<string, string> GetMonsterInfo(string _monsterUID)
    {
        string jsonString = null;
        try
        {
            //jsonString = File.ReadAllText(Application.dataPath + resourcePath + jsonPath + classPath + playerClass.ToString() + ".json");
            jsonString = File.ReadAllText(streamingAssetPath + jsonPath + monsterPath + _monsterUID.ToString() + ".json");
        }
        catch (Exception e)
        {
            //Debug.LogWarning(e);
            return null;
        }

        JsonData monsterInfoData = JsonMapper.ToObject(jsonString);

        //Debug.Log(classData["ClassName"].ToString());

        Dictionary<string, string> _monsterInfoDict = new Dictionary<string, string>();


        _monsterInfoDict.Add("Monster_UID", monsterInfoData["Monster_UID"].ToString());
        _monsterInfoDict.Add("Monster_Name", monsterInfoData["Monster_Name"].ToString());
        _monsterInfoDict.Add("Monster_Hp", monsterInfoData["Monster_Hp"].ToString());
        _monsterInfoDict.Add("Monster_Armour", monsterInfoData["Monster_Armour"].ToString());
        _monsterInfoDict.Add("Monster_MoveSpeed", monsterInfoData["Monster_MoveSpeed"].ToString());
        _monsterInfoDict.Add("Monster_Damage", monsterInfoData["Monster_Damage"].ToString());
        _monsterInfoDict.Add("Monster_AttackDistance", monsterInfoData["Monster_AttackDistance"].ToString());
        _monsterInfoDict.Add("Monster_AttackSpeed", monsterInfoData["Monster_AttackSpeed"].ToString());
        _monsterInfoDict.Add("Monster_Exp", monsterInfoData["Monster_Exp"].ToString());
        _monsterInfoDict.Add("Monster_SkillUID", monsterInfoData["Monster_SkillUID"].ToString());

        return _monsterInfoDict;
    }

    /// <summary>
    /// DB에서 모든 몬스터 정보를 가져오는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator GetAllMonsterCo()
    {
        // POST 방식의 요청
        WWWForm form = new WWWForm();
        // classUrl(php)에 form 값을 넘겨준다.
        WWW webRequest = new WWW(allMonsterUrl, form);

        yield return webRequest;

        // skillUrl로 넘겨준 값에 error가 반환이 되는 것이 아니면
        if (string.IsNullOrEmpty(webRequest.error))
        {
            // 실행
            GetAllMonsterJson(webRequest.text);
        }
    }

    /// <summary>
    /// 받아온 정보를 Json 파일로 저장하는 함수
    /// </summary>
    /// <param name="_jsonData"></param>
    private void GetAllMonsterJson(string _jsonData)
    {
        // 입력받은 데이터를 Parsing 하는 단계
        var parseData = JSON.Parse(_jsonData);
        // {"results":[]} 형태의 파일
        var arrayData = parseData["results"];
        // []와 같이 데이터만 남는다.

        Dictionary<string, string> monsterDict = new Dictionary<string, string>();

        // 개수가 0개보다 많을 경우
        if (arrayData.Count > 0)
        {
            for (int i = 0; i < arrayData.Count; i++)
            {
                monsterDict.Add("Monster_UID", arrayData[i]["Monster_UID"].Value);
                monsterDict.Add("Monster_Name", arrayData[i]["Monster_Name"].Value);
                monsterDict.Add("Monster_Hp", arrayData[i]["Monster_Hp"].Value);
                monsterDict.Add("Monster_Armour", arrayData[i]["Monster_Armour"].Value);
                monsterDict.Add("Monster_MoveSpeed", arrayData[i]["Monster_MoveSpeed"].Value);
                monsterDict.Add("Monster_Damage", arrayData[i]["Monster_Damage"].Value);
                monsterDict.Add("Monster_AttackDistance", arrayData[i]["Monster_AttackDistance"].Value);
                monsterDict.Add("Monster_AttackSpeed", arrayData[i]["Monster_AttackSpeed"].Value);
                monsterDict.Add("Monster_Exp", arrayData[i]["Monster_Exp"].Value);
                monsterDict.Add("Monster_SkillUID", arrayData[i]["Monster_SkillUID"].Value);

                string fileName = arrayData[i]["Monster_UID"].Value;
                JsonData classJson = JsonMapper.ToJson(monsterDict);

                //File.WriteAllText(Application.dataPath + resourcePath + jsonPath + weaponPath + fileName + ".json", classJson.ToString());
                File.WriteAllText(streamingAssetPath + jsonPath + monsterPath + fileName + ".json", classJson.ToString());

                monsterDict.Clear();
            }
        }
        else
        {
            Debug.Log("몬스터 정보가 없습니다.");
        }
    }


    #endregion

    #region MonsterSkill


    /// <summary>
    /// DB에서 모든 몬스터 스킬 정보를 가져오는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator GetAllMonsterSkillCo()
    {
        // POST 방식의 요청
        WWWForm form = new WWWForm();
        // classUrl(php)에 form 값을 넘겨준다.
        WWW webRequest = new WWW(allMonsterSkillUrl, form);

        yield return webRequest;

        // skillUrl로 넘겨준 값에 error가 반환이 되는 것이 아니면
        if (string.IsNullOrEmpty(webRequest.error))
        {
            // 실행
            GetAllMonsterSkillJson(webRequest.text);
        }
    }

    /// <summary>
    /// 받아온 정보를 Json 파일로 저장하는 함수
    /// </summary>
    /// <param name="_jsonData"></param>
    private void GetAllMonsterSkillJson(string _jsonData)
    {
        // 입력받은 데이터를 Parsing 하는 단계
        var parseData = JSON.Parse(_jsonData);
        // {"results":[]} 형태의 파일
        var arrayData = parseData["results"];
        // []와 같이 데이터만 남는다.

        Dictionary<string, string> monsterSkillDict = new Dictionary<string, string>();

        // 개수가 0개보다 많을 경우
        if (arrayData.Count > 0)
        {
            for (int i = 0; i < arrayData.Count; i++)
            {
                monsterSkillDict.Add("MonsterSkill_UID", arrayData[i]["MonsterSkill_UID"].Value);
                monsterSkillDict.Add("MonsterSkill_Name", arrayData[i]["MonsterSkill_Name"].Value);
                monsterSkillDict.Add("MonsterSkill_SkillUID", arrayData[i]["MonsterSkill_SkillUID"].Value);
                monsterSkillDict.Add("MonsterSkill_Coefficient", arrayData[i]["MonsterSkill_Coefficient"].Value);

                string fileName = arrayData[i]["MonsterSkill_UID"].Value;
                JsonData classJson = JsonMapper.ToJson(monsterSkillDict);

                //File.WriteAllText(Application.dataPath + resourcePath + jsonPath + weaponPath + fileName + ".json", classJson.ToString());
                File.WriteAllText(streamingAssetPath + jsonPath + monsterSkillPath + fileName + ".json", classJson.ToString());

                monsterSkillDict.Clear();
            }
        }
        else
        {
            Debug.Log("몬스터 스킬 데이터가 없습니다.");
        }
    }


    #endregion

    #region StageSpawn

    /// <summary>
    /// DB에서 모든 몬스터 스킬 정보를 가져오는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator GetAllStageSpawnCo()
    {
        // POST 방식의 요청
        WWWForm form = new WWWForm();
        // classUrl(php)에 form 값을 넘겨준다.
        WWW webRequest = new WWW(allStageSpawnUrl, form);

        yield return webRequest;

        // skillUrl로 넘겨준 값에 error가 반환이 되는 것이 아니면
        if (string.IsNullOrEmpty(webRequest.error))
        {
            // 실행
            GetAllStageSpawnJson(webRequest.text);
        }
    }

    /// <summary>
    /// 받아온 정보를 Json 파일로 저장하는 함수
    /// </summary>
    /// <param name="_jsonData"></param>
    private void GetAllStageSpawnJson(string _jsonData)
    {
        // 입력받은 데이터를 Parsing 하는 단계
        var parseData = JSON.Parse(_jsonData);
        // {"results":[]} 형태의 파일
        var arrayData = parseData["results"];
        // []와 같이 데이터만 남는다.

        Dictionary<string, string> spawnDict = new Dictionary<string, string>();

        // 개수가 0개보다 많을 경우
        if (arrayData.Count > 0)
        {
            for (int i = 0; i < arrayData.Count; i++)
            {
                spawnDict.Add("Stage_Count", arrayData[i]["Stage_Count"].Value);
                spawnDict.Add("Stage_MaxSpawn", arrayData[i]["Stage_MaxSpawn"].Value);

                string fileName = arrayData[i]["Stage_Count"].Value;
                JsonData classJson = JsonMapper.ToJson(spawnDict);

                //File.WriteAllText(Application.dataPath + resourcePath + jsonPath + weaponPath + fileName + ".json", classJson.ToString());
                File.WriteAllText(streamingAssetPath + jsonPath + stageSpawnPath + fileName + ".json", classJson.ToString());

                spawnDict.Clear();
            }
        }
        else
        {
            Debug.Log("스테이지 정보가 없습니다.");
        }
    }

    #endregion



    // 통합해서 php에서 데이터를 읽어오고 파일을 저장하려고 했는데 파일 이름을 정하는데 오류가 있어 그냥 포기함.
    // 다른 방식이 있을 수 있으나 지금은 하나하나 작성하는 것으로 한다.



}
