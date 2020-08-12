using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBall : MonoBehaviour
{
    public float Speedx;
    public float MaxSpeed;
    public float AngularVelocity = 3;

    private float NowSpeed;
    private float Distance;
    private GameObject ConnectObject;
    [HideInInspector]
    public bool isRevolve;
    private GameObject[] AbleConnectObjects;   
    private bool BallExit = false;   


    private Rigidbody2D R2D;
    private SpringJoint2D SJ2D;
    
    void Start()
    {
        R2D = GetComponent<Rigidbody2D>();
        SJ2D = GetComponent<SpringJoint2D>();
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            //get mouse position and find the nearest connect body
            Vector3 mouseP = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
            AbleConnectObjects = GameObject.FindGameObjectsWithTag("ConnectObject");
            if (AbleConnectObjects.Length > 0)
            {
                ConnectObject = AbleConnectObjects[0];
                for (int i = 1; i < AbleConnectObjects.Length; i++)
                {
                    if (Vector2.Distance(AbleConnectObjects[i].transform.position, mouseP) < Vector2.Distance(ConnectObject.transform.position, mouseP))
                    {
                        ConnectObject = AbleConnectObjects[i];
                    }
                }
                isRevolve = true;
                BallExit = false;

                //make the ball revolve around the ConnectObject and keep the speed constant
                SJ2D.connectedBody = ConnectObject.GetComponent<Rigidbody2D>();
                SJ2D.connectedAnchor = Vector2.zero;
                GetComponent<DistanceJoint2D>().connectedBody = ConnectObject.GetComponent<Rigidbody2D>();
                GetComponent<DistanceJoint2D>().connectedAnchor = Vector2.zero;            
                GetComponent<DistanceJoint2D>().enabled = true;
                Distance = Vector2.Distance(transform.position, ConnectObject.transform.position);
                GetComponent<DistanceJoint2D>().distance = Distance;
                Vector2 k1 = (transform.position - ConnectObject.transform.position).normalized;
                Vector2 k2 = new Vector2(-k1.y, k1.x);
                Vector2 sk = R2D.velocity.normalized;
                if (ConnectObject.transform.position.x>transform.position.x)
                {
                    NowSpeed = Mathf.Clamp(R2D.velocity.sqrMagnitude, 0, MaxSpeed);
                }
                else
                {
                    NowSpeed = -Mathf.Clamp(R2D.velocity.sqrMagnitude, 0, MaxSpeed);
                }
                R2D.velocity = k2 * NowSpeed;
            }
        }

        if (Input.GetMouseButton(0))
        {

            if (isRevolve)
            {
                //keep the speed constant        
                Vector2 k1 = (transform.position - ConnectObject.transform.position).normalized;
                Vector2 k2 = new Vector2(-k1.y, k1.x);
                R2D.velocity = k2 * NowSpeed;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (isRevolve)
            {
                R2D.velocity = Vector2.zero;
                GetComponent<DistanceJoint2D>().enabled = false;
                SJ2D.enabled = true;
                isRevolve = false;
            }
        }

        if (isRevolve)
        {            
            OnRevolve();        
        }
        else
        {
            //control the speed
            if (R2D.velocity.sqrMagnitude > MaxSpeed)
                R2D.velocity = Mathf.Lerp(MaxSpeed, R2D.velocity.magnitude, Time.deltaTime) * R2D.velocity.normalized;
          
                Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 5, 2 * Time.deltaTime);
            
            if (BallExit)
            {
                float posX = transform.position.x;
                float posY = transform.position.y;
                Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(posX, posY, Camera.main.transform.position.z), 3 * Time.deltaTime);
                //Camera.main.transform.position = new Vector3(posX, posY, Camera.main.transform.position.z);               
            }         
            if (SJ2D.enabled == false)
            {

            }


        }

    }

    //控制旋转时的速度
    void OnRevolve()
    {
        if (Distance > 1f && R2D.velocity.sqrMagnitude > MaxSpeed)
        {
            R2D.velocity = R2D.velocity.normalized * AngularVelocity * Mathf.Clamp(Distance, 0, 3);
            MaxSpeed = AngularVelocity * Mathf.Clamp(Distance, 0, 2);
        }
        else if (R2D.velocity.sqrMagnitude > MaxSpeed)
            R2D.velocity = MaxSpeed * R2D.velocity.normalized;

        if (Vector2.Distance(transform.position, ConnectObject.transform.position) > 4f)
        {
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, (Vector2.Distance(transform.position, ConnectObject.transform.position) - 4) * 2f + 5, 2 * Time.deltaTime);
        }
        //camera following when revolve            
        float posX = ConnectObject.transform.position.x;
        float posY = ConnectObject.transform.position.y;
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(posX, posY, Camera.main.transform.position.z), 3 * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == ConnectObject)
        {
            SJ2D.enabled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {

        if (other.gameObject == ConnectObject)
        {
            BallExit = true;
        }
    }
}
