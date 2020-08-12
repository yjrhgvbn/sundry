using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopWeaponNPC : NPC {

    public void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GetComponent<AudioSource>().Play();
            ShopWeaponUI._instance.TransformState();
        }
    }
}
