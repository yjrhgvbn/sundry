using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressAnyKey : MonoBehaviour {

    private bool isAnyKeyDown = false;//表示任何按钮按下
    private GameObject buttonContainer;
    
	// Use this for initialization
	void Start () {
        buttonContainer = this.transform.parent.Find("ButtonContainer").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        if (isAnyKeyDown == false)
        {
            if(Input.anyKey)
            {
                //show button contanier
                //hide self
                ShowButton();
            }
        }
	}
    void ShowButton()
    {
        buttonContainer.SetActive(true);
        this.gameObject.SetActive(false);
        isAnyKeyDown = true;
    }
}
