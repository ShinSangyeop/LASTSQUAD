using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InterfaceSet;

public class Wall_Dmage_Scrip : MonoBehaviour, IDamaged
{
    public GameObject Hp_Ui;
    public Hp_Bar hp_bar;
    public Build_Fence_Damage build_fence_damage;

    //[SerializeField]
    //float boxMaxHp;
    //[SerializeField]
    //float boxHp;

    public float viewRange = 15f;
    [Range(0, 360)]
    public float viewAngle = 120f;

    public float damage;

    Transform Walltr;
    Transform Playertr;

    Rigidbody rigid;
    BoxCollider boxcollder;

    void Awake()
    {
        //boxMaxHp = 150f;
        //boxHp = 150f;
        hp_bar = Hp_Ui.GetComponent<Hp_Bar>();
        //Hp_Ui.GetComponent<Hp_Bar>().hp = 150f;
        //Hp_Ui.GetComponent<Hp_Bar>().maxhp = 150f;
        hp_bar.maxhp = 150f;
        hp_bar.hp = 150f;

        damage = 10f;

        rigid = GetComponent<Rigidbody>();
        boxcollder = GetComponent<BoxCollider>();

        Walltr = GetComponent<Transform>();
        //Playertr = GetComponent <;
    }

    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag == "ENEMY")//적에게 대미지를 입엇을때
    //    {
    //        Bullet_Script enemy = other.GetComponent<Bullet_Script>();
    //        hp_bar.hp -= enemy.damage;//적의 공격력만큼 체력이 깎인다.
    //        /* boxHp -= enemy.damage;*/
    //        Debug.Log("___box_hit___ : " + enemy.damage);//적의 공격 대미지
    //        Debug.Log("___box_hp___ : " + hp_bar.hp);//구조물의 남은 체력

    //        hp_bar.Need_Repair();

    //        Die();
    //    }
    //    if (other.tag == "ENEMY")//적이 구조물을 지나갈때
    //    {
    //        Bullet_Script enemy = other.GetComponent<Bullet_Script>();
    //        enemy.hp -= damage;//적의 체력이 대미지만큼 피해를 입는다.
    //        Debug.Log("___enemy_damage___ :" + damage);//구조물의 대미지
    //        Debug.Log("___enemy_hp___ :" + enemy.hp);//적 남은 체력
    //        if (enemy.hp <= 0)
    //        {
    //            Destroy(enemy.gameObject);//적의 체력이 0이하가 되어 죽는다.
    //            Debug.Log("___DEAD___");
    //        }
    //        //Move_able pl = other.GetComponent<Move_able>();
    //        //pl.hp -= damage;//적의 체력이 대미지만큼 피해를 입는다.
    //        //Debug.Log("___damage___ :" + damage);//구조물의 대미지
    //        //Debug.Log("___enemy.hp___ :" + pl.hp);//적 남은 체력
    //        //if (pl.hp <= 0)
    //        //{
    //        //    Destroy(pl.gameObject);//적의 체력이 0이하가 되어 죽는다.
    //        //    Debug.Log("___DEAD___");
    //        //}
    //    }
    //}

    private void OnEnable()
    {
        hp_bar.hp = hp_bar.maxhp;
        hp_bar.hp_function.text = "";
        build_fence_damage.GetComponent<BoxCollider>().isTrigger = true;
    }

    //void OnCollisionEnter(Collision collision)
    //{
    //    //if (collision.collider.CompareTag("PLAYER"))//구조물에 부딪혔을 때
    //    //{
    //    //Move_able pl = collision.gameObject.GetComponent<Move_able>();
    //    //Debug.Log("___Collision___");//부딪힘을 인식
    //    //}
    //    if (collision.collider.CompareTag("ENEMY"))//적에게 대미지를 입엇을때
    //    {
    //        Bullet_Script enemy = collision.gameObject.GetComponent<Bullet_Script>();
    //        hp_bar.hp -= enemy.damage;//적의 공격력만큼 체력이 깎인다.
    //        Debug.Log("___Shoot___ : " + enemy.damage);//적의 공격 대미지
    //        Debug.Log("___Wall_hp___ : " + hp_bar.hp);//구조물의 남은 체력
    //        Die();
    //    }
    //}

    private void Update()
    {
        Ray();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ENEMY"))
        {
            LivingEntity enemy = other.GetComponent<LivingEntity>();

            enemy.Damaged(damage, Vector3.zero, Vector3.zero);
        }

    }


    void Ray()
    {
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(gameObject.transform.position, 5f);
    }
    public void Die()
    {
        Debug.Log($"__ hp: {hp_bar.hp}");
        if (hp_bar.hp <= 0)
        {
            //gameObject.SetActive(false);//구조물의 체력이 0이하가 되어 사라진다. 
            build_fence_damage.BuildingDestroy();
        }

    }

    public float Damaged(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        hp_bar.hp -= damage;

        if (hp_bar.hp <= 0f)
        {
            Die();
        }
        return 0;
    }

}
