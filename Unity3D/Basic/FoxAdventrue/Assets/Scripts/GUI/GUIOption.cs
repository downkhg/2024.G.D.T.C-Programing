using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class GUIOption : MonoBehaviour
{
    public Text textLanguage;
    public TMP_Dropdown dropdownLanguage;
    public List<string> languages;

    public void EventLanguage(int idx)
    {
        Debug.Log($"Change EventLanguage[{idx}]:{languages[idx]}");
        GameManager.GetInstance().EventChangeLanguage(languages[idx]);
    }

    public void InitLanguageOption()
    {
        dropdownLanguage.ClearOptions();
        List<TMP_Dropdown.OptionData> optionDataLists = new List<TMP_Dropdown.OptionData>();

        foreach (string language in languages)
        {
            optionDataLists.Add(new TMP_Dropdown.OptionData(language));
        }
        dropdownLanguage.AddOptions(optionDataLists);
        dropdownLanguage.value = 0;
        dropdownLanguage.onValueChanged.AddListener(EventLanguage);
    }


    public Slider sliderBGSound;

    public void EventBGSound()
    {
        AudioListener.volume = sliderBGSound.value;
    }

    private void Awake()
    {
        InitLanguageOption();
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
