using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store : MonoBehaviour
{
    /* 상점의 역할은 플레이어가 상호작용을 시도할 때
     * 현재 상점이 다른 플레이어가 상점을 사용하고 있는지 확인을 하고
     * 사용하고 있지 않으면 자식으로 가지고 있는 Canvas를 Player의 Child로 잠깐 옮겨준다.
     * 
     * Store Canvas에 들어갈 Script
     * 그때 플레이어가 이동하지 않게 PlayerCtrl의 isUIOpen을 true로 만들어버린다.
     * 그리고 마우스를 사용할 수 있도록 CursorState를 None으로(고정 해제) 하면 플레이어의 회전이 동작하지 않는다.
     * 이후 버튼을 누르면 각 버튼에 맞게 동작한다.
     */

    // 상점의 판매목록을 보여주는 Canvas
    public GameObject storeCanvas;
    // 상점을 사용하고 있는지 확인하는 변수
    public bool isUsed;

    // Start is called before the first frame update
    void Start()
    {
        isUsed = false;
    }

    /// <summary>
    /// 플레이어가 상점과 상호작용을 통해서 상점을 열었을 때 동작할 함수.
    /// </summary>
    public bool OpenStore(Transform _playerTr)
    {
        // 상점을 이미 사용하고 있을 경우 함수를 종료해서 아무런 동작을 하지 않게 한다.
        if (isUsed == true) { return false; }
        // 상점이 열렸으므로 isUsed를 true로 해준다.
        isUsed = true;
        // 상점의 Canvas를 활성화하고
        storeCanvas.SetActive(true);
        // 부모를 player로 만들어 준다.
        storeCanvas.transform.SetParent(_playerTr);
        // UI가 열렸음을 알림.
        _playerTr.GetComponent<PlayerCtrl>().isUIOpen = true;


        // Canvas의 설정
        StoreCtrl storeCtrl = storeCanvas.GetComponent<StoreCtrl>();
        Canvas _canvas = storeCtrl.GetComponent<Canvas>();
        _canvas.worldCamera = _playerTr.Find("UICamera").GetComponent<Camera>();
        _canvas.planeDistance = 0.0105f;



        return true;
    }

    /// <summary>
    /// 플레이어가 상점을 닫았을 때 동작할 함수.
    /// </summary>
    public void CloseStore(Transform _playerTr)
    {
        //Debug.Log("____ Store Closed ____");

        // Canvas의 Script를 통해서 자신에게 Canvas가 자식으로 돌아왔을 것이다.
        // 돌아온 Canvas를 비활성화 해준다.
        storeCanvas.SetActive(false);
        // 상점이 닫혔으므로 isUsed를 false로 해준다.
        isUsed = false;
        // UI를 닫았음을 알림.
        _playerTr.GetComponent<PlayerCtrl>().isUIOpen = false;

    }


}
