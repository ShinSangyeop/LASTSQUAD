using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodEffectCtrl : MonoBehaviour
{
    private float removeTime;
    private float _time;

    // Start is called before the first frame update
    void Start()
    {
        removeTime = 0.35f;
    }

    private void OnEnable()
    {
        _time = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (_time >= removeTime)
        {
            _time = 0f;
            PlayerEffectCtrl.ReturnBloodEffect(this);
        }
        else
        {
            _time += Time.deltaTime;
        }
    }



}
