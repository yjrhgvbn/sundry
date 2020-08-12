using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoodsShow_bourse : MonoBehaviour {

    [HideInInspector]
    public int inputAmount = 0;

    private Image icon;
    private Text goodsName;
    private Text price;
    private Text remainingAmount;
    private InputField numInput;
    private ScrollRect scroll;
    private ArticleMessage nowShowArticle;


    void Awake()
    {
        icon = transform.Find("Article_icon").GetComponentInChildren<Image>();
        Transform to = transform.Find("Message");
        goodsName = to.Find("name_text").GetComponent<Text>();
        price = to.Find("price_text").GetComponent<Text>();
        remainingAmount = to.Find("remaining_text").GetComponent<Text>();
        scroll = GetComponentInParent<ScrollRect>();
        numInput = to.Find("numInput").GetComponent<InputField>();
    }

    private void Start()
    {
        //获取购买的数量，保存为numinput
        numInput.onValueChanged.AddListener(delegate {
            numInput.text = Mathf.Clamp(int.Parse(numInput.text), 0, int.Parse(remainingAmount.text)).ToString();
            inputAmount = int.Parse(numInput.text);            
            scroll.Refresh();
        });

        //SetItem(Article._instance.GetArticleByName("Bowl"), 100, 9);
    }

    void Update () {        
		
	}
    
    /// <summary>
    /// 设置每个条目的属性
    /// </summary>
    public void SetItem(ArticleMessage article,int num,int price)
    {
        nowShowArticle = article;
        icon.sprite = Resources.Load<Sprite>(ResourcePath.ArticleIconPath + article.iconName);
        goodsName.text = article.name;
        this.price.text = price.ToString();
        remainingAmount.text = num.ToString();
        numInput.text = "0";
    }

    /// <summary>
    /// 改变购买数量
    /// </summary>
    /// <param name="num">可正可负</param>
    public void ChangeInput(int num)
    {
        if (numInput.text == "")
        {
            numInput.text = "1";
        }
        else
            numInput.text = Mathf.Clamp(int.Parse(numInput.text) + num, 0, int.Parse(remainingAmount.text)).ToString();
    }

    public void MaxPlus()
    {
        //TODO
        //增加到当前金币能购买的数量
        numInput.text = remainingAmount.text.ToString();
    }
    public void MaxMinus()
    {
        numInput.text = "0";
    }

    public void Purchase()
    {
        Article._instance.AddArticle(nowShowArticle.name, inputAmount);
        remainingAmount.text = (int.Parse(remainingAmount.text) - inputAmount).ToString();
        numInput.text = "0";
        scroll.Refresh();
    }

    public int TotallPrice()
    {
        return inputAmount * int.Parse(price.text); 
    }

}
