using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectCtrl : MonoBehaviour
{
    [SerializeField]
    GameObject bloodEffect;
    [SerializeField]
    GameObject sparkEffect;


    public Queue<BloodEffectCtrl> bloodQ = new Queue<BloodEffectCtrl>();
    public Queue<SparkEffectCtrl> sparkQ = new Queue<SparkEffectCtrl>();


    private static PlayerEffectCtrl instance = null;
    public static PlayerEffectCtrl Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        bloodEffect = Resources.Load<GameObject>("Prefabs/Effect/BloodEffect");
        sparkEffect = Resources.Load<GameObject>("Prefabs/Effect/SparkEffect");


        Init(5);
    }

    private void Init(int _count)
    {
        for (int i = 0; i < _count; i++)
        {
            bloodQ.Enqueue(CreateNewBloodEffect());
            sparkQ.Enqueue(CreateNewSparkEffect());
        }
    }


    private BloodEffectCtrl CreateNewBloodEffect()
    {
        var newObj = Instantiate(bloodEffect).GetComponent<BloodEffectCtrl>();
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(this.transform);
        return newObj;
    }

    public static BloodEffectCtrl GetBloodEffect()
    {
        if (Instance.bloodQ.Count > 0)
        {
            var obj = Instance.bloodQ.Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            var obj = Instance.CreateNewBloodEffect();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
    }

    public static void ReturnBloodEffect(BloodEffectCtrl _effect)
    {
        _effect.gameObject.SetActive(false);
        _effect.transform.SetParent(Instance.transform);
        Instance.bloodQ.Enqueue(_effect);
    }

    private SparkEffectCtrl CreateNewSparkEffect()
    {
        var newObj = Instantiate(sparkEffect).GetComponent<SparkEffectCtrl>();
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(this.transform);
        return newObj;
    }

    public static SparkEffectCtrl GetSparkEffect()
    {
        if (Instance.sparkQ.Count > 0)
        {
            var obj = Instance.sparkQ.Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            var obj = Instance.CreateNewSparkEffect();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
    }

    public static void ReturnSparkEffect(SparkEffectCtrl _effect)
    {
        _effect.gameObject.SetActive(false);
        _effect.transform.SetParent(Instance.transform);
        Instance.sparkQ.Enqueue(_effect);
    }



}
