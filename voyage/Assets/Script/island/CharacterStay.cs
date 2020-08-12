using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStay : MonoBehaviour {

    private Vector3 downPos;
    private Vector3 upPos;
    private bool up = true;

	// Use this for initialization
	void Start () {
        SetCube();
	}
	
	// Update is called once per frame
	void Update () {
        UpDwon();
	}

    private void UpDwon()
    {
        if (Vector3.Distance(transform.position, upPos) < 1)
            up = false;
        else if (Vector3.Distance(transform.position, downPos) < 1)
            up = true;
        if (up)
            transform.Translate(Vector3.up * Time.deltaTime * 5, Space.World);
        else
            transform.Translate(-Vector3.up * Time.deltaTime * 5, Space.World);
    }

    public void LeftCube()
    {
        transform.parent.GetComponent<IslandCubeInformation>().IsPlayerStay = false;
    }
    public void SetCube()
    {
        //transform.Translate(transform.parent.position + Vector3.up * 50 - transform.position);
        transform.position = transform.parent.position + Vector3.up * 50;
        downPos = transform.parent.position + Vector3.up * 45;
        upPos = transform.parent.position + Vector3.up * 55;
        up = true;
        transform.parent.GetComponent<IslandCubeInformation>().IsPlayerStay = true;
    }
}
