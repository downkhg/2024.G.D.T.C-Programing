using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opossum : MonoBehaviour
{
    public float Speed = 1;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.left * Speed * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        Vector3 vPos = this.transform.position;
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        Collider2D collider = Physics2D.OverlapBox(vPos, boxCollider.size, 0, 1 << LayerMask.NameToLayer("Player"));

        if (collider)
        {
            if (collider.gameObject.GetComponent<Dynamic>().isSuperMode == false)
                Destroy(collider.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag ==  "Player")
            if (collision.gameObject.GetComponent<Dynamic>().isSuperMode == false)
                Destroy(collision.gameObject);

        if (collision.gameObject.tag == "DeathZone")
            Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            Destroy(this.gameObject);
        }
    }
}
