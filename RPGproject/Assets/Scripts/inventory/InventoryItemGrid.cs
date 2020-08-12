using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemGrid : MonoBehaviour {

    public int ID = 0;
    public int num = 0;
    private ObjectInfo info = null;
    public UILabel numLabel;

	// Use this for initialization
	void Start () {
        numLabel = this.GetComponentInChildren<UILabel>();
	}
	
	public void SetID(int ID,int num = 1)
    {
        this.ID = ID;
        info = ObjectsInfo._instance.GetObjectByID(ID);
        InventoryItem item = this.GetComponentInChildren<InventoryItem>();
        item.SetIconName(ID, info.icon_name);
        numLabel.enabled = true;
        this.num = num;
        numLabel.text = num.ToString();
    }

    public void ClearInfo()
    {
        ID = 0;
        info = null;
        num = 0;
        numLabel.enabled = false;
    }
           
   
    public void PluseNumber(int num =1)
    {
        this.num += num;
        numLabel.text = this.num.ToString();
    }

    public bool MinusNumber(int num = 1)
    {
        if (this.num >= num)
        {
            this.num -= num;
            numLabel.text = this.num.ToString();
            if (this.num == 0)
            {
                ClearInfo();
                GameObject.Destroy(this.GetComponentInChildren<InventoryItem>().gameObject);
            }
            return true;
        }
        return false;

    }
}
