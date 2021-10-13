using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall_script : MonoBehaviour
{
    public GameObject Hp_Ui;
    public Hp_Bar hp_bar;
    public Build_Fence build_fence;

    //[SerializeField]
    //float boxHp;
    //float boxMaxHp;

    Rigidbody rigid;
    BoxCollider boxcollder;

    void Awake()
    {
        hp_bar = Hp_Ui.GetComponent<Hp_Bar>();
        hp_bar.maxhp = 200f;
        hp_bar.hp = 200f;
        //Hp_Ui.GetComponent<Hp_Bar>().hp = 200f;
        //Hp_Ui.GetComponent<Hp_Bar>().maxhp = 200f;

        //boxMaxHp = 200f;
        //boxHp = 200f;

        rigid = GetComponent<Rigidbody>();
        boxcollder = GetComponent<BoxCollider>();
    }
    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag == "PLAYER")//구조물에 부딪혔을 때
    //    {
    //        Debug.Log("___Trigger___");//부딪힘을 인식
    //    }
    //    if (other.tag == "ENEMY")//적에게 대미지를 입엇을때
    //    {
    //        Bullet_Script bullet = other.GetComponent<Bullet_Script>();
    //        boxHp -= bullet.damage;//적의 공격력만큼 체력이 깎인다.
    //        Debug.Log("___Shoot___ : " + bullet.damage);//적의 공격 대미지
    //        Debug.Log("___Wall_hp___ : " + boxHp);//구조물의 남은 체력
    //        Die();
    //}
    private void OnEnable()
    {
        hp_bar.hp = hp_bar.maxhp;
        hp_bar.hp_function.text = "";
        build_fence.GetComponent<BoxCollider>().isTrigger = true;
    }
    void OnCollisionEnter(Collision collision)
    {
        //if (collision.collider.CompareTag("PLAYER"))//구조물에 부딪혔을 때
        //{
        //Move_able pl = collision.gameObject.GetComponent<Move_able>();
        //Debug.Log("___Collision___");//부딪힘을 인식
        //}
        if (collision.collider.CompareTag("ENEMY"))//적에게 대미지를 입엇을때
        {
           //hp_bar.hp -= enemy.damage;//적의 공격력만큼 체력이 깎인다.
           // Debug.Log("___Shoot___ : " + enemy.damage);//적의 공격 대미지
           // Debug.Log("___Wall_hp___ : " + hp_bar.hp);//구조물의 남은 체력

            hp_bar.Need_Repair();

            Die();
        }
    }
    private void Update()
    {
        Ray();
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
            build_fence.BuildingDestroy();
        }
    }
<<<<<<< HEAD

    public float Damaged(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        hp_bar.hp -= damage;

        if (hp_bar.hp <= 0f)
        {
            Die();
        }
        return 0;
    }
=======
>>>>>>> parent of 21a53d0 (20211012_enemy수정)
}
