using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall_Hp_Bar_Script : MonoBehaviour
{
    Camera cam;//카메라 변수
    Canvas can;//캔버스 변수
    RectTransform rectParent;//부모의 rectTransform 변수
    RectTransform rect;//자신의 rectTransform 변수

    public Vector3 offset = Vector3.zero;//Hp바 위치 조절
    public Transform wall;//방어물자 위치

    // Start is called before the first frame update
    void Start()
    {
        can = GetComponentInParent<Canvas>();
        cam = can.worldCamera;
        rectParent = can.GetComponent<RectTransform>();
        rect = this.gameObject.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        var screenPos = Camera.main.WorldToScreenPoint(wall.position + offset);

        var localPos = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, screenPos, cam, out localPos);

        rect.localPosition = localPos;
    }
}
