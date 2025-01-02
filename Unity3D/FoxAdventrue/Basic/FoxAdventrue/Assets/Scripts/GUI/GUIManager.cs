using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void Initialize()
    {
        gameObject.SetActive(true);
        SetGUIState(curScene);   
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
