using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollRect : MonoBehaviour
{
    public Text priceText;
    public GameObject goodsShowGo;

    private GoodsShow_bourse[] goodsShow_Bourses;
    private int totalPrice = 0;
    private GameObject contant;

    private void Awake()
    {
        contant = transform.GetChild(0).GetChild(0).gameObject;
        SetContainSpecific();
    }

    private void Start()
    {

        InitialGoodsShow();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitialGoodsShow()
    {
        for(int i = 0;i<3;i++)
        {
            GameObject go = Instantiate(goodsShowGo, contant.transform);
            string ID = "1000" + Random.Range(0, 5).ToString();
            go.GetComponent<GoodsShow_bourse>().SetItem(Article._instance.GetArticleByID(ID), Random.Range(0, 100), Random.Range(0, 1000));
        }
        SetContainSpecific();
    }

    void SetContainSpecific()
    {
        goodsShow_Bourses = GetComponentsInChildren<GoodsShow_bourse>();
    }

    public void Purchase()
    {
        //TODO
        //减少金币
        foreach (var goodsShow_Bourse in goodsShow_Bourses)
        {
            goodsShow_Bourse.Purchase();
        }
        Refresh();
    }

    /// <summary>
    /// 刷新总价显示
    /// </summary>
    public void Refresh()
    {
        totalPrice = 0;
        foreach(var goodsShow_Bourse in goodsShow_Bourses)
        {
            totalPrice += goodsShow_Bourse.TotallPrice();
        }
        priceText.text = totalPrice.ToString();
    }   
}
