using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dynamic : MonoBehaviour
{
    public float JumpPower;
    public float Speed = 1;
    //public bool isGround; //게임은 현실의 물리와 같지 않은경우도 허용하기도 하므로, 
    public bool isJump; //필요한 조건인 점프인지 확인하는것이 더 정확한 코드가 된다.
    public bool isLodder;
    public int Score;
    static public int Life = 3; //정적변수를 활용하면 모든객체가 공유된다. 그러므로 새로생성된 객체도 값이 유지된다. 단 멀티플레이게임을 제작할수없다.
    //public int Life = 3; //객체마다 값이 3으로 설정되어있으므로 객체가 생성시 최대값으로 취급된다.

    public Gun gun;

    public Vector3 vDir;

    private void OnDestroy()
    {
        //GameObject.Find("GameManager").GetComponent<GameManager>().Life--; //작동은하지만 객체에 접근하는데 연산이 필요하다.
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

        //이동: 시간에 따라 위치가 변경되는 것.
        //transform.position += Vector3.right * Time.deltaTime;
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
        //Debug.Log("OnTriggerEnter2D:" + collision.gameObject.name);
        if(collision.gameObject.name == "Lodder")
        {
            Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
            rigidbody.gravityScale = 0;
            rigidbody.velocity = Vector2.zero;
            isLodder = true;
        }

        if (collision.gameObject.name == "house")
        {
            
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
