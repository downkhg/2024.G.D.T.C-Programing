using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIStatusBar : MonoBehaviour
{
    public RectTransform rectBarBG;
    public RectTransform rectBar;
    public Text textLabel;

    public void SetBar(float cur, float max)
    {
        float rat = cur / max;
        Vector2 vSize = rectBar.sizeDelta;
        vSize.x = rectBarBG.sizeDelta.x * rat;
        rectBar.sizeDelta = vSize;
    }
}
