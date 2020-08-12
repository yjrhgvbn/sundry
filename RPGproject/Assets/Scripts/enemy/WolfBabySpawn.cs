using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfBabySpawn : MonoBehaviour {

    public int maxNum = 5;
    private int currentNum = 0;
    public float time = 3f;
    private float timer = 0f;
    public GameObject perfab;

    private void Update()
    {
        if(currentNum<maxNum)
        {
            timer += Time.deltaTime;
            if (timer > time)
            {
                Vector3 pos = transform.position;
                pos.x += Random.Range(-5, 5);
                pos.z += Random.Range(-5, 5);
                GameObject go = GameObject.Instantiate(perfab, pos, Quaternion.identity);
                go.GetComponent<WolfBaby>().spawn = this;
                timer = 0;
                currentNum++;
            }
        }
    }

    public void MinusNumber()
    {
        this.currentNum--;
    }

}
