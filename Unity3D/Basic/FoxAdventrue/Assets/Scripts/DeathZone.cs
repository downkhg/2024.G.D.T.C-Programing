using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    public static float LineY = -1;

    private void FixedUpdate()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(collision.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(collision.gameObject);
    }
}
