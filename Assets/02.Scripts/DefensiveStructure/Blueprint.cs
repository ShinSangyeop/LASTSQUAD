using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blueprint : MonoBehaviour
{
    // 실제 구조물
    public GameObject structure;
    // 청사진
    public GameObject bluePrint;
    private Collider _collider;

    // 구조물의 UID
    public string _uid;

    public bool isBuild;

    void Start()
    {
        _collider = GetComponent<Collider>();
        isBuild = false;
    }

    void Update()
    {
        // Unity 에디터에서만 동작하는 코드
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            BuildingBuild();
        }
#endif
    }

    public void BuildingBuild()
    {
        structure.SetActive(true);
        bluePrint.SetActive(false);
        gameObject.GetComponent<BoxCollider>().enabled = false;
        isBuild = true;
        _collider.enabled = false;
    }

    public void BuildingDestroy()
    {
        bluePrint.SetActive(true);
        structure.SetActive(false);
        gameObject.GetComponent<BoxCollider>().enabled = true;
        isBuild = false;
        _collider.enabled = true;
    }

    public void StartAutoRepair()
    {
        structure.GetComponent<DefensiveStructure>().BuildingAutoRepair();
    }
}
