using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dynamic : MonoBehaviour
{
    public float JumpPower;
    public float Speed = 1;
    public bool isGround;
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
            if (isGround)
            {
                Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
                rigidbody.AddForce(Vector3.up * JumpPower);
            }
        }

        //이동: 시간에 따라 위치가 변경되는 것.
        //transform.position += Vector3.right * Time.deltaTime;
    }

    private void OnGUI()
    {
        GUI.Box(new Rect(0, 0, 200, 20), "Score:" + Score);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name != "Ground")
            Destroy(collision.gameObject);
        if (collision.gameObject.name == "crate")
            Score++;
       
        isGround = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //땅을 벗어난 경우(땅에서 떨어질때도 작동하여 의도하지않은 코드를 만든다)
        isGround = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "cherry")
            Score += 10;
    }
}
