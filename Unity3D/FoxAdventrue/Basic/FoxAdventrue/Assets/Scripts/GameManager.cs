using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Responner resoponnerPlayer;
    public List<Responner> responnerMonsters;

    public List<GameObject> listLifes;
    //public static int Life = 3; //게임관리자에 static을쓰더라도 플레이추가시마다 라이프가 추가되어야한다.
    public int Life = 3; //싱글톤을 이용하면 일반멤버도 다른객체에서 바로 접근가능하도록 만들수있다.

    public void ProcessLife()
    {
        for (int i = 0; i < listLifes.Count; i++)
        {
            if (i < Life) listLifes[i].SetActive(true);
            else listLifes[i].SetActive(false);
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

    public GUIManager guiManager;
    public ItemManager ItemManager;

    public void Awake()
    {
        instance = this;
        guiManager.Initialize();
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ProcessSetPatrol();
        ProcessLife();
        guiManager.UpdateGUIState();
    }
}
