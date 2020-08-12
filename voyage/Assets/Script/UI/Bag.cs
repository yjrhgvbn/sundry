using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 只保存要显示的物品，一切属性以Article中为基准
/// </summary>
public class Bag : MonoBehaviour {    


    public static Bag _instance;

    private GameObject[] Button;
    private GameObject grid;
    private int nowPage = 0;
    private GameObject InventoryTitle;
    private GameObject PagePoint;

    //保存的字符串格式为：名字_类型_编号，
    //其中编号-1表示需要更新文字显示的，编号为正数表示数量显示为999
    private List<string> GoodsMessage = new List<string>();
    private List<string> ConsumableMessage = new List<string>(48);
    private List<string> CollectionMessage = new List<string>(48);
    private List<string> QuestItemMessage = new List<string>(48);
    private List<string> NowShowMessage = new List<string>(48);

    private void Awake()
    {
        _instance = this;
        grid = transform.Find("Grid").gameObject;
        InventoryTitle = transform.Find("InventoryTitle").gameObject;
        PagePoint = transform.Find("PagePoint").gameObject;
        string[] temp = new string[48];
        GoodsMessage.AddRange(temp);
        ConsumableMessage.AddRange(temp);
        CollectionMessage.AddRange(temp);
        QuestItemMessage.AddRange(temp);
        NowShowMessage = GoodsMessage;
        Button = new GameObject[4];
        Button[0] = transform.Find("Goods").gameObject;
        Button[1] = transform.Find("Consumable").gameObject;
        Button[2] = transform.Find("Collection").gameObject;
        Button[3] = transform.Find("QuestItem").gameObject;
    }

    private void Update()
    {        
        
    }


    /// <summary>
    /// 统计物品到背包UI中，已显示的也可以调用,背包满返回false
    /// </summary>
    /// <param name="name">格式：名字_类型_编号，为-1表示会更新数量显示，正数恒为999</param>
    /// <param name="startPage">添加到某一页，其他页数有相同的物品时自动删除</param>
    public bool TryAddArticleToBag(string name, int startPage = -1)
    {   
        //name:物品名字
        //根据名字添加到不同类型的背包
        int endPos = 48,stratPos = 0;
        bool isSucceed = true;
        if(startPage!=-1)
        {
            endPos = startPage * 16 + 16;
            stratPos = startPage * 16;
        }
        int pos = -1;
        switch (name.Split('_')[1])
        {
            case "Goods":
                if (GoodsMessage.Contains(name))
                {
                    //不是添加到其他页且已经显示时直接默认成功
                    if(startPage == -1)
                        break;
                    else
                        pos = GoodsMessage.IndexOf(name);
                }
                isSucceed = TryAddMessageInRange(GoodsMessage, name, stratPos, endPos);
                if(isSucceed && GoodsMessage.Contains(name) && startPage != -1)
                {
                    GoodsMessage[pos] = null;
                }
                break;
            case "Consumable":
                if (ConsumableMessage.Contains(name) && startPage == -1)
                {
                    if (startPage == -1)
                        break;
                    else
                        pos = ConsumableMessage.IndexOf(name);
                }
                isSucceed = TryAddMessageInRange(ConsumableMessage, name, stratPos, endPos);
                if (isSucceed && ConsumableMessage.Contains(name) && startPage != -1)
                {
                    ConsumableMessage[pos] = null;
                }
                break;
            case "Collection":
                if (CollectionMessage.Contains(name) && startPage == -1)
                {
                    if (startPage == -1)
                        break;
                    else
                        pos = CollectionMessage.IndexOf(name);
                }
                isSucceed = TryAddMessageInRange(CollectionMessage, name, stratPos, endPos);
                if (isSucceed && CollectionMessage.Contains(name) && startPage != -1)
                {
                    CollectionMessage[pos] = null;
                }
                break;
            case "QuestItem":
                if (QuestItemMessage.Contains(name) && startPage == -1)
                {
                    if (startPage == -1)
                        break;
                    else
                        pos = QuestItemMessage.IndexOf(name);
                }
                isSucceed = TryAddMessageInRange(QuestItemMessage, name, stratPos, endPos);
                if (isSucceed && QuestItemMessage.Contains(name) && startPage != -1)
                {
                    QuestItemMessage[pos] = null;
                }
                break;
        }
        UpdateGridShow();
        return isSucceed;
    }

    /// <summary>
    ///直接从背包UI删除第一个找到的name
    /// </summary>
    public void DeleteArticleByBag(string name)
    {                        
        switch (name.Split('_')[1])
        {
            case "Goods":
                GoodsMessage[GoodsMessage.IndexOf(name)] = null;
                break;
            case "Consumable":
                GoodsMessage[ConsumableMessage.IndexOf(name)] = null;
                break;
            case "Collection":
                GoodsMessage[CollectionMessage.IndexOf(name)] = null;
                break;
            case "QuestItem":
                GoodsMessage[QuestItemMessage.IndexOf(name)] = null;
                break;
        }
        UpdateGridShow();
    }

    /// <summary>
    /// 更新背包显示
    /// </summary>
    public void UpdateGridShow()
    {
        //只更新显示的16个图表的信息
        for (int i = nowPage * 16, j = 0; j < 16; j++, i++)
        {
            if (NowShowMessage[i] == null)
            {
                grid.transform.GetChild(j).GetChild(0).gameObject.SetActive(false);
            }
            else
            {
                grid.transform.GetChild(j).GetChild(0).gameObject.SetActive(true);
                ArticleMessage temp = Article._instance.GetArticleByName(NowShowMessage[i].Split('_')[0]);
                grid.transform.GetChild(j).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(ResourcePath.ArticleIconPath + temp.iconName);
                if (NowShowMessage[i].Split('_')[2] == "-1")
                {
                    grid.transform.GetChild(j).GetChild(0).GetComponentInChildren<Text>().text = ((Article._instance.GetGoodsByName(temp.name).number - 1) % 999 + 1).ToString();              
                }
                else
                {
                    grid.transform.GetChild(j).GetChild(0).GetComponentInChildren<Text>().text = "999";
                }
            }
        }
    }

    /// <summary>
    /// 交换当前背包的物品，不要求j格有物品
    /// </summary>
    /// <param name="j">-2表示移动到前一页，-1表示移动到后一页，当前页面要求正数表示</param>
    public void  SwapeByGridId(int i,int j)
    {
        //移动到别的页面时，如果页面已满，与最后一格交换
        int iPos = i + nowPage * 16, jPos = j + nowPage * 16;
        if (j == -2)
        {
            int prePage = (nowPage + 2) % 3;
            if (TryAddArticleToBag(NowShowMessage[iPos], prePage))
                return;
            string t = NowShowMessage[prePage * 16 + 15];
            NowShowMessage[prePage * 16 + 15] = NowShowMessage[iPos];
            NowShowMessage[iPos] = t;
            UpdateGridShow();
            return;
        }
        if(j == -1)
        {
            int nextPage = (nowPage + 1) % 3;
            if (TryAddArticleToBag(NowShowMessage[iPos], nextPage))
                return;
            string t = NowShowMessage[nextPage * 16 + 15];
            NowShowMessage[nextPage * 16 + 15] = NowShowMessage[iPos];
            NowShowMessage[iPos] = t;
            UpdateGridShow();
            return;
        }        
        if(NowShowMessage[jPos] == null)
        {
            NowShowMessage[jPos] = NowShowMessage[iPos];
            NowShowMessage[iPos] = null;
        }
        else
        {
            string t = NowShowMessage[jPos];
            NowShowMessage[jPos] = NowShowMessage[iPos];
            NowShowMessage[iPos] = t;
        }
        UpdateGridShow();
    }

    public void ToNextPage()
    {
        nowPage = (nowPage + 1) % 3;
        for (int i = 0; i < 3; i++)
        {
            PagePoint.transform.GetChild(i).GetComponent<Image>().color = Color.white;
        }
        PagePoint.transform.GetChild(nowPage).GetComponent<Image>().color = Color.yellow;
        UpdateGridShow();
    }

    public void ToPrePage()
    {
        nowPage = (nowPage + 2) % 3;
        for (int i = 0; i < 3; i++)
        {
            PagePoint.transform.GetChild(i).GetComponent<Image>().color = Color.white;
        }
        PagePoint.transform.GetChild(nowPage).GetComponent<Image>().color = Color.yellow;
        UpdateGridShow();
    }

    public void ShowGoods()
    {
        foreach(var g in Button)
        {
            g.GetComponent<Image>().color = Color.white;
        }
        Button[0].GetComponent<Image>().color = Color.yellow;
        NowShowMessage = GoodsMessage;
        InventoryTitle.transform.GetComponentInChildren<Text>().text = "Goods";
        UpdateGridShow();
    }

    public void ShowConsumable()
    {
        foreach (var g in Button)
        {
            g.GetComponent<Image>().color = Color.white;
        }
        Button[1].GetComponent<Image>().color = Color.yellow;
        NowShowMessage = ConsumableMessage;
        InventoryTitle.transform.GetComponentInChildren<Text>().text = "Consumable";
        UpdateGridShow();
    }

    public void ShowCollection()
    {
        foreach (var g in Button)
        {
            g.GetComponent<Image>().color = Color.white;
        }
        Button[2].GetComponent<Image>().color = Color.yellow;
        NowShowMessage = CollectionMessage;
        InventoryTitle.transform.GetComponentInChildren<Text>().text = "Collection";
        UpdateGridShow();
    }

    public void ShowQuestItem()
    {
        foreach (var g in Button)
        {
            g.GetComponent<Image>().color = Color.white;
        }
        Button[3].GetComponent<Image>().color = Color.yellow;
        NowShowMessage = QuestItemMessage;
        InventoryTitle.transform.GetComponentInChildren<Text>().text = "QuestItem";
        UpdateGridShow();
    }

    private bool TryAddMessageInRange(List<string> MessageType,string name,int strat,int end)
    {
        for (int i = strat; i < end; i++)
        {
            if (MessageType[i] == null)
            {
                MessageType[i] = name;
                break;
            }
            if (i == end - 1)
            {
                //TODO
                //表示物品已满
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 返回格式：名字_类型_编号
    /// </summary>
    public string  GetFormatNameByNowGridId(int i)
    {
        return NowShowMessage[i + nowPage * 16];
    }
}
