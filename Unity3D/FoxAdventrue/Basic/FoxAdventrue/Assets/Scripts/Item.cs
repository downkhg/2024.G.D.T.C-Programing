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

    public void UseItem(Dynamic dynamic)
    {
        ItemInfo itemInfo = GameManager.GetInstance().ItemManager.GetItemInfo(Itemtype);
        itemInfo.Use(dynamic);
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
        //if (collision.gameObject.name == "player")
        if (collision.gameObject.tag == "Player")
        {
            ItemInfo itemInfo = GameManager.GetInstance().ItemManager.GetItemInfo(this.Itemtype);
            //UseItem(collision.gameObject.GetComponent<Dynamic>());
            GameManager.GetInstance().iventoryPlayer.SetIventory(itemInfo);
            Destroy(this.gameObject);
        }
    }
}
