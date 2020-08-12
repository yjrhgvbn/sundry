using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : UIDragDropItem {

    private UISprite sprite;
    private int ID;
    private bool isHover = false;    

    private void Awake()
    {        
        sprite = this.GetComponent<UISprite>();
    }

    private void Update()
    {
        if(isHover)
        {
            InventoryDes._instance.Show(ID);

            if(Input.GetMouseButtonDown(1))
            {
                bool success = EquipmentUI._instance.Dress(ID);
                if(success)
                {
                    transform.parent.GetComponent<InventoryItemGrid>().MinusNumber();
                }
            }
        }
    }

    protected override void OnDragDropRelease(GameObject surface)
    {
        base.OnDragDropRelease(surface);

        if (surface != null)
        {
            if (surface.tag == Tags.inventory_item_grid)//空格子
            {
                if (surface == this.transform.parent.gameObject)//拖放到自己的格子
                { }
                else
                {
                    InventoryItemGrid oldParent = this.transform.parent.GetComponent<InventoryItemGrid>();

                    this.transform.parent = surface.transform;
                    ResetPosition();
                    InventoryItemGrid newParent = surface.GetComponent<InventoryItemGrid>();
                    newParent.SetID(oldParent.ID, oldParent.num);

                    oldParent.ClearInfo();
                }
            }
            else if(surface.tag == Tags.inventory_item)//拖拽到有物品的格子
            {
                InventoryItemGrid grid1 = this.transform.parent.GetComponent<InventoryItemGrid>();
                InventoryItemGrid grid2 = surface.transform.parent.GetComponent<InventoryItemGrid>();
                int id = grid1.ID;int num = grid1.num;
                grid1.SetID(grid2.ID, grid2.num);
                grid2.SetID(id, num);

            }     
            else if(surface.tag == Tags.short_cut)//拖到快捷方式
            {
                surface.GetComponent<ShortCutGrid>().SetInventory(ID);
            }
        }
        ResetPosition();
    }

    void ResetPosition()
    {
        transform.localPosition = Vector3.zero;
    }

    public void SetID(int ID)
    {
        ObjectInfo info = ObjectsInfo._instance.GetObjectByID(ID);
        sprite.spriteName = info.icon_name;
        this.ID = ID;
    }

    public void SetIconName(int ID,string icon_name)
    {
        this.ID = ID;
        sprite.spriteName = icon_name;
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
