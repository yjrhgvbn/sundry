using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomText : MonoBehaviour {
    public GameObject Ball;
    public GameObject BlockBall;

	// Use this for initialization
	void Start () {
        Debug.Log(Camera.main.orthographicSize);
        Debug.Log(Camera.main.orthographicSize*4/3);
        Vector2 Opos = new Vector2((int)(Camera.main.transform.position.x - Camera.main.orthographicSize * 4 / 3), (int)(Camera.main.transform.position.y - Camera.main.orthographicSize));
        for (int i = 0; i <= (int)Camera.main.orthographicSize * 2 ; i += 2)
        {
            for(int j =0;j<= (int)Camera.main.orthographicSize * 4 / 3 * 2 ;j += 2)
            {
                float value = Random.value;
                Debug.Log(value);
                if (value <= 1f /16f  )
                    Instantiate(Ball, new Vector3(Opos.x + j + Random.Range(-0.25f,0.25f), Opos.y + i + Random.Range(-0.25f, 0.25f), Ball.transform.position.z), Ball.transform.rotation, transform);
                else if(value <= 1f/48f+1f/16f)
                    Instantiate(BlockBall, new Vector3(Opos.x + j + Random.Range(-0.25f, 0.25f), Opos.y + i + Random.Range(-0.25f, 0.25f), Ball.transform.position.z), Ball.transform.rotation, transform);

            }          
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
