using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemManager.E_ITEM_TYPE Itemtype;

    public void SetItemColor()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        switch (Itemtype)
        {
            case ItemManager.E_ITEM_TYPE.SUPERMODE:
                spriteRenderer.color = Color.yellow;
                break;
            case ItemManager.E_ITEM_TYPE.LEASER:
                spriteRenderer.color = Color.cyan;
                break;
        }
    }

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
        //Debug.Log(gameObject.name + ".OnTriggerEnter2D:"+ collision.name);
        Dynamic dynamic = collision.gameObject.GetComponent<Dynamic>();

        //if (collision.gameObject.name == "player")
        if (collision.gameObject.tag == "Player")
        {
             GameManager.GetInstance().ItemManager.GetItemInfo(Itemtype);
        }
    }
}
