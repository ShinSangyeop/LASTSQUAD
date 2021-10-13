using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefensiveStructure : LivingEntity
{

    public GameObject Hp_Ui;
    Blueprint blueprint;

    BoxCollider boxCollder;

    // Ray 함수 실행 시간
    private float rayTime;
    // Ray 함수 실행 주기
    private float rayDelay;

    protected virtual void Awake()
    {
        try
        {
            blueprint = transform.parent.GetComponent<Blueprint>();
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"____ {this.gameObject.name} {e} ____");
        }

        boxCollder = GetComponent<BoxCollider>();

        rayDelay = 0.5f;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }


    private void Update()
    {
        Ray();
    }

    void Ray()
    {
        rayTime += Time.deltaTime;

        // 일정시간에 한 번 정도 실행
        if (rayTime >= rayDelay)
        {
            rayTime -= rayDelay;
            Collider[] colliders = Physics.OverlapSphere(gameObject.transform.position, 5f, 1 << LayerMask.NameToLayer("PLAYER"));

            if (colliders.Length == 0)
            {
                Hp_Ui.SetActive(false);
            }
            else if (colliders.Length > 0)
            {
                Hp_Ui.SetActive(true);
            }
            //Debug.Log(colliders.Length);
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(gameObject.transform.position, 5f);
    }

    // 파괴되었을 때 실행
    public override void OnDeath()
    {
        base.OnDeath();
        Debug.Log($"__ hp: {currHP}");
        if (currHP <= 0)
        {
            //gameObject.SetActive(false);//구조물의 체력이 0이하가 되어 사라진다. 
            blueprint.BuildingDestroy();
        }
    }

    public override float Damaged(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        base.Damaged(damage, hitPoint, hitNormal);
        return 0;

    }

    public void Repair()
    {
        if (currHP < startHp)
        {
            currHP = startHp;
        }
    }

    public void BuildingAutoRepair()
    {
        StartCoroutine(CoBuildingAutoRepair());
    }

    IEnumerator CoBuildingAutoRepair()
    {
        yield return new WaitForSeconds(5f);
        currHP += 5f;
        if (currHP >= startHp)
        {
            currHP = startHp;
        }
    }


}
