using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hp_Bar : MonoBehaviour
{
    public float hp;
    public float maxhp;


    public Slider hp_fillamount;//hp 슬라이더 바
    public Text hp_percent;//hp바 남은 퍼센테이지 텍스트
    public Text hp_function;//hp바 알림 텍스트
    //Transform cam;


    bool IsRepair;

    void Start()
    {
        //hp_percent = GetComponent<Text>();
        //hp_function = GetComponent<Text>();

        IsRepair = false;

    }
    void Update()
    {
        fillamount(0f);
        Value_Percent(0f);

    }
    //void LateUpdate()
    //{
    //    transform.LookAt(transform.position + cam.forward);
    //}
    public void Need_Repair()
    {
        if ((maxhp * 50 / 100) <= hp)
        {
            hp_function.text = "";
        }
        else if ((maxhp * 30 / 100) <= hp)
        {
            hp_function.text = "수리 필요";
        }
        else if ((maxhp * 10 / 100) <= hp)
        {
            hp_function.text = "수리 필요!!";
        }
        else if ((maxhp * 0 / 100) <= hp)
        {
            hp_function.text = "수리 필요!!!!!";
        }

        if (IsRepair == true)
        {

        }
        else if (IsRepair == false)
        {

        }
    }

    public void Repair()
    {

    }

    public void Value_Percent(float value)
    {
        hp_percent.text = ((hp / maxhp) * 100f).ToString("F1") + "%";
    }

    public void fillamount(float value)
    {
        hp_fillamount.value = hp / maxhp;
    }

}
