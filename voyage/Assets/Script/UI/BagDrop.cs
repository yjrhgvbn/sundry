using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BagDrop : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{

    public BagSpecific specific;

    private Image image;
    private Vector3 EnterScale;
    private Vector3 normalScale;

    private float Timer = 0f;
    private readonly float DelayTime = 0.4f;
    private bool isHover = false;

    [HideInInspector]
    public int ID;

    void Start()
    {
        image = transform.GetChild(0).GetComponent<Image>();
        normalScale = transform.GetChild(0).GetComponent<RectTransform>().localScale;
        EnterScale = transform.GetChild(0).GetComponent<RectTransform>().localScale * 1.1f;
        ID = int.Parse(gameObject.name.Split('_')[1]);
    }

    void Update()
    {
        //鼠标悬停显示具体信息
        if(!isHover)
        {
            Timer = 0f;
        }
        else if(Timer>=DelayTime)
        {
            //只需调用一次
            isHover = false;
            specific.SetSpecific(Bag._instance.GetFormatNameByNowGridId(ID).Split('_')[0], 100);
            //每四个格子信息栏浮现的位置一样
            int pos = 0;
            if (ID % 4 < 2 && ID < 8)
                pos = 0;
            else if (ID % 4 > 1 && ID < 8)
                pos = 1;
            else if (ID % 4 > 1 && ID > 7)
                pos = 2;
            else if (ID % 4 < 2 && ID > 7)
                pos = 3;
            specific.SetPos(pos);
            specific.Show();
        }  
        else if (isHover)
            Timer += Time.deltaTime;
    }

    public void OnDrop(PointerEventData eventData)
    {       
        GameObject drayGameObject = eventData.pointerDrag;
        if (drayGameObject == null)
            return;
        Bag._instance.SwapeByGridId(drayGameObject.GetComponentInParent<BagDrop>().ID, ID);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {        
        if (transform.GetChild(0).gameObject.activeInHierarchy == false)
        {
            return;
        }
        isHover = true;
        image.GetComponent<RectTransform>().localScale = EnterScale;
    }   

    public void OnPointerExit(PointerEventData eventData)
    {
        if (transform.GetChild(0).gameObject.activeInHierarchy == false)
        {
            return;
        }
        isHover = false;
        specific.Hide();
        image.GetComponent<RectTransform>().localScale = normalScale;
    }
}
