using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hp_Bar : MonoBehaviour
{
    public float hp;
    public float maxhp;


    public Slider hp_fillamount;//hp �����̴� ��
    public Text hp_percent;//hp�� ���� �ۼ������� �ؽ�Ʈ
    public Text hp_function;//hp�� �˸� �ؽ�Ʈ
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
            hp_function.text = "���� �ʿ�";
        }
        else if ((maxhp * 10 / 100) <= hp)
        {
            hp_function.text = "���� �ʿ�!!";
        }
        else if ((maxhp * 0 / 100) <= hp)
        {
            hp_function.text = "���� �ʿ�!!!!!";
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
