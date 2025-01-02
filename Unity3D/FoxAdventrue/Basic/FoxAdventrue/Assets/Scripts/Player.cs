using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Status
{
    public int hp;
    public int mp;
    public int str;
}

public class Player : MonoBehaviour
{
    public Status Status;
    public int MaxHP;
    public int MaxMP;

    public GUIStatusBar guiHPBar;

    public void UpdateStatusBar()
    {
        if(guiHPBar)
            guiHPBar.SetBar(Status.hp, MaxHP);
    }

    private void Awake()
    {
        MaxHP = Status.hp;
        MaxMP = Status.hp;
    }

    public void Attack(Player player)
    {
        player.Status.hp = player.Status.hp - this.Status.str;
    }

    public bool Death()
    {
        return (this.Status.hp <= 0);
    }
    public int idx;
    private void OnGUI()
    {
        GUI.Box(new Rect(idx * 100, 0, 100, 20), 
            string.Format("{0}/{1}\n{2}/{3}\n{2}\n", 
            Status.hp, MaxHP, Status.mp, MaxMP,Status.str));
    }

    private void Update()
    {
        UpdateStatusBar();

        if (Death())
            Destroy(this.gameObject);
    }
}
