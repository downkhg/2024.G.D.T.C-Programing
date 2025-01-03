using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIItemButton : MonoBehaviour
{
    public Text textItemName;
    public Image imgItemIcon;
    [SerializeField]
    Button btnItemButton;

    public void Set(GUIManager guiManager, ItemInfo itemInfo, Dynamic dynamic)
    {
        textItemName.text = itemInfo.name;
        imgItemIcon.sprite = itemInfo.imgIcon;
        btnItemButton = this.gameObject.GetComponent<Button>();
        btnItemButton.onClick.AddListener(() => guiManager.SetItemInfoPannel(itemInfo, dynamic));
    }
}
