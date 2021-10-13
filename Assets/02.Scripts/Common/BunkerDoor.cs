using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunkerDoor : DefensiveStructure
{
    protected override void Awake()
    {
        base.Awake();
        startHp = 250f;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    protected override void OnEnable()
    {
        base.OnEnable();
        currHP = startHp;
        Debug.Log($"____ Bunker Hp {currHP} ____");
    }

    public override void OnDeath()
    {
        GameManager.instance.GameOver();

    }

    public override float Damaged(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        base.Damaged(damage, hitPoint, hitNormal);
        Debug.Log($"____ Bunker Curr Hp: {this.currHP} ____");
        return 0;
    }


}
