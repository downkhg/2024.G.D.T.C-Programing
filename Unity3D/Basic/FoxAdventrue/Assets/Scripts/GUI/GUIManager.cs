using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    public List<GameObject> listGUIScenes;
    public enum E_SCENE { PLAY, TITLE, THEEND, GAMEOVER, MAX }
    public E_SCENE curScene;

    public GUIIventory guiIventory;

    GameManager gameManager;

    public GUIStatusBar guiHPBar;

    private void Awake()
    {
        SetMatchRat();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            SetMatchRat();
        }
    }

    public void SetMatchRat()
    {
        float matchRat = (float)Screen.height / (float)Screen.width;
        CanvasScaler canvasScaler = GetComponent<CanvasScaler>();
        canvasScaler.matchWidthOrHeight = matchRat;
        Debug.Log($"SetMatchRat({Screen.width}x{Screen.height}):{matchRat}");
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
            guiIventory.InitializeGUI(iventory, dynamic);
        }
        else
        {
            objPopupLayer.SetActive(false);
            guiIventory.ReleaseGUI();
        }
    }

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

    public void ActiveGUI(bool active)
    {
        foreach(var scene in listGUIScenes)
        {
            scene.SetActive(active);
        }
    }

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
