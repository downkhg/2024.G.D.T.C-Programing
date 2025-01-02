using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dynamic : MonoBehaviour
{
    public float JumpPower;
    public float Speed = 1;
    //public bool isGround; //게임은 현실의 물리와 같지 않은경우도 허용하기도 하므로, 
    public bool isJump; //필요한 조건인 점프인지 확인하는것이 더 정확한 코드가 된다.
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

        //이동: 시간에 따라 위치가 변경되는 것.
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
