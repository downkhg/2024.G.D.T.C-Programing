using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dynamic : MonoBehaviour
{
    public float JumpPower;
    public float Speed = 1;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
            transform.position += Vector3.right * Speed;// * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            transform.position += Vector3.left * Speed;// * Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
            rigidbody.AddForce(Vector3.up * JumpPower);
        }

        //�̵�: �ð��� ���� ��ġ�� ����Ǵ� ��.
        //transform.position += Vector3.right * Time.deltaTime;
    }
}
