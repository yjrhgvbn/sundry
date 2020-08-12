using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class creater : MonoBehaviour {

    public GameObject Ball;
    public GameObject BlockBall;
    private MainBall mainBall;
    float left, right, top, botton;
    float checkLeft, checkRight, checkTop, checkBotton;
    float nL, nR, nT, nB, sL, sR, sT, sB;
    float h1, h2, w1, w2;
    private Vector2 prePos;
    private bool isOutCheckPos = false;
    private List<GameObject> ballList = new List<GameObject>();

    // Use this for initialization
    private void Awake()
    {        
        mainBall = GameObject.FindWithTag("Ball").GetComponent<MainBall>();
    }

    void Start () {        
        h1 = 24f;
        h2 = 16f;
        w1 = 32f;
        w2 = 24f;
        prePos = mainBall.transform.position;
        left = prePos.x - w1;
        right = prePos.x + w1;
        top = prePos.y + h1;
        botton = prePos.y - h1;
        checkLeft = prePos.x - w2;
        checkRight = prePos.x + w2;
        checkTop = prePos.y + h2;
        checkBotton = prePos.y - h2;
        Init();
    }
	
	// Update is called once per frame
	void Update () {
        if (mainBall.isRevolve == true)
        {
        }
        else
        {
            OnMove();
            if(isOutCheckPos)
            {
                SetLocation();
                OnMoveCreate();
                DestoryBall();
                isOutCheckPos = false;
            }
        }
	}

    void SetLocation()
    {
        checkLeft = left + w1 - w2;
        checkRight = right - w1 + w2;
        checkTop = top - h1 + h2;
        checkBotton = botton + h1 - h2;
    }

    void OnMove()
    {
        float x = mainBall.transform.position.x;
        float y = mainBall.transform.position.y;
        if (x > checkRight)
        {
            isOutCheckPos = true;
            nL = left + w2;nR = right + w2; sL = right;sR = nR;left = nL;right = nR;
            if (y >= prePos.y)
            {
                nT = y + h1;nB = top;sT = nB;sB = y - h1;botton = sB;top = nT;
            }
            else
            {
                nT = botton;nB = y - h1;sT = y + h1;sB = nT;botton = nB;top = sT;
            }
            prePos.x = checkRight;
            prePos.y = y;
        }
        else if (x < checkLeft)
        {
            isOutCheckPos = true;
            nL = left - w2; nR = right - w2; sL = nL; sR = left; left = nL; right = nR;
            if (y >= prePos.y)
            {
                nT = y + h1; nB = top; sT = nB; sB = y - h1; botton = sB; top = nT;
            }
            else
            {
                nT = botton; nB = y - h1; sT = y + h1; sB = nT; botton = nB; top = sT;
            }
            prePos.x = checkLeft;
            prePos.y = y;
        }
        else if (y > checkTop)
        {
            isOutCheckPos = true;
            nT = top + h2; nB = top; sT = nB; sB = botton + h2; botton = sB; top = nT;
            if (x >= prePos.x)
            {
                nL = x - w1; nR = x + w1; sL = right; sR = nR; left = nL; right = nR;
            }
            else
            {
                nL = x - w1; nR = x + w1; sL = nL; sR = left; left = nL; right = nR;
            }

            prePos.x = x;
            prePos.y = checkTop;
        }
        else if (y < checkBotton)
        {
            isOutCheckPos = true;
            nT = botton; nB = botton - h2; sT = top - h2; sB = nT; botton = nB; top = sT;
            if (x >= prePos.x)
            {
                nL = x - w1; nR = x + w1; sL = right; sR = nR; left = nL; right = nR;
            }
            else
            {
                nL = x - w1; nR = x + w1; sL = nL; sR = left; left = nL; right = nR;
            }
            prePos.x = x;
            prePos.y = checkBotton;
        }       
    }

    void OnMoveCreate()
    {
        //障碍物概率h/1000 * 1/48 (>1/48 <1/24)
        for (int i = (int)nB + 1; i <= nT; i++)
        {
            for (int j = (int)nL + 1; j <= nR; j++)
            {
                float value = Random.value;
                float blockRate = (1f / 48f) * (Mathf.Clamp(i, 0f, 1000f) / 1000f + 1f);                
                if (value <= 1f / 32f)
                {
                    ballList.Add(Instantiate(Ball, new Vector3(j + Random.Range(-0.25f, 0.25f), i + Random.Range(-0.25f, 0.25f), Ball.transform.position.z), Quaternion.identity, transform));
                }
                else if (value <= blockRate + 1f / 32f)
                {
                    ballList.Add(Instantiate(BlockBall, new Vector3(j + Random.Range(-0.25f, 0.25f), i + Random.Range(-0.25f, 0.25f), Ball.transform.position.z), Quaternion.identity, transform));
                }                             
            }
        }
        for (int i = (int)sB + 1; i <= sT; i++)
        {
            for (int j = (int)sL + 1; j <= sR; j++)
            {
                float value = Random.value;
                float blockRate = (1f / 48f) * (Mathf.Clamp(i, 0f, 1000f) / 1000f + 1f);                
                if (value <= 1f / 32f)
                {
                    ballList.Add(Instantiate(Ball, new Vector3(j + Random.Range(-0.25f, 0.25f), i + Random.Range(-0.25f, 0.25f), Ball.transform.position.z), Quaternion.identity, transform));
                }
                else if (value <= blockRate + 1f / 32f)
                {
                    ballList.Add(Instantiate(BlockBall, new Vector3(j + Random.Range(-0.25f, 0.25f), i + Random.Range(-0.25f, 0.25f), Ball.transform.position.z), Quaternion.identity, transform));
                }                
            }
        }
        
    }

    void Init()
    {
        for (int i = (int)botton; i < top; i++)
        {
            for (int j = (int)left; j < right; j++)
            {
                float value = Random.value;
                float blockRate = (1f / 48f) * (Mathf.Clamp(i, 0f, 1000f) / 1000f + 1f);                
                if (value <= 1f / 32f)
                {
                    ballList.Add(Instantiate(Ball, new Vector3(j + Random.Range(-0.25f, 0.25f), i + Random.Range(-0.25f, 0.25f), Ball.transform.position.z), Quaternion.identity, transform));
                }
                else if (value <= blockRate + 1f / 32f)
                {
                    ballList.Add(Instantiate(BlockBall, new Vector3(j + Random.Range(-0.25f, 0.25f), i + Random.Range(-0.25f, 0.25f), Ball.transform.position.z), Quaternion.identity, transform));
                }                
            }
        }
    }

    void DestoryBall()
    {
        for (int i = 0; i < ballList.Count; i++)
        {
            if (ballList[i].transform.position.x > right || ballList[i].transform.position.x < left || ballList[i].transform.position.y > top || ballList[i].transform.position.y < botton)
            {
                Destroy(ballList[i]);
                ballList.Remove(ballList[i]);
            }
        }
    }
}
