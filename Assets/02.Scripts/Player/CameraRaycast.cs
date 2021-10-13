using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRaycast : MonoBehaviour
{

    #region LayerMask

    #endregion

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    /// <summary>
    /// 보고 있는 타겟 오브젝트를 그대로 넘겨주는 함수
    /// </summary>
    /// <param name="_raycastRange">Raycast disance</param>
    /// <param name="targerLayerMasks">Target layer mask. Need shift calculate</param>
    /// <returns>Return target RaycastHit<br/>If target is Empty return null </returns>
    /// 매개변수에 Raycast 대상이 될 LayerMask도 같이 받아서 처리해야겠다.
    /// 그래야지 철책뒤의 적에 대해서 인식을 할 수 있을 것이다.
    public RaycastHit GetRaycastTarget(float _raycastRange, LayerMask targerLayerMasks)
    {
        RaycastHit hit;

        if(Physics.Raycast(transform.position, transform.forward, out hit, _raycastRange, targerLayerMasks))
        {
<<<<<<< HEAD
            //Debug.Log("Hit Object: " + hit.transform.gameObject.name.ToString());

            //Debug.Log(target.tag);
        }

=======
            //target = hit.transform.gameObject;
            //Debug.Log(target.tag);
        }

        //Debug.Log(hit.point);
>>>>>>> parent of 21a53d0 (20211012_enemy수정)

        return hit;
    }


<<<<<<< HEAD
    public List<RaycastHit> GetWeaponRaycastTarget(float _raycastRange, LayerMask targetLayerMasks, ItemGun.GunType _gunType)
    {
        List<RaycastHit> _returnHit = new List<RaycastHit>();
        RaycastHit hit;

        // 샷건이 아닌 경우 한 발을 사용
        if (_gunType != ItemGun.GunType.SG)
        {
            if (Physics.Raycast(transform.position, transform.forward, out hit, _raycastRange, targetLayerMasks))
            {
                Debug.DrawLine(transform.position, transform.forward, Color.green, _raycastRange);

                _returnHit.Add(hit);
            }
        }
        // 샷건인 경우 5발을 사용
        else
        {
            // 샷건처럼 보이기 위해서 정면 기준으로 조금씩 움직일 값.
            float _value = 0.1f;

            // 정면
            if (Physics.Raycast(transform.position, transform.forward, out hit, _raycastRange, targetLayerMasks))
            {
                //Debug.Log("____ HIT 1 ____");
                _returnHit.Add(hit);
            }
            // 위
            if (Physics.Raycast(transform.position, transform.forward + new Vector3(0f, _value, 0f), out hit, _raycastRange, targetLayerMasks))
            //if (Physics.Raycast(transform.position, new Vector3(dir.x + _value, dir.y + _value, transform.forward.z), out hit, _raycastRange, targetLayerMasks))
            {
                //Debug.Log("____ HIT 2 ____");
                _returnHit.Add(hit);
            }
            // 아래
            if (Physics.Raycast(transform.position, transform.forward + new Vector3(0f, -_value, 0f), out hit, _raycastRange, targetLayerMasks))
            {
                //Debug.Log("____ HIT 3 ____");
                _returnHit.Add(hit);
            }
            // 좌
            if (Physics.Raycast(transform.position, transform.forward + new Vector3(-_value, 0f, 0f), out hit, _raycastRange, targetLayerMasks))
            {
                //Debug.Log("____ HIT 4 ____");
                _returnHit.Add(hit);
            }
            // 우
            if (Physics.Raycast(transform.position, transform.forward + new Vector3(_value, 0f, 0f), out hit, _raycastRange, targetLayerMasks))
            {
                //Debug.Log("____ HIT 5 ____");
                _returnHit.Add(hit);
            }

        }

        return _returnHit;
    }
=======
>>>>>>> parent of 21a53d0 (20211012_enemy수정)

}
