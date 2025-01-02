using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTracker : MonoBehaviour
{
    public Transform trTargetPoint;
    public float Speed = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveProcess();
    }

    public bool MoveProcess()
    {
        if (trTargetPoint)
        {
            Vector3 vTargetPos = trTargetPoint.position;
            Vector3 vPos = this.transform.position;
            Vector3 vDist = vTargetPos - vPos;//위치의 차이를 이용한 거리구하기
            Vector3 vDir = vDist.normalized;//두물체사이의 방향(평준화-거리를뺀 이동량)
            float fDist = vDist.magnitude; //두물체사이의 거리(스칼라-순수이동량)

            if (fDist > Time.deltaTime)//한프레임의 이동거리보다 클때만 이동한다.
            {
                transform.position += vDir * Speed * Time.deltaTime;
                return true;
            }
        }
        return false;
    }
}
