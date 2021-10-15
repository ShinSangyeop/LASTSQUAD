using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StoreButtonType
{
    public enum ButtonType
    {
        None = -1,
        WeaponType = 0, DefStructType, ItemType,
        RifleList = 10, SMGList, SGList,
        DefStructList = 20,
        ItemList = 30, PerkList,

        BuyButton = 50,
    }


}


public class MyButton : MonoBehaviour
{
    public StoreButtonType.ButtonType buttonType;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
