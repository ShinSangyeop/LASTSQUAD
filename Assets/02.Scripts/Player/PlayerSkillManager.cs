using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillManager : MonoBehaviour
{
    private PlayerCtrl playerCtrl;

    #region 플레이어 스킬 관련 변수
    public PlayerUI playerUI;

    // internal은 외부 프로젝트에서 접근할 수 없는
    // 내부 프로젝트에서만 public으로 사용되는 제한자라고 한다.
    internal int status0_Level = 0;
    internal int status1_Level = 0;
    internal int status2_Level = 0;
    internal int ability0_Level = 0;
    internal int ability1_Level = 0;
    internal int perk0_Level = 0;
    internal int perk1_Level = 0;
    internal int perk2_Level = 0;

    // 스킬 세팅이 되어있는지 확인하는 변수
    public bool skillSettingComplete = false;


    #endregion


    // Start is called before the first frame update
    void Start()
    {
        playerCtrl = GetComponent<PlayerCtrl>();
        playerUI = GetComponent<PlayerUI>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 설정된 스킬 3개를 가져와서 UID를 통해서 스킬 정보를 가져오고 UI에 표시하는 함수.
    /// 그런데 UID를 가져와도 정보를 다 가져오는게 아니라서 이 함수 내에서 String 값으로 텍스트에 직접 넣어줘야한다.
    /// 일반적인 방식은 아마 가졍는 데이터에 스킬의 설명도 같이 있어서 그 파일을 수정하면 되는 방식일텐데
    /// 가라로 만들다보니 많이 이상해졌다.
    /// </summary>
    /// <returns></returns>
    internal IEnumerator SelectSkillSetting()
    {
        CursorState.CursorLockedSetting(false);

        string imagePath = "Player/Images/Skills/";
        string imageName = "_img";

        playerUI.skillSelectObj.SetActive(true);

        for (int i = 0; i < 3; i++)
        {
            // 설정된 스킬의 정보를 가져온다.
            Dictionary<string, string> skillInfo = DBManager.Instance.GetPlayerSkill(playerCtrl._select_SkillList[i]);
            playerUI.skillInfo_ImageList[i].sprite = Resources.Load<Sprite>($"{imagePath}{skillInfo["PlayerSkill_Name"]}{imageName}");

            if (playerCtrl._select_SkillList[i] == "030000000")
            {


                playerUI.skillInfo_TextList[i].text = $"<b><size=50>{skillInfo["PlayerSkill_Name"].ToString()}</size></b>\n" +
                    $"공격력이 <color=red>{(FindPlayerSkill.GetPlayerSkill(skillInfo["PlayerSkill_Name"], skillInfo["PlayerSkill_SkillUID"], 1)[0] * float.Parse(skillInfo["PlayerSkill_Coefficient"])).ToString()}</color>" +
                    $" 증가합니다.";
            }
            else if (playerCtrl._select_SkillList[i] == "030000001")
            {


                //playerUI.skillInfo_ImageList
                playerUI.skillInfo_TextList[i].text = $"<b><size=50>{skillInfo["PlayerSkill_Name"].ToString()}</size></b>\n" +
                    $"방어력이 <color=red>{(FindPlayerSkill.GetPlayerSkill(skillInfo["PlayerSkill_Name"], skillInfo["PlayerSkill_SkillUID"], 1)[0] * float.Parse(skillInfo["PlayerSkill_Coefficient"])).ToString()}</color>" +
                    $" 증가합니다.";
            }
            else if (playerCtrl._select_SkillList[i] == "030000002")
            {


                //playerUI.skillInfo_ImageList
                playerUI.skillInfo_TextList[i].text = $"<b><size=50>{skillInfo["PlayerSkill_Name"].ToString()}</size></b>\n" +
                    $"최대 체력이 <color=red>{(FindPlayerSkill.GetPlayerSkill(skillInfo["PlayerSkill_Name"], skillInfo["PlayerSkill_SkillUID"], 1)[0] * float.Parse(skillInfo["PlayerSkill_Coefficient"])).ToString()}</color>" +
                    $" 증가합니다.";
            }
            else if (playerCtrl._select_SkillList[i] == "030010000")
            {


                //playerUI.skillInfo_ImageList
                playerUI.skillInfo_TextList[i].text = $"<b><size=50>{skillInfo["PlayerSkill_Name"].ToString()}</size></b>\n" +
                    $"보유 최대 총알이 <color=red>{(FindPlayerSkill.GetPlayerSkill(skillInfo["PlayerSkill_Name"], skillInfo["PlayerSkill_SkillUID"], 1)[0] * float.Parse(skillInfo["PlayerSkill_Coefficient"])).ToString()}</color>" +
                    $"% 증가합니다.";
            }
            else if (playerCtrl._select_SkillList[i] == "030010001")
            {


                //playerUI.skillInfo_ImageList
                playerUI.skillInfo_TextList[i].text = $"<b><size=50>{skillInfo["PlayerSkill_Name"].ToString()}</size></b>\n" +
                    $"공격 속도가 <color=red>{(FindPlayerSkill.GetPlayerSkill(skillInfo["PlayerSkill_Name"], skillInfo["PlayerSkill_SkillUID"], 1)[0] * float.Parse(skillInfo["PlayerSkill_Coefficient"])).ToString()}</color>" +
                    $"% 증가합니다.";
            }
            else if (playerCtrl._select_SkillList[i] == "030010002")
            {


                //playerUI.skillInfo_ImageList
                playerUI.skillInfo_TextList[i].text = $"<b><size=50>{skillInfo["PlayerSkill_Name"].ToString()}</size></b>\n" +
                    $"아이템 회복량이 <color=red>{(FindPlayerSkill.GetPlayerSkill(skillInfo["PlayerSkill_Name"], skillInfo["PlayerSkill_SkillUID"], 1)[0] * float.Parse(skillInfo["PlayerSkill_Coefficient"])).ToString()}</color>" +
                    $" 증가합니다.";
            }
            else if (playerCtrl._select_SkillList[i] == "030010003")
            {


                //playerUI.skillInfo_ImageList
                playerUI.skillInfo_TextList[i].text = $"<b><size=50>{skillInfo["PlayerSkill_Name"].ToString()}</size></b>\n" +
                    $"아이템 사용 속도가 <color=red>{(FindPlayerSkill.GetPlayerSkill(skillInfo["PlayerSkill_Name"], skillInfo["PlayerSkill_SkillUID"], 1)[0] * float.Parse(skillInfo["PlayerSkill_Coefficient"])).ToString()}</color>" +
                    $"% 증가합니다.";
            }
            else if (playerCtrl._select_SkillList[i] == "030010004")
            {


                //playerUI.skillInfo_ImageList
                playerUI.skillInfo_TextList[i].text = $"<b><size=50>{skillInfo["PlayerSkill_Name"].ToString()}</size></b>\n" +
                    $"건설 속도가 <color=red>{(FindPlayerSkill.GetPlayerSkill(skillInfo["PlayerSkill_Name"], skillInfo["PlayerSkill_SkillUID"], 1)[0] * float.Parse(skillInfo["PlayerSkill_Coefficient"])).ToString()}</color>" +
                    $"% 증가합니다.";
            }
            else if (playerCtrl._select_SkillList[i] == "030010005")
            {


                //playerUI.skillInfo_ImageList
                playerUI.skillInfo_TextList[i].text = $"<b><size=50>{skillInfo["PlayerSkill_Name"]}</size></b>\n" +
                    $"수리 속도가 <color=red>{(FindPlayerSkill.GetPlayerSkill(skillInfo["PlayerSkill_Name"], skillInfo["PlayerSkill_SkillUID"], 1)[0] * float.Parse(skillInfo["PlayerSkill_Coefficient"])).ToString()}</color>" +
                    $"% 증가합니다.";
            }
            // 레벨업으로 인한 스킬 선택에 Perk은 표시되지 않는다.
            /*
            else if (playerCtrl._select_SkillList[i] == "030020000")
            {


                //playerUI.skillInfo_ImageList
                playerUI.skillInfo_TextList[i].text = $"<b><size=50>{skillInfo["PlayerSkill_Name"].ToString()}</size></b>\n" +
                    $"공격 속도가 <color=red>{(FindPlayerSkill.GetPlayerSkill(skillInfo["PlayerSkill_Name"], skillInfo["PlayerSkill_SkillUID"], 1)[0] * float.Parse(skillInfo["PlayerSkill_Coefficient"])).ToString()}</color>" +
                    $"% 증가합니다.";
            }
            else if (playerCtrl._select_SkillList[i] == "030020001")
            {


                //playerUI.skillInfo_ImageList
                playerUI.skillInfo_TextList[i].text = $"<b><size=50>{skillInfo["PlayerSkill_Name"]}</size></b>\n" +
                    $"<color=red>{(FindPlayerSkill.GetPlayerSkill(skillInfo["PlayerSkill_Name"], skillInfo["PlayerSkill_SkillUID"], 1)[0] * float.Parse(skillInfo["PlayerSkill_Coefficient"])).ToString()}</color>" +
                    $"% 확률로\n총알을 사용하지 않게 됩니다.";
            }
            else if (playerCtrl._select_SkillList[i] == "030020002")
            {


                //playerUI.skillInfo_ImageList
                playerUI.skillInfo_TextList[i].text = $"<b><size=50>{skillInfo["PlayerSkill_Name"].ToString()}</size></b>\n" +
                    $"<color=red>{(FindPlayerSkill.GetPlayerSkill(skillInfo["PlayerSkill_Name"], skillInfo["PlayerSkill_SkillUID"], 1)[0] * float.Parse(skillInfo["PlayerSkill_Coefficient"])).ToString()}</color>" +
                    $"% 확률로\n 대폭 증가한 데미지를 입힙니다.";
            }
            else if (playerCtrl._select_SkillList[i] == "030020003")
            {


                //playerUI.skillInfo_ImageList
                playerUI.skillInfo_TextList[i].text = $"<b><size=50>{skillInfo["PlayerSkill_Name"].ToString()}</size></b>\n" +
                    $"아아템 회복량이 <color=red>{(FindPlayerSkill.GetPlayerSkill(skillInfo["PlayerSkill_Name"], skillInfo["PlayerSkill_SkillUID"], 1)[0] * float.Parse(skillInfo["PlayerSkill_Coefficient"])).ToString()}</color>" +
                    $" 증가합니다.";
            }
            else if (playerCtrl._select_SkillList[i] == "030020004")
            {


                //playerUI.skillInfo_ImageList
                playerUI.skillInfo_TextList[i].text = $"<b><size=50>{skillInfo["PlayerSkill_Name"].ToString()}</size></b>\n" +
                    $"아이템 사용 속도가 <color=red>{(FindPlayerSkill.GetPlayerSkill(skillInfo["PlayerSkill_Name"], skillInfo["PlayerSkill_SkillUID"], perk1_Level + 1)[0] * float.Parse(skillInfo["PlayerSkill_Coefficient"])).ToString()}</color>" +
                    $"% 증가합니다.";
            }
            else if (playerCtrl._select_SkillList[i] == "030020005")
            {


                //playerUI.skillInfo_ImageList
                playerUI.skillInfo_TextList[i].text = $"<b><size=50>{skillInfo["PlayerSkill_Name"].ToString()}</size></b>\n" +
                    $"<color=red>{(FindPlayerSkill.GetPlayerSkill(skillInfo["PlayerSkill_Name"], skillInfo["PlayerSkill_SkillUID"], perk2_Level + 1)[0] * float.Parse(skillInfo["PlayerSkill_Coefficient"])).ToString()}</color>" +
                    $"% 확률로\n아이템을 사용하지 않게 됩니다.";
            }
            else if (playerCtrl._select_SkillList[i] == "030020006")
            {


                //playerUI.skillInfo_ImageList
                playerUI.skillInfo_TextList[i].text = $"<b><size=50>{skillInfo["PlayerSkill_Name"].ToString()}</size></b>\n" +
                    $"건설 속도가 <color=red>{(FindPlayerSkill.GetPlayerSkill(skillInfo["PlayerSkill_Name"], skillInfo["PlayerSkill_SkillUID"], perk0_Level + 1)[0] * float.Parse(skillInfo["PlayerSkill_Coefficient"])).ToString()}</color>" +
                    $"% 증가합니다.";
            }
            else if (playerCtrl._select_SkillList[i] == "030020007")
            {


                //playerUI.skillInfo_ImageList
                playerUI.skillInfo_TextList[i].text = $"<b><size=50>{skillInfo["PlayerSkill_Name"].ToString()}</size></b>\n" +
                    $"건설한 건물의 최대 체력이 <color=red>{(FindPlayerSkill.GetPlayerSkill(skillInfo["PlayerSkill_Name"], skillInfo["PlayerSkill_SkillUID"], perk1_Level + 1)[0] * float.Parse(skillInfo["PlayerSkill_Coefficient"])).ToString()}</color>" +
                    $" 증가합니다.";
            }
            else if (playerCtrl._select_SkillList[i] == "030020008")
            {


                //playerUI.skillInfo_ImageList
                playerUI.skillInfo_TextList[i].text = $"<b><size=50>{skillInfo["PlayerSkill_Name"].ToString()}</size></b>\n" +
                    $"건설한 건물이 <color=red>{(FindPlayerSkill.GetPlayerSkill(skillInfo["PlayerSkill_Name"], skillInfo["PlayerSkill_SkillUID"], perk2_Level + 1)[0] * float.Parse(skillInfo["PlayerSkill_Coefficient"])).ToString()}</color>" +
                    $"의 자동회복을 가집니다.";
            }
            */

            yield return null;
        }

    }







}
