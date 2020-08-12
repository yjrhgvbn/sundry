using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandMessgae : MonoBehaviour {

    public Vector3 Center;
    public string IslandName;
    public int IslandId;

    public void SetQuadActive()
    {
        transform.Find("Quad").gameObject.SetActive(true);
    }

    public void SetQuadfalse()
    {
        transform.Find("Quad").gameObject.SetActive(false);
    }

}
