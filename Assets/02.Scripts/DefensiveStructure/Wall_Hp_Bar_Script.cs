using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall_Hp_Bar_Script : MonoBehaviour
{
    Camera cam;//ī�޶� ����
    Canvas can;//ĵ���� ����
    RectTransform rectParent;//�θ��� rectTransform ����
    RectTransform rect;//�ڽ��� rectTransform ����

    public Vector3 offset = Vector3.zero;//Hp�� ��ġ ����
    public Transform wall;//���� ��ġ

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
