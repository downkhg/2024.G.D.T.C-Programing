using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : MonoBehaviour
{
    public Transform trTargetPoint;

    public Transform trResponPoint;
    public Transform trPatrolPoint;

    public float JumpPower;
    public bool isJump;
    public bool isMove;
    public float Speed = 1;
    public float Site = 1;

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
        switch (state)
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

    public bool MoveProcess()
    {
        if (trTargetPoint)
        {
            Vector3 vTargetPos = trTargetPoint.position;
            Vector3 vPos = this.transform.position;
            vTargetPos.y = 0;
            vPos.y = 0;
            Vector3 vDist = vTargetPos - vPos;//위치의 차이를 이용한 거리구하기
            Vector3 vDir = vDist.normalized;//두물체사이의 방향(평준화-거리를뺀 이동량)
            float fDist = vDist.magnitude; //두물체사이의 거리(스칼라-순수이동량)

            if (fDist > Speed *Time.deltaTime)//한프레임의 이동거리보다 클때만 이동한다.
            {
                transform.position += vDir * Speed * Time.deltaTime;
                isMove = true;
                return true;
            }
            else
            {
                isMove = false;
                Debug.Log("MoveProcess");
            }
        }
        return false;
    }

    public void FindProcess()
    {
        Vector3 vPos = this.transform.position;
        Collider2D collider = Physics2D.OverlapCircle(vPos, Site, 1 << LayerMask.NameToLayer("Player"));

        if (collider)
        {
            trTargetPoint = collider.transform;
            //SetState(E_AI_STATUS.TRACKING);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SetState(curAIState);
    }

    public float curTime;
    public float maxTime = 0.5f;

    // Update is called once per frame
    void Update()
    {
        if(curTime >= maxTime)
        {
            if (isJump == false)
            {
                Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
                rigidbody.velocity = Vector2.zero;
                rigidbody.AddForce(Vector3.up * JumpPower);
                isJump = true;
            }

            curTime = 0;
        }
        else curTime += Time.deltaTime;
        
        FindProcess();
        if(isJump) 
            MoveProcess();
        UpdateState();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, Site);

        Gizmos.color = Color.green;
        if (trResponPoint) Gizmos.DrawWireSphere(trResponPoint.position, Time.deltaTime);

        Gizmos.color = Color.red;
        if (trPatrolPoint) Gizmos.DrawWireSphere(trPatrolPoint.position, Time.deltaTime);

        Gizmos.color = Color.blue;
        if (trTargetPoint) Gizmos.DrawWireSphere(trTargetPoint.position, Time.deltaTime * 2);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isJump = false;
    }
}
