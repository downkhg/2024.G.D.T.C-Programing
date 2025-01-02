using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dynamic : MonoBehaviour
{
    public float JumpPower;
    public float Speed = 1;
    //public bool isGround; //������ ������ ������ ���� ������쵵 ����ϱ⵵ �ϹǷ�, 
    public bool isJump; //�ʿ��� ������ �������� Ȯ���ϴ°��� �� ��Ȯ�� �ڵ尡 �ȴ�.
    public int Score;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
            transform.position += Vector3.right * Speed * Time.deltaTime;

        if (Input.GetKey(KeyCode.LeftArrow))
            transform.position += Vector3.left * Speed * Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (isJump == false)
            {
                Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
                rigidbody.velocity = Vector2.zero;
                rigidbody.AddForce(Vector3.up * JumpPower);
                isJump = true;
            }
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
        Debug.Log("OnCollisionEnter2D:"+collision.gameObject.name);
        if (collision.gameObject.tag == "Object")
        {
            Destroy(collision.gameObject);
            Score++;
        }
       
        isJump = false;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("OnCollisionExit2D:" + collision.gameObject.name);
        //isGround = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("OnTriggerEnter2D:" + collision.gameObject.name);
      
    }
}
