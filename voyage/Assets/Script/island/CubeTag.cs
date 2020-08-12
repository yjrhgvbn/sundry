using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CubeTag : MonoBehaviour {

    public GameObject PlayerPerfab;

    private GameObject mouseHoverCube = null;

    GameObject CharTemPos = null;

    void Update () {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && hit.collider.tag == Tags.islandCube)
            {
                mouseHoverCube = hit.collider.gameObject;                
            }
            else
            {                           
                mouseHoverCube = null;
            }

            CharacterMove(hit);
        }

	}

    private void CharacterMove(RaycastHit hit)
    {
        if (!CharTemPos && !mouseHoverCube)
            return;
        if (Input.GetMouseButtonDown(0) && mouseHoverCube)
        {
            CharTemPos = Instantiate(PlayerPerfab, PlayerPerfab.transform.parent);
            CharTemPos.GetComponent<CharacterStay>().enabled = false;
        }
        else if (Input.GetMouseButton(0) && mouseHoverCube && CharTemPos)
        {
            CharTemPos.transform.position = hit.point + new Vector3(0f, 25f, 0f);
        }        
        else if (Input.GetMouseButtonUp(0) && mouseHoverCube && CharTemPos)
        {
            Destroy(CharTemPos);
            PlayerPerfab.GetComponent<CharacterStay>().LeftCube();
            PlayerPerfab.transform.parent = hit.collider.transform;
            PlayerPerfab.GetComponent<CharacterStay>().SetCube();
        }
        else if(Input.GetMouseButtonUp(0) && !mouseHoverCube)
        {
            Destroy(CharTemPos);
        }
    } 
}
