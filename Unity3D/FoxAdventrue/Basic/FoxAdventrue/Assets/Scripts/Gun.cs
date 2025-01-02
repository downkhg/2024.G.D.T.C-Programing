using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject objBullet;
    public float Power;

    public Dynamic user;

    public enum E_BULLET_TYPE { BULLET, LESAER }
    public E_BULLET_TYPE typeBullet;

    public bool isLeaser = false;

    public Vector3 vLeaserEnd;
    public LineRenderer lineRenderer;


    public void SetBullet(E_BULLET_TYPE bullet)
    {
        switch(bullet)
        {
            case E_BULLET_TYPE.BULLET:
                break;
            case E_BULLET_TYPE.LESAER:
                
                break;
        }
        typeBullet = bullet;
    }

    void UpdateBullet()
    {
        switch (typeBullet)
        {
            case E_BULLET_TYPE.BULLET:
                break;
            case E_BULLET_TYPE.LESAER:
                {
                    if (isLeaser)
                    {
                        Vector3 vStart = this.transform.position;
                        Vector3 vEnd = vStart + (user.vDir * 99999);
                        RaycastHit2D raycastHit = Physics2D.Linecast(this.transform.position, vEnd, 1 << LayerMask.NameToLayer("Monster"));
                       
                        if (raycastHit.collider)
                        {
                            vLeaserEnd = raycastHit.point;
                            Destroy(raycastHit.collider.gameObject);
                            Debug.DrawLine(vStart, vLeaserEnd, Color.red);

                            lineRenderer.SetPosition(0, vStart);
                            lineRenderer.SetPosition(1, vLeaserEnd);
                        }
                        else
                        {
                            Debug.DrawLine(vStart, vEnd, Color.green);
                            lineRenderer.SetPosition(0, vStart);
                            lineRenderer.SetPosition(1, vEnd);
                        }
                    }
                }
                break;
        }
    }

    public void Shot(Vector3 dir)
    {
        switch (typeBullet)
        {
            case E_BULLET_TYPE.BULLET:
                GameObject copyBullet = Instantiate(objBullet, transform.position, Quaternion.identity);
                Rigidbody2D rigidbody = copyBullet.GetComponent<Rigidbody2D>();
                rigidbody.AddForce(dir * Power);
                break;
            case E_BULLET_TYPE.LESAER:
                if (isLeaser == true)
                {
                    isLeaser = false;
                    lineRenderer.SetPosition(1, this.transform.position);
                }
                else
                {
                    isLeaser = true;
                    
                }
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SetBullet(typeBullet);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBullet();
    }
}
