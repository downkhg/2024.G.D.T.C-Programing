using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eagle : MonoBehaviour
{
    public float Speed = 1;
    public float Site = 1;

    public Transform trTargetPoint;
    public Transform trResponPoint;
    public Transform trPatrolPoint;

    public bool isPatrol;
    public bool isReturn;
    public bool isMove;
    public bool isTracking;

    public enum E_AI_STATUS
    {
        NONE,
        TRACKING,
        RETURN,
        PATROL
    }

    public E_AI_STATUS curAIState = E_AI_STATUS.NONE;

    public void SetState(E_AI_STATUS state)
    {
        switch(state)
        {
            case E_AI_STATUS.TRACKING:
                
                break;
            case E_AI_STATUS.RETURN:
                trTargetPoint = trResponPoint;
                break;
            case E_AI_STATUS.PATROL:
                trTargetPoint = trPatrolPoint;
                break;
        }
        curAIState = state;
    }

    public void UpdateState()
    {
        switch (curAIState)
        {
            case E_AI_STATUS.TRACKING:
                if (trTargetPoint == null)
                    SetState(E_AI_STATUS.RETURN);
                break;
            case E_AI_STATUS.RETURN:
                if (trTargetPoint)
                {
                    if (isMove == false)
                    {
                        SetState(E_AI_STATUS.PATROL);
                    }
                }
                break;
            case E_AI_STATUS.PATROL:
                if (trTargetPoint)
                {
                    if (isMove == false)
                    {
                        if (trTargetPoint.gameObject.name == trPatrolPoint.gameObject.name)
                        {
                            trTargetPoint = trResponPoint;
                        }
                        else if (trTargetPoint.gameObject.name == trResponPoint.gameObject.name)
                        {
                            trTargetPoint = trPatrolPoint;
                        }
                    }
                }
                break;
        }
    }

    private void OnDisable()
    {
        Instantiate(this.gameObject, trResponPoint.position, Quaternion.identity).GetComponent<Eagle>().enabled = true;
        //GameObject objEagle = Instantiate(this.gameObject, trResponPoint.position, Quaternion.identity);
        //objEagle.SetActive(true);
        //objEagle.GetComponent<Eagle>().enabled = true;
    }

    private void OnDestroy()
    {
        Debug.Log("OnDestroy:"+gameObject.name);
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, Site);

        Gizmos.color = Color.green;
        if(trResponPoint) Gizmos.DrawSphere(trResponPoint.position, Time.deltaTime);

        Gizmos.color = Color.red;
        if(trPatrolPoint) Gizmos.DrawSphere(trPatrolPoint.position, Time.deltaTime);

        Gizmos.color = Color.blue;
        if(trTargetPoint) Gizmos.DrawWireSphere(trTargetPoint.position, Time.deltaTime*2);
    }

    private void Start()
    {
        SetState(curAIState);
    }


    private void FixedUpdate()
    {
        FindProcess();
        MoveProcess();
        UpdateState();
    }

    public void FindProcess()
    {
        Vector3 vPos = this.transform.position;
        Collider2D collider = Physics2D.OverlapCircle(vPos, Site, 1 << LayerMask.NameToLayer("Player"));

        if (collider)
        {
            trTargetPoint = collider.transform;
            SetState(E_AI_STATUS.TRACKING);
        }
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
                isMove = true;
                return true;
            }
            else
                isMove = false;
        }
        return false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //if(collision.gameObject.tag == "Player")
        //{
        //    Vector3 vPos = this.transform.position;
        //    Vector3 vTargetPos = collision.transform.position;
        //    Vector3 vDist = vTargetPos - vPos;//위치의 차이를 이용한 거리구하기
        //    Vector3 vDir = vDist.normalized;//두물체사이의 방향(평준화-거리를뺀 이동량)
        //    float fDist = vDist.magnitude; //두물체사이의 거리(스칼라-순수이동량)

        //    if(fDist > Time.deltaTime)//한프레임의 이동거리보다 클때만 이동한다.
        //        transform.position += vDir * Speed * Time.deltaTime;
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            Destroy(this.gameObject);
        }

        if (collision.gameObject.tag == "Player")
        {
            Destroy(collision.gameObject);
        }
    }
}
