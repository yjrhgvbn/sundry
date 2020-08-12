using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour {

    private void OnMouseEnter()
    {
        CursorManager._instance.SetNpcTalk();
    }

    private void OnMouseExit()
    {
        CursorManager._instance.SetNormal();
    }

}
