using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboaard : MonoBehaviour
{
    public Renderer renderer;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Camera.main.transform);
    }

    public Vector2 vUV;

    private void FixedUpdate()
    {
        vUV.x += 0.25f;
        vUV.y += 0.25f;

        renderer.material.SetTextureOffset("_MainTex", vUV);
    }
}
