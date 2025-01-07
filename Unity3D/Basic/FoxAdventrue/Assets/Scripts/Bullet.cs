using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float ShotDist;
    Vector3 vStartPos;
    // Start is called before the first frame update
    void Start()
    {
        vStartPos = transform.position;
        Destroy(this.gameObject, 1);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vCurPos = transform.position;//

        Vector3 vDist = vCurPos - vStartPos;
        //float fDist = vDist.magnitude;
        float fDist = Vector3.Distance(vCurPos, vStartPos);

        if (fDist >= ShotDist)
            Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Monster")
        {
            Player player = GameManager.GetInstance().resoponnerPlayer.objTarget.GetComponent<Player>();
            Player target = collision.GetComponent<Player>();

            player.Attack(target);
        }
    }
}
