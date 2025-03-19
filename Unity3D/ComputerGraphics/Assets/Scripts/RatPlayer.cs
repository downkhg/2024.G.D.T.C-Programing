using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatPlayer : MonoBehaviour
{
    public GameObject objTarget;
    public float fCosT;
    public float fAngle;
    public float fLibAngle;
    public Vector3 vAsix;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 vPos = transform.position;
        Vector3 vTargetPos = objTarget.transform.position;
        Vector3 vPlayerLook = transform.forward;
        Vector3 vTargetDir = (vTargetPos - vPos).normalized;

        vAsix = Vector3.Cross(vPlayerLook, vTargetDir);

        fCosT = Vector3.Dot(vPlayerLook, vTargetDir);

        fAngle = Mathf.Acos(fCosT) * Mathf.Rad2Deg;
        fLibAngle = Vector3.Angle(vPlayerLook, vTargetDir);

        transform.Rotate(vAsix, fLibAngle);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vPos = transform.position;

        Debug.DrawLine(vPos, vPos + transform.forward,Color.blue);
        Debug.DrawLine(vPos, vPos + vAsix,Color.green);
    }
}
