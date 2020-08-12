using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ContinueDown : MonoBehaviour , IPointerDownHandler ,IPointerUpHandler{

    //按下时每fixedupdate增加的量
    public int onepreNum;

    private bool isDwon = false;
    private GoodsShow_bourse GS;

    private float timer = 0f;
    private float dTime = 0.2f;

    void Start()
    {
        GS = transform.parent.parent.GetComponent<GoodsShow_bourse>();
    }

    public void Update()
    {
        //变速
        if (isDwon)
        {
            timer += Time.deltaTime;
            if(timer>=dTime)
            {
                dTime *= 0.8f;
                GS.ChangeInput(onepreNum);
                timer = 0f;
            }
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        isDwon = true;
        GS.ChangeInput(onepreNum);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDwon = false;
        timer = 0f;
        dTime = 0.2f;
    }
}
