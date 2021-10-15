using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fence : DefensiveStructure
{

    protected override void Awake()
    {
        base.Awake();
        startHp = 200f;
    }


}
