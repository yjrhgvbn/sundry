using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TypeIconDrop : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Image image;
    private Vector3 EnterScale;
    private Vector3 normalScale;

    void Start()
    {
        image = transform.GetComponent<Image>();
        normalScale = transform.GetComponent<RectTransform>().localScale;
        EnterScale = transform.GetComponent<RectTransform>().localScale * 1.1f;
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject drayGameObject = eventData.pointerDrag;
        if (drayGameObject == null)
            return;
        string drayName = Bag._instance.GetFormatNameByNowGridId(int.Parse(drayGameObject.name.Split('_')[1]));
        ArticleMessage drayArticle =  Article._instance.GetArticleByName(drayName.Split('_')[0]);
        switch (gameObject.name)
        {
            case "Goods":
                if (drayName.Split('-')[1] == "Goods")
                    break;
                if(drayArticle.types.Contains(ArticleMessage.Type.Goods))
                {
                    //TODO
                    //获得要转移的数量
                    
                }
                break;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.GetComponent<RectTransform>().localScale = EnterScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.GetComponent<RectTransform>().localScale = normalScale;
    }
}
