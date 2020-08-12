using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {

    public bool b = true;
    public bool a = false;
    // Use this for initialization
    void Start () {
        GetComponent<Rigidbody2D>().gravityScale = 0f;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            b = true;
            a = true;
        }
        if (a)
        {
            if (Input.GetMouseButtonDown(1))
            {
                Debug.Log(b);
            }
            else if (Input.GetMouseButtonDown(0))
            {
                Debug.Log(a);
            }
        }
    }
}
