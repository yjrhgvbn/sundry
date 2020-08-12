using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ArrowDrop : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
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
        if(gameObject.name == "LeftArrow")
            Bag._instance.SwapeByGridId(drayGameObject.GetComponentInParent<BagDrop>().ID, -2);
        else
            Bag._instance.SwapeByGridId(drayGameObject.GetComponentInParent<BagDrop>().ID, -1);
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
