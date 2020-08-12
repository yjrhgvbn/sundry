using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inventory : MonoBehaviour {

    public static inventory _instance;
    private TweenPosition tween;
    private int coinCount = 1000;
    private bool isShow = false;

    public List<InventoryItemGrid> itemGridList = new List<InventoryItemGrid>();
    public UILabel coinNumberLabel;
    public GameObject InventoryItem;

    private void Awake()
    {
        _instance = this;
        tween = this.GetComponent<TweenPosition>();      
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.X))
        {                      
            GetID(Random.Range(2001,2023));            
        }
    }

    //拾起到id的物品，并添加到物品栏里
    public void GetID(int id, int count = 1)
    {
        InventoryItemGrid grid = null;
        foreach (InventoryItemGrid temp in itemGridList)
        {
            if (temp.ID == id)
            {
                grid = temp;
                break;
            }
        }
        if (grid != null)
        {
            grid.PluseNumber(count);
        }
        else
        {
            foreach (InventoryItemGrid temp in itemGridList)
            {
                if (temp.ID == 0)
                {
                    grid = temp;
                    break;
                }
            }
            if(grid!=null)
            {
                GameObject itemGo = NGUITools.AddChild(grid.gameObject, InventoryItem);
                itemGo.transform.localPosition = Vector3.zero;
                itemGo.GetComponent<UISprite>().depth = 4;
                grid.SetID(id, count);
            }
        }
    }

    void Show()
    {
        isShow = true;        
        tween.PlayForward();
    }

    void Hide()
    {
        isShow = false;
        tween.PlayReverse();
    }

    public void TransformState()
    {
        if(!isShow)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    public void AddCoin(int coin)
    {
        coinCount += coin;
        coinNumberLabel.text = coinCount.ToString();
    }

    public bool GetCoin(int coin)
    {
        if(coinCount>=coin)
        {
            coinCount -= coin;
            coinNumberLabel.text = coinCount.ToString();
            return true;
        }
        return false;
    }

    public bool MinusId(int id,int count = 1)
    {
        InventoryItemGrid grid = null;
        foreach (InventoryItemGrid temp in itemGridList)
        {
            if (temp.ID == id)
            {
                grid = temp;
                break;
            }
        }
        if (grid == null)
        {
            return false;
        }
        else
        {
            return grid.MinusNumber(count);
        }
    }
}
