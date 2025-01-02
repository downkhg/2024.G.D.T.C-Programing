using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject objBullet;
    public float Power;

    public void Shot(Vector3 dir)
    {
        GameObject copyBullet = Instantiate(objBullet, transform.position, Quaternion.identity);
        Rigidbody2D rigidbody = copyBullet.GetComponent<Rigidbody2D>();
        rigidbody.AddForce(dir * Power);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
