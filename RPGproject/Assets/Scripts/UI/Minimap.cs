using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour {

    private Camera minimapCamera;

    private void Awake()
    {
        minimapCamera = GameObject.FindWithTag(Tags.minimap).GetComponent<Camera>();
    }

    public void OnZoomInClick()//放大
    {
        minimapCamera.orthographicSize--;
    }

    public void OnZoomOutClick()//缩小
    {
        minimapCamera.orthographicSize++;
    }
}
