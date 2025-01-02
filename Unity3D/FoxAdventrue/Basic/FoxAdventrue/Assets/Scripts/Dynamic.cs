using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dynamic : MonoBehaviour
{
    public float JumpPower;
    public float Speed = 1;
    //public bool isGround; //������ ������ ������ ���� ������쵵 ����ϱ⵵ �ϹǷ�, 
    public bool isJump; //�ʿ��� ������ �������� Ȯ���ϴ°��� �� ��Ȯ�� �ڵ尡 �ȴ�.
    public bool isLodder;
    public int Score;
    static public int Life = 3; //���������� Ȱ���ϸ� ��簴ü�� �����ȴ�. �׷��Ƿ� ���λ����� ��ü�� ���� �����ȴ�. �� ��Ƽ�÷��̰����� �����Ҽ�����.
    //public int Life = 3; //��ü���� ���� 3���� �����Ǿ������Ƿ� ��ü�� ������ �ִ밪���� ��޵ȴ�.

    public Gun gun;

    public Vector3 vDir;

    public bool isSuperMode;

    public IEnumerator ProcessTimmer()
    {
        isSuperMode = true;
        yield return new WaitForSeconds(1);
        isSuperMode = false;
        this.GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void ProcessSuperMode()
    {
        if (isSuperMode)
        {
            SpriteRenderer spriteRenderer = this.GetComponent<SpriteRenderer>();
            Color color = spriteRenderer.color;
            if (color.a == 1)
                color.a = 0;
            else
                color.a = 1;
            spriteRenderer.color = color;
        }
    }

    private void Start()
    {
        StartCoroutine(ProcessTimmer());
    }

    private void OnDestroy()
    {
        //GameObject.Find("GameManager").GetComponent<GameManager>().Life--; //�۵��������� ��ü�� �����ϴµ� ������ �ʿ��ϴ�.
        Debug.Log(gameObject.name+".OnDestroy()");
        GameManager.GetInstance().Life--;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += Vector3.right * Speed * Time.deltaTime;
            vDir = Vector3.right;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left * Speed * Time.deltaTime;
            vDir = Vector3.left;
        }

        if (isLodder)
        {
            if (Input.GetKey(KeyCode.UpArrow))
                transform.position += Vector3.up * Speed * Time.deltaTime;

            if (Input.GetKey(KeyCode.DownArrow))
                transform.position += Vector3.down * Speed * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isJump == false)
            {
                Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
                rigidbody.velocity = Vector2.zero;
                rigidbody.AddForce(Vector3.up * JumpPower);
                isJump = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            gun.Shot(vDir);
        }

        //�̵�: �ð��� ���� ��ġ�� ����Ǵ� ��.
        //transform.position += Vector3.right * Time.deltaTime;

        ProcessSuperMode();
    }

    private void OnGUI()
    {
        GUI.Box(new Rect(0, 0, 200, 20), "Score:" + Score);
        GUI.Box(new Rect(0, 20, 200, 20), "Life:" + Life);
        //GUI.Box(new Rect(20, 0, 200, 20), "Ground:" + isGround);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("OnCollisionEnter2D:"+collision.gameObject.name);
        if (collision.gameObject.tag == "Object")
        {
            Destroy(collision.gameObject);
            Score++;
        }

        isJump = false;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //Debug.Log("OnCollisionExit2D:" + collision.gameObject.name);
        //isGround = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("OnTriggerEnter2D:" + collision.gameObject.name);
        if(collision.gameObject.name == "Lodder")
        {
            Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
            rigidbody.gravityScale = 0;
            rigidbody.velocity = Vector2.zero;
            isLodder = true;
        }

        if (collision.gameObject.name == "house")
        {
            ////���̶�Ű�� ��簴ü���� ���ϴ� ���ӿ�����Ʋ �˻��Ͽ� ���Ӱ����� ��ũ��Ʈ�� ������.
            //GameObject.Find("GameManager").GetComponent<GameManager>().guiManager.SetGUIState(GUIManager.E_SCENE.THEEND);
            //��ü�� �˻������ʰ� �ٷ� ���Ӱ����ڿ� �����Ѵ�.
            GameManager.GetInstance().guiManager.SetGUIState(GUIManager.E_SCENE.THEEND);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Debug.Log("OnTriggerExit2D:" + collision.gameObject.name);
        if (collision.gameObject.name == "Lodder")
        {
            Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
            rigidbody.gravityScale = 1;
            rigidbody.velocity = Vector2.zero;
            isLodder = false;
        }
    }
}
