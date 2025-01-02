using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eagle : MonoBehaviour
{
    public float Speed = 1;
    public float Site = 1;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, Site);
    }

    private void FixedUpdate()
    {
        Vector3 vPos = this.transform.position;
        Collider2D collider = Physics2D.OverlapCircle(vPos, Site, 1<<LayerMask.NameToLayer("Player"));

        if(collider && collider.gameObject.tag == "Player")
        {
            Debug.Log("OverlapCircle:" + collider.gameObject.name);
            Vector3 vTargetPos = collider.transform.position;
            Vector3 vDist = vTargetPos - vPos;//위치의 차이를 이용한 거리구하기
            Vector3 vDir = vDist.normalized;//두물체사이의 방향(평준화-거리를뺀 이동량)
            float fDist = vDist.magnitude; //두물체사이의 거리(스칼라-순수이동량)

            if (fDist > Time.deltaTime)//한프레임의 이동거리보다 클때만 이동한다.
                transform.position += vDir * Speed * Time.deltaTime;
        }
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
    }
}
