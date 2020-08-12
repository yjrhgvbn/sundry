using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentItem : MonoBehaviour {

    private UISprite sprite;
    public int ID;

    private bool isHover = false;

    private void Awake()
    {        
        sprite = this.GetComponent<UISprite>();    
    }

    private void Update()
    {
        if (isHover)//当鼠标在装备栏之上时
        {
            if(Input.GetMouseButtonDown(1))//卸下装备
            {
                EquipmentUI._instance.TakeOff(ID, this.gameObject);               
            }
        }        
    }

    public void SetID(int ID)
    {
        this.ID = ID;
        ObjectInfo info = ObjectsInfo._instance.GetObjectByID(ID);
        SetInfo(info);
    }

    public void SetInfo(ObjectInfo info)
    {
        this.ID = info.ID;
        sprite.spriteName = info.icon_name;
    }

    public void OnHoverOver()
    {
        isHover = true;
    }

    public void OnHoverOut()
    {
        isHover = false;
    }
}
