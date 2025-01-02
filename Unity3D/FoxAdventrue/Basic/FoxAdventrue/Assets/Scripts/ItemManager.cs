using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ItemInfo
{
    public enum E_ITEM_EFFECT { SUPERMODE,RECOVERY,SCORE,BULLET,LEASER,MAX  }
    public string name;
    public string content;
    public E_ITEM_EFFECT item_effect;
    public int value;
    public Sprite imgIcon;
    public string eat_effect;
    public GameObject object_prefab;

    public ItemInfo(string _name, string _content, E_ITEM_EFFECT _item_eff, int _value, string _img_icon_name, string _eat_effect_name, string _object_name) 
    {
        name = _name;
        content = _content;
        item_effect = _item_eff;
        value = _value;
        imgIcon = Resources.Load<Sprite>("Item/" + _img_icon_name);
        eat_effect = _eat_effect_name;
        object_prefab = Resources.Load("Prefabs/Item/" + _object_name) as GameObject;
    }

    public void Use(Dynamic dynamic)
    {
        switch(item_effect)
        {
            case E_ITEM_EFFECT.SUPERMODE:
                dynamic.ActiveSuperMode(value);
                break;
            case E_ITEM_EFFECT.RECOVERY:

                break;
            case E_ITEM_EFFECT.SCORE:
                dynamic.Score += value;
                break;
            case E_ITEM_EFFECT.BULLET:
                dynamic.gun.SetBullet(Gun.E_BULLET_TYPE.BULLET);
                break;
            case E_ITEM_EFFECT.LEASER:
                dynamic.gun.SetBullet(Gun.E_BULLET_TYPE.LESAER);
                break;
        }
    }
}

public class ItemManager : MonoBehaviour
{
    [SerializeField]
    public enum  E_ITEM_TYPE{ SUPERMODE, CHERRY, GEM, BULLER, LEASER } 
    public List<ItemInfo> listItemInfos;


    public ItemInfo GetItemInfo(int idx)
    {
        return listItemInfos[idx];
    }

    public ItemInfo GetItemInfo(E_ITEM_TYPE type)
    {
        return listItemInfos[(int)type];
    }

    public void Initialize()
    {
        listItemInfos = new List<ItemInfo>((int)ItemInfo.E_ITEM_EFFECT.MAX);
        listItemInfos.Add(new ItemInfo("무적", "일정시간동안 무적이 된다", ItemInfo.E_ITEM_EFFECT.SUPERMODE,1, "supermode_icon", "eat_effect", "supermode_item"));
        listItemInfos.Add(new ItemInfo("체리", "일정량 HP를 회복한다", ItemInfo.E_ITEM_EFFECT.RECOVERY,100, "cherry_icon", "eat_effect", "cherry_item"));
        listItemInfos.Add(new ItemInfo("보석", "점수를 획득한다", ItemInfo.E_ITEM_EFFECT.SCORE, 1000, "gem_icon", "eat_effect", "gem_item"));
        listItemInfos.Add(new ItemInfo("총알", "단발씩 발사하는 총알이다.", ItemInfo.E_ITEM_EFFECT.BULLET, 100, "bullet_icon", "eat_effect", "bullet_item"));
        listItemInfos.Add(new ItemInfo("레이져", "일정주기마다 직선상에 대상에게 데미지를 준다", ItemInfo.E_ITEM_EFFECT.LEASER, 1000, "leaser_icon", "eat_effect", "leaser_item"));
    }

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
