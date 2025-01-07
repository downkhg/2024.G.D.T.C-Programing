using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIIventory : MonoBehaviour
{
    public GUIItemInfoPanel guiIteminfoPanel;
    public List<GUIItemButton> listItemButton;
    public RectTransform rectItemContent;

    public void InitializeGUI(Iventory iventory, Dynamic dynamic)
    {
        SetItemButtons(iventory, dynamic);
        if(iventory !=null && iventory.listItems.Count > 0)
            guiIteminfoPanel.SetItemInfoPannel(this,iventory.GetIventory(0), dynamic);
    }

    public void ReleaseGUI()
    {
        ReleaseItemButtons();
    }

    public void SetItemButtons(Iventory iventory, Dynamic dynamic)
    {
        GameObject prefabButton = Resources.Load("Prefabs/GUI/ItemButton") as GameObject;
        Debug.LogFormat("SetItemButtons[{0}]", iventory.listItems.Count);
        foreach (ItemInfo iteminfo in iventory.listItems)
        {
            GameObject objButton = Instantiate(prefabButton, rectItemContent.transform);
            GUIItemButton guiItemButton = objButton.GetComponent<GUIItemButton>();
            SpriteRenderer spriteRenderer = iteminfo.object_prefab.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
                guiItemButton.imgItemIcon.color = spriteRenderer.color;
            else
                guiIteminfoPanel.imgItemInfoPanel.color = Color.white;
            guiItemButton.Set(guiIteminfoPanel, iteminfo, dynamic);
            listItemButton.Add(guiItemButton);
        }
        GridLayoutGroup gridLayoutGroupContent = rectItemContent.GetComponent<GridLayoutGroup>();
        Vector2 vButtonSize = gridLayoutGroupContent.cellSize;
        Vector2 vContentSize = rectItemContent.sizeDelta;
        vContentSize.y = vButtonSize.y * listItemButton.Count;
        rectItemContent.sizeDelta = vContentSize;
    }

    public void ReleaseItemButtons()
    {
        foreach (GUIItemButton itemButton in listItemButton)
        {
            Destroy(itemButton.gameObject);
        }
        listItemButton.Clear();
    }

    public void UpdateItemButton(Iventory iventory, Dynamic dynamic)
    {
        ReleaseItemButtons();
        SetItemButtons(iventory, dynamic);

        ItemInfo itemInfo = iventory.GetIventory(0);
        if (itemInfo != null)
            guiIteminfoPanel.SetItemInfoPannel(this, itemInfo, dynamic);
        else
            guiIteminfoPanel.ClearItemInfoPannel();
    }

    public void EventTestSetItemButton()
    {
        ItemInfo itemInfo = GameManager.GetInstance().ItemManager.GetItemInfo(ItemManager.E_ITEM_TYPE.GEM);
        Dynamic dynamic = GameManager.GetInstance().resoponnerPlayer.objTarget.GetComponent<Dynamic>();
        listItemButton[0].Set(guiIteminfoPanel, itemInfo, dynamic);
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
