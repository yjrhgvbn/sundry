using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopDrugNPC : NPC {

    //当鼠标停留在这个游戏物体上时，会一直调用这个方向
    public void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(0))
        {
            GetComponent<AudioSource>().Play();
            ShopDrug._instance.TransformState();
        }           
    }
}
