using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{

    private Transform tr; // 카메라의 위치
    public Transform neckTr; // 카메라가 기준으로 잡을 목 위치

    public float forward;
    public float up;
    public float right;


    public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
        offset = new Vector3(0.1f, -0.06f, 0.01f);

    }


    // Update is called once per frame
    void Update()
    {

        //Vector3 pos = new Vector3();
        //Vector3 value = tr.forward * (-0.035f) + tr.up * 0.075f + tr.right * (-0.035f);
        //pos = value + neckTr.position;

        //tr.position = Vector3.Lerp(tr.position, pos, 4f * Time.deltaTime);
        ////tr.up = transform.up;

    }

    private void LateUpdate()
    {
        // 목의 위치를 기준으로 Off Set 값 만큼 항상 상대적으로 위쪽에 있게 설정.
        tr.position = neckTr.position - (neckTr.up * offset.y + neckTr.right * offset.x + neckTr.forward * offset.z);

        //Debug.Log("Cross: " + Vector3.Cross((neckTr.right * right + neckTr.up * up + neckTr.forward * forward), offset));
        //Debug.Log("Dot: " + Vector3.Dot((neckTr.right * right + neckTr.up * up + neckTr.forward * forward), offset));

    }


}
