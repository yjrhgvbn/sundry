using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Followplayer : MonoBehaviour {

    public float distance = 0;
    public float scrollSpeed = 10f;
    public float rotateSpeed = 3f;

    private Transform player;
    private Vector3 offsetPosition;
    private bool isRotating = false;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag(Tags.player).transform;
        transform.LookAt(player);
        offsetPosition = transform.position - player.position;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = offsetPosition + player.position;
        //视野拉近拉远
        ScrollView();
        //视野旋转
        RotateView();
    }

    void ScrollView()
    {
        //Input.GetAxis("Mouse ScrollWheel");
        distance = offsetPosition.magnitude;
        distance -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
        distance = Mathf.Clamp(distance, 2f, 18f);
        offsetPosition = offsetPosition.normalized * distance;
    }

    void RotateView()
    {
        //Input.GetAxis("Mouse X");   //鼠标在水平方向的滑动
        //Input.GetAxis("Mouse Y");   //鼠标在垂直方向的滑动
        if (Input.GetMouseButtonDown(1))
        {
            isRotating = true;
        }
        if (Input.GetMouseButtonUp(1))
        {
            isRotating = false;
        }

        if(isRotating)
        {
            transform.RotateAround(player.position, player.up, rotateSpeed * Input.GetAxis("Mouse X"));

            Vector3 originalPos = transform.position;
            Quaternion originalRotation = transform.rotation;

            transform.RotateAround(player.position, transform.right, -rotateSpeed * Input.GetAxis("Mouse Y"));
            float x = transform.eulerAngles.x;
            if (x <= 10 || x >= 80)
            {
                transform.position = originalPos;
                transform.rotation = originalRotation;
            }

            offsetPosition = transform.position - player.position;

        }       
    }
}
