using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoad : MonoBehaviour {

    public GameObject magicianPrefab;
    public GameObject swordmanPrefab;

    private void Awake()
    {    
        int selectIndex = PlayerPrefs.GetInt("SelectedCharacterIndex");
        string name = PlayerPrefs.GetString("name");

        GameObject go = null;
        if(selectIndex == 0)
        {
            go = GameObject.Instantiate(magicianPrefab);
        }
        else
        {
            go = GameObject.Instantiate(swordmanPrefab);
        }

        go.GetComponent<PlayerStates>().name = name;
    }
}
