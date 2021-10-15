using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyBuyButton : MyButton
{
    // 판매되는 아이템의 uid
    public string _uid;

    // 원래 판매할 아이템의 정보를 UID를 통해서 DBMangaer의 GetInformation과 같은 방식으로 정보를 가져와야 하는데
    // DB에 지금 Store관련 데이터를 안넣어놔서 public 변수를 사용해서 수동으로 하나하나 입력해두려고 한다.
    // 후에 수정할 시간이 있으면 수정해야한다.
    public int _price;
    public string _info;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
