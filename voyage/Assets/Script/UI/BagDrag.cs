using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BagDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private GameObject DragIcon;

    //点击时背包图标时进行拖拽
    public void OnBeginDrag(PointerEventData eventData)
    {
        if(transform.GetChild(0).gameObject.activeInHierarchy == false)
        {
            DragIcon = null;
            return;
        }
        DragIcon = Canvas.Instantiate(transform.GetChild(0).gameObject,transform.root);
        DragIcon.GetComponent<RectTransform>().localPosition = Vector3.zero;
        DragIcon.transform.SetAsLastSibling();

        var group = DragIcon.AddComponent<CanvasGroup>();
        group.blocksRaycasts = false;
        SetDragPostion(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (DragIcon == null)
        {
            eventData.pointerDrag = null;
            return;
        }
        SetDragPostion(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (DragIcon != null)          
            Destroy(DragIcon);
        DragIcon = null;
    }

    private void SetDragPostion(PointerEventData eventData)
    {
        RectTransform rt = DragIcon.GetComponent<RectTransform>();

        Vector3 mouseWorldPos;
        if(RectTransformUtility.ScreenPointToWorldPointInRectangle(rt,eventData.position,eventData.pressEventCamera,out mouseWorldPos))
        {
            rt.position = mouseWorldPos;
        }
    }
}
