using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManger : MonoBehaviour {
    
    public MiniCamera miniCamera;

    private void Awake()
    {
        miniCamera = GameObject.Find("/MiniMapCamera").GetComponent<MiniCamera>();
    }

    public void Start()
    {
        test();
    }

    private void test()
    {
        miniCamera.AddTaskTarget(new Vector3(76f, 4.6f, 0), Color.yellow);
    }

}
