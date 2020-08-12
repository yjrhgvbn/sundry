using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 背包悬停信息显示
/// </summary>
public class BagSpecific : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    private Text spcificName;
    private Image icon;
    private Text type;
    private Text describe;
    private Text purPrice;

    private RectTransform rectTransform;
    private CanvasGroup CG;
    private bool isMouseExit;
    private bool isToColse;

    private void Awake()
    {
        spcificName = transform.Find("name_text").GetComponent<Text>();
        type = transform.Find("type_text").GetComponent<Text>();
        describe = transform.Find("describe_text").GetComponent<Text>();
        purPrice = transform.Find("purPrice_text").GetComponent<Text>();
        icon = transform.Find("Article_icon").GetComponent<Image>();
        CG = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        //SetSpecific(Article._instance.GetArticleByName("Bowl"), 100);
    }

    private void Update()
    {
        if ((isToColse && isMouseExit) || Input.GetMouseButtonDown(0))
        {
            CG.alpha = 0f;
            CG.blocksRaycasts = false;
            CG.interactable = false;
        }
    }

    /// <summary>
    /// 设置显示内容
    /// ToDo
    /// 将显示的价格改为平均值
    /// </summary>    
    /// <param name="price">取购买的平均值</param>
    public void SetSpecific(string name,int price)
    {
        ArticleMessage article = Article._instance.GetArticleByName(name);
        spcificName.text = article.name;
        icon.sprite = Resources.Load<Sprite>(ResourcePath.ArticleIconPath + article.iconName);
        string text = "";
        foreach(ArticleMessage.Type type in article.types)
        {
            text += "," + type.ToString();
        }
        type.text = text.Substring(1);
        describe.text = article.describe;
        purPrice.text = price.ToString();
    }

    /// <summary>
    /// 设定信息栏浮现位置，0↘，1↙，2↖，3↗
    /// </summary>
    /// <param name="i"></param>
    public void SetPos(int direction)
    {
        float width = rectTransform.rect.width;
        float height = rectTransform.rect.height;
        if(direction == 0)
        {
            rectTransform.position = Input.mousePosition + new Vector3(width, -height, 0);
        }
        else if (direction == 1)
        {
            rectTransform.position = Input.mousePosition + new Vector3(-width, -height, 0);
        }
        else if (direction == 2)
        {
            rectTransform.position = Input.mousePosition + new Vector3(-width, height, 0);
        }
        else if (direction == 3)
        {
            rectTransform.position = Input.mousePosition + new Vector3(width, height, 0);
        }
    }

    public void Show()
    {
        isToColse = false;
        CG.alpha = 1f;
        CG.blocksRaycasts = true;
        CG.interactable = true;
    }

    public void Hide()
    {
        isToColse = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseExit = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseExit = true;
    }
}
