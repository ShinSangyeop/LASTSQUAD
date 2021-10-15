using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarbedWire : DefensiveStructure
{

    protected override void Awake()
    {
        base.Awake();
        startHp = 150f;
        damage = 2f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ENEMY"))
        {
            // 적에게만 데미지를 준다.
            other.GetComponent<LivingEntity>().Damaged(damage, Vector3.zero, Vector3.zero);
            // 적에게 데미지를 주고나면 10의 피해를 받는다.
            Damaged(10f, Vector3.zero, Vector3.zero);
        }
    }

}
