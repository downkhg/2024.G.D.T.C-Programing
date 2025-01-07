using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LanguageDTO
{
    public int Id;
    public string Secene;
    public string Type;
    public string UI;
    public string Text;
    public string Image;
    public string Color;

    public LanguageDTO(string row)
    {
        string[] columns = row.Split(',');
        if (columns.Length >= 6)
        {
            Id = int.Parse(columns[0]);
            Type = columns[1];
            Secene = columns[2];
            UI = columns[3];
            Text = columns[4];
            Image = columns[5];
            Color = columns[6];
        }
        else
        {
            Debug.LogError($"LanguageDTO Err[{columns.Length}]:{row}");
        }
    }

    public LanguageDTO(int id,string secene, string type, string ui, string text, string img, string color)
    {
        Id = id;
        Secene = secene;
        Type = type;
        UI = ui;
        Text = text;
        Image = img;
        Color = color;
    }
}

public class LanguageDAO
{
    public List<LanguageDTO> languages = new List<LanguageDTO>();

    public void AccessDataBase(string language)
    {
        TextAsset textAsset = Resources.Load<TextAsset>($"Data/LanguagePack_{language}");

        string fileText = textAsset.text;
        string[] rows = fileText.Split('\n');

        for (int i = 1; i < rows.Length; i++)
        {
            string[] columns = rows[i].Split(',');
            if (columns.Length >= 7)
                languages.Add(new LanguageDTO(int.Parse(columns[0]), columns[1], columns[2], columns[3], columns[4], columns[5], columns[6]));
        }
    }

    public void RemvoeDataBase()
    {
        languages.Clear();
    }

    public LanguageDTO GetDTO(int idx)
    {
        try { return languages[idx]; } catch { return null; }
    }
}

public class GameManager : MonoBehaviour
{
    public CameraTracker cameraTracker;
    public Responner resoponnerPlayer;
    public Iventory iventoryPlayer;
    public List<Responner> responnerMonsters;

    public List<GameObject> listLifes;
    //public static int Life = 3; //게임관리자에 static을쓰더라도 플레이추가시마다 라이프가 추가되어야한다.
    public int Life = 3; //싱글톤을 이용하면 일반멤버도 다른객체에서 바로 접근가능하도록 만들수있다.

    public void ProcessLife()
    {
        for (int i = 0; i < listLifes.Count; i++)
        {
            if (i < Life) 
                listLifes[i].SetActive(true);
            else 
                listLifes[i].SetActive(false);
        }

        if(Life <= 0)
        {
            guiManager.SetGUIState(GUIManager.E_SCENE.GAMEOVER);
        }
    }

    public void EventExit()
    {
        Application.Quit();
    }

    public void EventScenceChange(string name)
    {
        DontDestroyOnLoad(this.gameObject);
        SceneManager.LoadScene(name);
    }

    public void EventReset()
    {
        for(int i = 0; i < responnerMonsters.Count; i++)
        {
            responnerMonsters[i].objTarget.transform.position = responnerMonsters[i].transform.position;
        }
        Life = 3;
    }

    //싱글톤: 모든 클래스에서 게임관리자인스턴스에 접근하도록 만드는 패턴.//전역변수,정적멤버,객체의 참조
    public static GameManager GetInstance() { return instance;  }
    public static GameManager instance = null;

    public LanguageDAO languageDAO;
    public string curLanguage;

    public bool EventChangeLanguage(string language)
    {
        if (curLanguage == language) return false;
        languageDAO.AccessDataBase(language);
        guiManager.ActiveGUI(true);
        foreach (LanguageDTO lang in languageDAO.languages)
        {
            try
            {
                Debug.Log($"ChangLanguage:{lang.Secene}/{lang.UI}");
                GameObject objSecene = GameObject.Find(lang.Secene);
                Transform trUI = objSecene.transform.Find(lang.UI);
                Text textUI = trUI.gameObject.GetComponent<Text>();
                Image imgUI = trUI.gameObject.GetComponent<Image>();

                switch(lang.Type)
                {
                    case "Button":
                        textUI = trUI.transform.Find("Text").GetComponent<Text>();
                        break;
                }

                switch (lang.Color)
                {
                    case "red":
                        if (textUI) textUI.color = Color.red;
                        if (imgUI) imgUI.color = Color.red;
                        break;
                    case "white":
                        if (textUI) textUI.color = Color.white;
                        if (imgUI) imgUI.color = Color.white;
                        break;
                    case "black":
                        if (textUI) textUI.color = Color.black;
                        if (imgUI) imgUI.color = Color.black;
                        break;
                }
                if (textUI) textUI.text = lang.Text;
                if (imgUI) imgUI.sprite = Resources.Load<Sprite>($"Data/{language}/image/{lang.Image}");
            }
            catch (Exception err)
            {
                Debug.LogException(err);
            }
        }
        guiManager.ActiveGUI(false);
        guiManager.SetGUIState(guiManager.curScene);
        languageDAO.RemvoeDataBase();
        curLanguage = language;
        return true;
    }

    public GUIManager guiManager;
    public ItemManager ItemManager;

    public void Awake()
    {
        instance = this;
        Initalize(); //만약이전에 객체들을 재활용이 불가능하다면, 다음과 같이 게임관리자 작업시 모두 초기화하여 사용해야한다.
        guiManager.Initialize(this);
        languageDAO = new LanguageDAO();
    }

    public void Initalize()
    {
        if (resoponnerPlayer == null)
        {
            GameObject objPlayerRespon = Resources.Load("Prefabs/PlayerResponner") as GameObject;
            resoponnerPlayer = objPlayerRespon.GetComponent<Responner>();
        }
    }

    public enum E_MONSTER{ OPPOSUM, EAGLE, FROG }

    public Responner GetRespnner(E_MONSTER monsterIdx)
    {
        return responnerMonsters[(int)monsterIdx];
    }

    public Transform trPatrolPoint;

    public void ProcessSetPatrol()
    {
        Responner responEagle = GetRespnner(E_MONSTER.EAGLE);

        if(responEagle && responEagle.objTarget)
        {
            Eagle eagle = responEagle.objTarget.GetComponent<Eagle>();

            if (eagle && eagle.trPatrolPoint == null)
            {
                eagle.trPatrolPoint = trPatrolPoint;
                eagle.trResponPoint = responEagle.transform;
                eagle.SetState(Eagle.E_AI_STATUS.RETURN);
                //Debug.Log("SetPatrol");
            }
        }

        Responner responFrog = GetRespnner(E_MONSTER.FROG);

        if (responFrog && responFrog.objTarget)
        {
            Frog frog = responFrog.objTarget.GetComponent<Frog>();

            if (frog && frog.trPatrolPoint == null)
            {
                frog.trPatrolPoint = trPatrolPoint;
                frog.trResponPoint = responEagle.transform;
                frog.SetState(Frog.E_AI_STATUS.RETURN);
                //Debug.Log("SetPatrol");
            }
        }
    }

    public void UpdateCameraTracker()
    {
        if (resoponnerPlayer.objTarget)
        {
            cameraTracker.trTargetPoint = resoponnerPlayer.objTarget.transform;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCameraTracker();
        ProcessSetPatrol();
        ProcessLife();
        guiManager.UpdateGUIState();
        guiManager.UpdatePlayerStatusBar(resoponnerPlayer.objTarget.GetComponent<Player>());


        if (Input.GetKeyDown(KeyCode.L))
        {
            EventChangeLanguage("EN");
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            EventScenceChange("Game");
        }
    }
}
