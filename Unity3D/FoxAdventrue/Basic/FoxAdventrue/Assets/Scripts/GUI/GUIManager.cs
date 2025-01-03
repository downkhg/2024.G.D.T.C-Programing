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

    public void SetItemInfoPannel(ItemInfo itemInfo, Dynamic dynamic)
    {
        imgItemInfoPanel.sprite = itemInfo.imgIcon;
        textItemInfoPanelName.text = itemInfo.name;
        textItemInfoPanelContent.text = itemInfo.content;
        buttonItemInfoPanelButton.onClick.AddListener(() => itemInfo.Use(dynamic));
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
        foreach (ItemInfo iteminfo in iventory.listItems)
        {
            GameObject objButton = Instantiate(prefabButton, rectItemContent.transform);
            GUIItemButton guiItemButton = objButton.GetComponent<GUIItemButton>();
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
        GameObject prefabButton = Resources.Load("Prefabs/GUI/ItemButton") as GameObject;
        foreach (GUIItemButton itemButton in listItemButton)
        {
            Destroy(itemButton.gameObject);
        }
        listItemButton.Clear();
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
