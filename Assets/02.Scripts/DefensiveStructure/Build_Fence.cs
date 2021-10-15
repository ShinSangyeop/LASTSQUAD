using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Build_Fence : MonoBehaviour
{
    //public GameObject Wall_Damage;
    public GameObject Wall;
    public GameObject BluePrint;

    Wall_script wall_;
    //Wall_Dmage_Scrip wall_Dmage_;

    // true:    false: 
    public bool IsBuild;

    void Start()
    {
        //wall_ = Wall.GetComponent<Wall_script>();
        wall_ = Wall.GetComponent<Wall_script>();

        
        IsBuild = false;
    }

    void Update()
    {
        //Install();

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            BUildingBuild();
        }
    }

    public void BUildingBuild()
    {
        Wall.SetActive(true);
        BluePrint.SetActive(false);
        gameObject.GetComponent<BoxCollider>().enabled = false;
        IsBuild = true;
    }

    public void BuildingDestroy()
    {
        BluePrint.SetActive(true);
        Wall.SetActive(false);
        gameObject.GetComponent<BoxCollider>().enabled = true;
        IsBuild = false;
    }


}
