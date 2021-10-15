using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    LivingEntity _parent;

    public Slider hp_fillamount;//hp 슬라이더 바
    public Text hp_percent;//hp바 남은 퍼센테이지 텍스트
    public Text hp_function;//hp바 알림 텍스트
    //Transform cam;


    bool IsRepair;

    void Start()
    {
        _parent = transform.parent.GetComponent<LivingEntity>();
        //hp_percent = GetComponent<Text>();
        //hp_function = GetComponent<Text>();

        IsRepair = false;

    }
    void Update()
    {
        fillamount(0f);
        Value_Percent(0f);

    }

    private void OnEnable()
    {
        hp_function.text = "";
    }

    public void Need_Repair()
    {
        if ((_parent.startHp * 50 / 100) <= _parent.currHP)
        {
            hp_function.text = "";
        }
        else if ((_parent.startHp * 30 / 100) <= _parent.currHP)
        {
            hp_function.text = "수리 필요";
        }
        else if ((_parent.startHp * 10 / 100) <= _parent.currHP)
        {
            hp_function.text = "수리 필요!!";
        }
        else if ((_parent.startHp * 0 / 100) <= _parent.currHP)
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
        hp_percent.text = ((_parent.currHP / _parent.startHp) * 100f).ToString("F1") + "%";
    }

    public void fillamount(float value)
    {
        hp_fillamount.value = _parent.currHP / _parent.startHp;
    }




}
