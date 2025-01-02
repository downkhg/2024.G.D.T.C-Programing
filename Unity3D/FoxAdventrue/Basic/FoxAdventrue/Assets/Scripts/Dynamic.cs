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

    public Gun gun;

    public Vector3 vDir;

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
    }

    private void OnGUI()
    {
        GUI.Box(new Rect(0, 0, 200, 20), "Score:" + Score);
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
