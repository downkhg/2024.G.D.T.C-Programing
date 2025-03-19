using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class ReflectBall : MonoBehaviour
{
    public Transform trTarget;
    public float Speed;

    public Vector3 vDir;

    // Start is called before the first frame update
    void Start()
    {
        vDir = trTarget.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += vDir * Speed * Time.deltaTime;
        Debug.DrawRay(transform.position, vDir, Color.blue);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(gameObject.name+".OnTriggerEnter:"+other.gameObject.name);
        ReflectPlane reflectPlane = other.GetComponent<ReflectPlane>();
        if (reflectPlane)
        {
            Vector3 vReflect = Vector3.Reflect( vDir, reflectPlane.GetNomal());
            Debug.DrawLine(transform.position, vReflect, Color.green);
            vDir = vReflect;
        }
    }
}
