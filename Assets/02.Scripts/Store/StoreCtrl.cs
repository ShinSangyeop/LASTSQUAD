using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StoreCtrl : MonoBehaviour, UnityEngine.EventSystems.IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Transform originParent;

    // 이전 버튼을 저장하는 변수
    GameObject _preButton;
    // UI의 이전 색을 저장하는 변수
    Color _preColor;


    // 지금은 상점의 버튼과 Canvas 등을 전부 하나하나 프리팹으로 생성해뒀지만
    // 나중에 JSON 같은 것으로 정리하면 열 때 정보를 가져와서 로드하는 방식을 사용할 수 있을 것이다.

    // 상점에서 판매하는 것들의 종류를 구분한 버튼 리스트
    //public List<UnityEngine.UI.Button> typeButtonList;
    public List<MyButton> typeButtonList;
    // 구분된 종류를 표시할 패널 리스트
    public List<UnityEngine.UI.Image> typePanelList;

    // 무기의 종류를 구분하는 버튼 리스트
    //public List<UnityEngine.UI.Button> weaponTypeButtonList;
    public List<MyButton> weaponTypeButtonList;
    // 구분된 패널 리스트
    public List<UnityEngine.UI.Image> weaponTypePanelList;
    // Rifle 리스트
    //public List<UnityEngine.UI.Button> rifleList;
    public List<MyButton> rifleList;
    // SMG 리스트
    //public List<UnityEngine.UI.Button> smgList;
    public List<MyButton> smgList;
    // SG 리스트
    //public List<UnityEngine.UI.Button> sgList;
    public List<MyButton> sgList;

    // 아이템의 종류를 구분하는 버튼 리스트
    //public List<UnityEngine.UI.Button> itemTypeButtonList;
    public List<MyButton> itemTypeButtonList;
    // 구분된 아이템 패널 리스트
    public List<UnityEngine.UI.Image> itemTypePanelList;
    // 아이템 리스트
    //public List<UnityEngine.UI.Button> itemList;
    public List<MyButton> itemList;
    // 특전 리스트
    //public List<UnityEngine.UI.Button> perkList;
    public List<MyButton> perkList;

    // 방어 물자의 정류를 구분하는 버튼 리스트
    //public List<UnityEngine.UI.Button> defStructTypeButtonList;
    public List<MyButton> defStructTypeButtonList;
    // 구분된 패널 리스트
    public List<UnityEngine.UI.Image> defStructTypePanelList;
    // 방어 물자 리스트
    //public List<UnityEngine.UI.Button> defStructList;
    public List<MyButton> defStructList;


    // 판매 목록의 정보를 표시해주는 텍스트
    public UnityEngine.UI.Text infoText;


    // Start is called before the first frame update
    void Start()
    {
        _preColor = new Color();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnStoreCloseBtn();
        }
    }

    private void OnEnable()
    {
        // 기존에 있었던 위치(상점)을 받아온다.
        originParent = transform.parent.transform;
        // UI가 켜졌으므로 커서 고정을 끈다.
        CursorState.CursorLockedSetting(false);


    }


    /// <summary>
    /// 상점을 닫는 버튼을 눌렀을 때 동작하는 함수
    /// </summary>
    public void OnStoreCloseBtn()
    {
        //Debug.Log("____ Close Button Click ____");

        Transform playerTr = transform.parent.GetComponent<Transform>();
        // Store Canvas를 Store의 자식으로 다시 되돌린다.
        transform.SetParent(originParent);
        // Store가 가지고 있는 Close Store 함수를 실행시킨다.
        originParent.GetComponent<Store>().CloseStore(playerTr);
        // UI가 꺼질 것이므로 커서 고정을 켠다
        CursorState.CursorLockedSetting(true);
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        MyButton _myButton = eventData.pointerCurrentRaycast.gameObject.GetComponent<MyButton>();

        Debug.Log("____ On Pointer Click: " + eventData.pointerCurrentRaycast.gameObject.name + " ____");

        switch (_myButton.buttonType)
        {
            // 무기 목록 표시
            case StoreButtonType.ButtonType.WeaponType:
                TypePanelActive(0);
                break;
            // 방어 물자 목록 표시
            case StoreButtonType.ButtonType.DefStructType:
                TypePanelActive(1);
                break;
            // 아이템 목록 표시
            case StoreButtonType.ButtonType.ItemType:
                TypePanelActive(2);
                break;
            // 라이플 목록 표시
            case StoreButtonType.ButtonType.RifleList:
                WeaponPanelActive(0);
                break;
            // 기관단총 목록 표시
            case StoreButtonType.ButtonType.SMGList:
                WeaponPanelActive(1);
                break;
            // 샷건 목록 표시
            case StoreButtonType.ButtonType.SGList:
                WeaponPanelActive(2);
                break;
            // 방어 물자 목록 표시
            case StoreButtonType.ButtonType.DefStructList:
                DefStructPanelActive(0);
                break;
            // 사용 아이템 목록 표시
            case StoreButtonType.ButtonType.ItemList:
                ItemPanelActive(0);
                break;
            // 특전 목록 표시
            case StoreButtonType.ButtonType.PerkList:
                ItemPanelActive(1);
                break;
            // 구매 아이템 버튼 클릭.
            case StoreButtonType.ButtonType.BuyButton:
                StartCoroutine(CoCheckSuccessBuy(BuyItem(_myButton.GetComponent<MyBuyButton>())));


                break;
            default:

                break;

        }

    }

    IEnumerator CoCheckSuccessBuy(bool _success)
    {
        // 이전에 클릭한 오브젝트를 따로 저장한다.
        GameObject _checkObject = _preButton;
        Color _preCheckColor = _preColor;
        if (!_success)
            _checkObject.GetComponent<UnityEngine.UI.Image>().color = Color.red;
        else
            _checkObject.GetComponent<UnityEngine.UI.Image>().color = Color.blue;
        yield return new WaitForSeconds(0.1f);
        _checkObject.GetComponent<UnityEngine.UI.Image>().color = _preCheckColor;

    }



    #region Panel Active 모음
    // 판매 목록 패널
    private void TypePanelActive(int _idx)
    {
        // 반복문이 돌아가는데 코루틴으로 돌리는게 맞을지도 모르겠다.

        // 위랑 아래 중에 뭐가 더 빠른지는 모르겠다. 위에 어디갔어...
        for (int i = 0; i < typePanelList.Count; i++)
        {
            if (i == _idx)
            {
                typePanelList[i].gameObject.SetActive(true);
            }
            else
            {
                typePanelList[i].gameObject.SetActive(false);
            }
        }
    }
    // 무기 패널
    private void WeaponPanelActive(int _idx)
    {

        for (int i = 0; i < weaponTypePanelList.Count; i++)
        {
            if (i == _idx)
            {
                weaponTypePanelList[i].gameObject.SetActive(true);
            }
            else
            {
                weaponTypePanelList[i].gameObject.SetActive(false);
            }
        }
    }
    // 방어 물자 패널
    private void DefStructPanelActive(int _idx)
    {

        for (int i = 0; i < defStructTypePanelList.Count; i++)
        {
            if (i == _idx)
            {
                defStructTypePanelList[i].gameObject.SetActive(true);
            }
            else
            {
                defStructTypePanelList[i].gameObject.SetActive(false);
            }
        }
    }
    // 아이템 패널
    private void ItemPanelActive(int _idx)
    {

        for (int i = 0; i < itemTypePanelList.Count; i++)
        {
            if (i == _idx)
            {
                itemTypePanelList[i].gameObject.SetActive(true);
            }
            else
            {
                itemTypePanelList[i].gameObject.SetActive(false);
            }
        }
    }
    #endregion

    #region Information Text 설정


    // 마우스를 올렸을 때 정보를 가져오기 위해서 사용
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("___ Pointer Enter: " + eventData.pointerCurrentRaycast.gameObject.name + " ____");
        try
        {
            _preButton = eventData.pointerCurrentRaycast.gameObject;
            // 마우스가 올라간 버튼의 기존 색을 _preColor에 저장하고 회색으로 한다.
            _preColor = _preButton.GetComponent<UnityEngine.UI.Image>().color;
            _preButton.GetComponent<UnityEngine.UI.Image>().color = Color.gray;

            // 구매할 수 있는 목록의 버튼에 올라가면 그 버튼의 아이템 정보를 가져온다.
            if (_preButton.GetComponent<MyButton>().buttonType == StoreButtonType.ButtonType.BuyButton)
            {
                MyBuyButton buyButton = _preButton.GetComponent<MyBuyButton>();

                // text에 문자를 저장할 때 string Builder라는 것을 사용하는 것이 좋다고 한다. 후에 찾아보자.
                // 아이템의 정보를 표시해준다.
                infoText.text = buyButton._info;
                infoText.text += $"   가격: {buyButton._price}";
            }

        }
        catch (System.Exception e)
        {
            // 어떤 오브젝트에 닿아서 어떤 에러가 발생했는지 확인
            Debug.LogWarning(_preButton.name + e.Message);
        }

    }

    // 빈 곳으로 마우스가 옮겼을 때 이전 정보를 초기화하기 위해서 사용
    public void OnPointerExit(PointerEventData eventData)
    {
        try
        {
            // 정보가 없으므로 정보를 비워준다.
            infoText.text = "";
            // 버튼 영역을 벗어났으므로 기존의 색으로 되돌려 준다.
            _preButton.GetComponent<UnityEngine.UI.Image>().color = _preColor;
        }
        catch (System.Exception e)
        {
            Debug.LogWarning(e);
        }

    }

    #endregion





    private bool BuyItem(MyBuyButton _myBuyButton)
    {
        bool canBuy = true;

        PlayerCtrl _playerCtrl = transform.parent.GetComponent<PlayerCtrl>();
        // 보유한 포인트와 가격의 비교를 통해서 구매 가능 여부를 표시해줄 필요가 있다.
        if (_playerCtrl._point < _myBuyButton._price)
        {
            return false;
        }


        // 구매할 물건의 UID를 통해서 어떤 물건을 구매하는지 확인
        string firstUID = _myBuyButton._uid.Substring(0, 2);
        string middleUID = _myBuyButton._uid.Substring(2, 3);
        string lastUID = _myBuyButton._uid.Substring(5, 4);


        // 무기 구매
        if (firstUID == "01")
        {
            Debug.Log("____ My Buy Button Item UID: " + _myBuyButton._uid + " ____");
            // 동일한 무기를 가지고 있으면 스킵하는 기능 필요

            canBuy = _playerCtrl.PlayerWeaponChange(_myBuyButton._uid);

        }
        // 아이템 구매
        else if (firstUID == "06")
        {
            // 플레이어가 이미 아이템(회복 아이템, 방어물자)을 가지고 있으면 추가적인 구매를 할 수 없다.
            // 회복 아이템의 경우에만
            if (_playerCtrl.isHaveItem == true && middleUID == "000" && lastUID == "0000")
            {
                return false;
            }
            // 지금은 경우의 수가 몇 없기 때문에 그냥 if로 처리했는데
            // 개수가 늘어나면 문제가 생길 수 있다.(코드 작성이 어려워진다.)
            // 때문에 ItemSetting 쪽에서 bool 값을 return 하고 그 값을 return 하는 것도 방법일 것이다.

            // 특전의 경우 현재 어떤 특전을 구매하려고 하는지 비교할 마땅한 변수가 없어서
            // ItemSetting Bool 값으로 반환하게 처음부터 만드는게 맞았던것 같다.
            // 즉 구매하는 목록의 종류만 넘겨주고 어떤 물건을 구매하는지
            // 구매가 가능한지는 ItemSetting에서 했었어야 했다.
            canBuy = _playerCtrl.ItemSetting(_myBuyButton._uid);
        }
        // 방어 물자 구매
        else if (firstUID == "07")
        {
            // 이미 아이템을 가지고 있으면 추가적인 구매를 할 수 없다.
            if (_playerCtrl.isHaveItem == true)
            {
                return false;
            }

            canBuy = _playerCtrl.ItemSetting(_myBuyButton._uid);
        }

        // 물건을 구매할 수 있을 때만 포인트를 감소시킨다.
        if (canBuy == true)
        {
            _playerCtrl._point -= _myBuyButton._price;
            _playerCtrl.ActionTextSetting("구매 완료");
        }
        return canBuy;
    }

}
