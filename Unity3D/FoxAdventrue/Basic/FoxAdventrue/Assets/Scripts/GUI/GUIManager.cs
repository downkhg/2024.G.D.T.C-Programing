using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    public List<GameObject> listGUIScenes;
    public enum E_SCENE { PLAY, TITLE, THEEND, GAMEOVER, MAX }
    public E_SCENE curScene;

    public GUIStatusBar guiHPBar;

    private void Awake()
    {
        ClearItemInfoPannel();
    }

    public void UpdatePlayerStatusBar(Player player)
    {
        if(guiHPBar && player)
        {
            guiHPBar.SetBar(player.Status.hp, player.MaxHP);
        }
    }

    public GameObject objPopupLayer;

    public void SetPopup(Iventory iventory, Dynamic dynamic)
    {
        if (objPopupLayer.activeSelf == false)
        {
            objPopupLayer.SetActive(true);
            SetItemButtons(iventory,dynamic);
            SetItemInfoPannel(iventory.GetIventory(0), dynamic);
        }
        else
        {
            objPopupLayer.SetActive(false);
            ReleaseItemButtons();
        }
    }

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
                Debug.Log("Click Buttion!"+ itemInfo.name);
                GameManager.GetInstance().iventoryPlayer.UseIventory(itemInfo, dynamic);
                UpdateItemButton(GameManager.GetInstance().iventoryPlayer , dynamic);
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

    public List<GUIItemButton> listItemButton;
    public RectTransform rectItemContent;

    public void SetItemButtons(Iventory iventory, Dynamic dynamic)
    {
        GameObject prefabButton = Resources.Load("Prefabs/GUI/ItemButton") as GameObject;
        Debug.LogFormat("SetItemButtons[{0}]",iventory.listItems.Count);
        foreach (ItemInfo iteminfo in iventory.listItems)
        {
            GameObject objButton = Instantiate(prefabButton, rectItemContent.transform);
            GUIItemButton guiItemButton = objButton.GetComponent<GUIItemButton>();
            SpriteRenderer spriteRenderer = iteminfo.object_prefab.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
                guiItemButton.imgItemIcon.color = spriteRenderer.color;
            else
                imgItemInfoPanel.color = Color.white;
            guiItemButton.Set(this, iteminfo, dynamic);
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
            SetItemInfoPannel(itemInfo, dynamic);
        else
            ClearItemInfoPannel();
    }

    public void EventTestSetItemButton()
    {
        ItemInfo itemInfo = GameManager.GetInstance().ItemManager.GetItemInfo(ItemManager.E_ITEM_TYPE.GEM);
        Dynamic dynamic = GameManager.GetInstance().resoponnerPlayer.objTarget.GetComponent<Dynamic>();
        listItemButton[0].Set(this, itemInfo, dynamic);
    }

    GameManager gameManager;

    public void Initialize(GameManager gameManager)
    {
        gameObject.SetActive(true);
        SetGUIState(curScene);
        this.gameManager = gameManager;
    }

    public void ShowGUIState(E_SCENE scene)
    {
        for (E_SCENE idx = E_SCENE.PLAY; idx < E_SCENE.MAX; idx++)
        {
            if (idx == scene)
                listGUIScenes[(int)idx].SetActive(true);
            else
                listGUIScenes[(int)idx].SetActive(false);
        }
    }

    //private void Update()
    //{
    //    //if(Input.GetKeyDown(KeyCode.I))
    //    //{
    //    //    //EventTestSetItemInfoPannel();
    //    //    EventTestSetItemButton();
    //    //}
    //}

    public void SetGUIState(E_SCENE scene)
    {
        switch (scene)
        {
            case E_SCENE.PLAY:
                Time.timeScale = 1;
                break;
            case E_SCENE.TITLE:
                Time.timeScale = 0;
                break;
            case E_SCENE.THEEND:
                Time.timeScale = 0;
                break;
            case E_SCENE.GAMEOVER:
                Time.timeScale = 0;
                GameManager.GetInstance().EventReset();
                break;
        }
        ShowGUIState(scene);
        curScene = scene;
    }

    public void UpdateGUIState()
    {
        switch (curScene)
        {
            case E_SCENE.PLAY:
                if(Input.GetKeyDown(KeyCode.I))
                {
                    SetPopup(gameManager.iventoryPlayer, gameManager.resoponnerPlayer.objTarget.GetComponent<Dynamic>());
                }
                break;
            case E_SCENE.TITLE:
                break;
            case E_SCENE.THEEND:
                break;
            case E_SCENE.GAMEOVER:
                break;
        }
    }

    public void EventSenceChange(int idx)
    {
        SetGUIState((E_SCENE)idx);
    }
}
