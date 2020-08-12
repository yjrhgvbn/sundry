using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 4;
    public float maxSpeed = 3;

    //private Rigidbody2D rigidbody2d;

    private void Awake()
    {
        //rigidbody2d = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
    }
    private void Update()
    {
        //Vector2 dir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        if (Mathf.Abs(h) > 0 || Mathf.Abs(v) > 0)
        {
            transform.Translate(new Vector3(h,v,0) * Time.deltaTime * speed, Space.World);
        }
        //transform.LookAt(transform.position + new Vector3(dir.x, dir.y, 0));
        //transform.position = transform.position + dir * speed * Time.deltaTime;
        //transform.Translate(transform.up * Time.deltaTime * speed, Space.World);
        /*
        Vector2 velocity = rigidbody2d.velocity;
        velocity = new Vector2(Mathf.Clamp(dir.x * speed * Time.deltaTime + velocity.x, -maxSpeed, maxSpeed), 
            Mathf.Clamp(dir.y * speed * Time.deltaTime + velocity.y, -maxSpeed, maxSpeed));
        velocity = Mathf.Clamp(velocity.magnitude, -maxSpeed, maxSpeed) * velocity.normalized;
        rigidbody2d.velocity = velocity;
        */
    }

    private void MoveControl()
    {
       
    }

}
