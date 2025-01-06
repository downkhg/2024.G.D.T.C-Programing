using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIItemInfoPanel : MonoBehaviour
{
    public Image imgItemInfoPanel;
    public Text textItemInfoPanelName;
    public Text textItemInfoPanelContent;
    public Button buttonItemInfoPanelButton;

    public void ClearItemInfoPannel()
    {
        //Destroy(imgItemInfoPanel.sprite);
        imgItemInfoPanel.color = Color.clear;
        textItemInfoPanelName.text = "";
        textItemInfoPanelContent.text = "";
        buttonItemInfoPanelButton.gameObject.SetActive(false);
        buttonItemInfoPanelButton.onClick.RemoveAllListeners();
    }

    public void SetItemInfoPannel(ItemInfo itemInfo, Dynamic dynamic)
    {
        imgItemInfoPanel.sprite = itemInfo.imgIcon;

        SpriteRenderer spriteRenderer = itemInfo.object_prefab.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            imgItemInfoPanel.color = spriteRenderer.color;
        else
            imgItemInfoPanel.color = Color.white;
        textItemInfoPanelName.text = itemInfo.name;
        textItemInfoPanelContent.text = itemInfo.content;
        buttonItemInfoPanelButton.gameObject.SetActive(true);
        buttonItemInfoPanelButton.onClick.RemoveAllListeners();
        buttonItemInfoPanelButton.onClick.AddListener(
            () =>
            {
                //itemInfo.Use(dynamic);
                Debug.Log("Click Buttion!" + itemInfo.name);
                GameManager.GetInstance().iventoryPlayer.UseIventory(itemInfo, dynamic);
                //UpdateItemButton(GameManager.GetInstance().iventoryPlayer, dynamic);
                Debug.Log("Click Buttion!");
            }
        );
    }

    public void EventTestSetItemInfoPannel()
    {
        ItemInfo itemInfo = GameManager.GetInstance().ItemManager.GetItemInfo(ItemManager.E_ITEM_TYPE.GEM);
        Dynamic dynamic = GameManager.GetInstance().resoponnerPlayer.objTarget.GetComponent<Dynamic>();
        SetItemInfoPannel(itemInfo, dynamic);
    }


    private void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
