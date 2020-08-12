using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandCubeInformation : MonoBehaviour {
    
    public GameObject exploreCanvas;
    public float[]rate;
    public IslandCubeExploreInfomation.ExploreType[]type;

    [HideInInspector]
    public bool IsPlayerStay = false;

    private void Start()
    {        
    }

    public void Act()
    {
        exploreCanvas.GetComponent<ExploreChoiceInformation>().NormalExploreEvent(IslandCubeType.Forest);
    }
}
