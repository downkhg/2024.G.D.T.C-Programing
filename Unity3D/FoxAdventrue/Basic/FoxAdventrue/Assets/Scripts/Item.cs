using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int Score = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(gameObject.name + ".OnTriggerEnter2D:"+ collision.name);
        Dynamic dynamic = collision.gameObject.GetComponent<Dynamic>();

        //if (collision.gameObject.name == "player")
        if (collision.gameObject.tag == "Player")
        {
            dynamic.Score += Score;
            Destroy(this.gameObject);
        }
    }
}
