using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iventory : MonoBehaviour
{
    public List<ItemInfo> listItems;

    public void SetIventory(ItemInfo item)
    {
        listItems.Add(item);
    }

    public ItemInfo GetIventory(int idx)
    {
        if(listItems.Count > 0)
            return listItems[idx];
        else
            return null;
    }

    public void UseIventory(ItemInfo item, Dynamic dynamic)
    {
        item.Use(dynamic);
        listItems.Remove(item);
    }

    private void OnGUI()
    {
        int w = 100;
        int h = 20;
        for(int i = 0; i < listItems.Count; i++)
        {
            if(GUI.Button(new Rect(0,i*h,w,h), listItems[i].name))
            {
                UseIventory(listItems[i], GameManager.GetInstance().resoponnerPlayer.objTarget.GetComponent<Dynamic>());
            }
        }
    }
}
