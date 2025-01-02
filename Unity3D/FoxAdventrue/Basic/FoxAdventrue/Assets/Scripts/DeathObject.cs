using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathObject : MonoBehaviour
{
    public float DeathY = -1;

    private void FixedUpdate()
    {
        Vector3 vPos = new Vector3(0, DeathY, 0);
        Debug.DrawLine(vPos + Vector3.left * 9999, vPos + Vector3.right * 9999, Color.red);
        if (transform.position.y < DeathZone.LineY)
        {
            Destroy(this.gameObject);
        }
    }
}
