using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iventory : MonoBehaviour
{
    public List<Item> listItems;

    public void SetIventory(Item item)
    {
        listItems.Add(item);
    }

    public void UseIventory(Item item)
    {
        listItems.Remove(item);
    }

    private void OnGUI()
    {
        int w = 100;
        int h = 20;
        for(int i = 0; i < listItems.Count; i++)
        {
            GUI.Box(new Rect(0,i*h,w,h), listItems[i].gameObject.name);
        }
    }
}
