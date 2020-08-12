using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



/// <summary>
/// 物品的共同特征
/// </summary>
public class ArticleMessage
{
    public string name;
    public string ID;
    public int totalNumber = 0;
    public string iconName;
    public string describe;
    public List<Type> types = new List<Type>();
    public enum Type
    {
        Normal, // 表示一般情况，即types[0];
        Goods,
        Consumable,
        Collection,
        QuestItem
    }
}
public class GoodsMessage
{
    public int number;
    public int weight;
}
public class ConsumableMessage
{
    public int number;
    public int weight;
}
public class CollectionMessage
{
    public int number;
}
public class QuestItemMessage
{
    public int number;
}


/// <summary>
/// 所有物品信息都保存这里
/// </summary>
public class Article : MonoBehaviour {

    public static Article _instance;

    public TextAsset articleInfoText;
    private Dictionary<string, ArticleMessage> articleDic = new Dictionary<string, ArticleMessage>();
    private Dictionary<string, GoodsMessage> goodsDic = new Dictionary<string, GoodsMessage>();
    private Dictionary<string, ConsumableMessage> consumableDic = new Dictionary<string, ConsumableMessage>();
    private Dictionary<string, CollectionMessage> collectionDic = new Dictionary<string, CollectionMessage>();
    private Dictionary<string, QuestItemMessage> questItemDic = new Dictionary<string, QuestItemMessage>();
    private Dictionary<string, string> IdName = new Dictionary<string, string>();    

    private void Awake()
    {
        _instance = this;
        ReadInformation();        
    }

    private void Start()
    {
        AddArticle("Bowl");
    }

    private void Update()
    {
    }

    public ArticleMessage GetArticleByID(string ID)
    {
        return articleDic[IdName[ID]];
    }
    public GoodsMessage GetGoodsByID(string ID)
    {
        return goodsDic[IdName[ID]];
    }

    /// <summary>
    /// 在所有物品中获取
    /// </summary>
    public ArticleMessage GetArticleByName(string name)
    {
        return articleDic[name];
    }
    public GoodsMessage GetGoodsByName(string name)
    {
        return goodsDic[name];
    }

    /// <summary>
    /// 在已拥有物品中获取
    /// </summary>
    public ArticleMessage GetOwnArticleByName(string name)
    {
        if (articleDic[name].totalNumber > 0)
            return articleDic[name];
        return null;
    }

    /// <summary>
    /// 添加物品,会判断背包是否已满
    /// </summary>
    /// <param name="num">不能为负数</param>
    public void AddArticle(string name, int num = 1)
    {
        //一个类型背包满时，会自动添加到另一个可转换的背包
        if (articleDic[name] == null)
        {
            //表示没有该物品
            //TODO
        }
        else
        {
            //添加到数据并更新背包        
            articleDic[name].totalNumber += num;
            bool isSucced = true;
            for (int i = 0; i < articleDic[name].types.Count; i++)
            {               
                string bagName = null;
                switch (articleDic[name].types[i])
                {
                    case ArticleMessage.Type.Goods:
                        while(num > 999)
                        {
                            //每999个物品分出一个格显示，减少到999
                            bagName = name + "_" + "Goods" + "_" + (goodsDic[name].number/999).ToString();
                            isSucced = Bag._instance.TryAddArticleToBag(bagName);
                            if (isSucced)
                            {
                                num -= 999;
                                goodsDic[name].number += 999;
                                articleDic[name].totalNumber += 999;
                            }
                            else
                                break;
                        }
                        if (isSucced && goodsDic[name].number / 999 != (goodsDic[name].number + num) / 999)
                        {
                            //要添加之后一格数量大于999
                            bagName = name + "_" + "Goods" + "_" + (goodsDic[name].number / 999).ToString();
                            isSucced = Bag._instance.TryAddArticleToBag(bagName);
                        }
                        if (isSucced)
                        {
                            //更新能变更数量的格子
                            goodsDic[name].number += num;
                            articleDic[name].totalNumber += num;
                            bagName = name + "_" + "Goods" + "_" + "-1";
                            isSucced = Bag._instance.TryAddArticleToBag(bagName);
                        }
                        else
                        {
                            //不成功则只添加到999;
                            int CanAdd = 999 - (goodsDic[name].number - 1) % 999 - 1;
                            goodsDic[name].number += CanAdd;
                            articleDic[name].totalNumber += CanAdd;
                            num -= CanAdd;
                            //TODO
                            //显示信息
                            if (i == articleDic[name].types.Count - 1)
                            {
                                //TODO
                                //表示所有可装背包都满了
                            }
                            Bag._instance.UpdateGridShow();
                        }
                        break;
                    case ArticleMessage.Type.Consumable:
                        while (num > 999)
                        {
                            bagName = name + "_" + "Consumable" + "_" + (consumableDic[name].number / 999).ToString();
                            isSucced = Bag._instance.TryAddArticleToBag(bagName);
                            if (isSucced)
                            {
                                num -= 999;
                                consumableDic[name].number += 999;
                                articleDic[name].totalNumber += 999;
                            }
                            else
                                break;
                        }
                        if (isSucced && consumableDic[name].number / 999 != (consumableDic[name].number + num) / 999)
                        {
                            bagName = name + "_" + "Consumable" + "_" + (consumableDic[name].number / 999).ToString();
                            isSucced = Bag._instance.TryAddArticleToBag(bagName);
                        }
                        if (isSucced)
                        {
                            consumableDic[name].number += num;
                            articleDic[name].totalNumber += num;
                            bagName = name + "_" + "Consumable" + "_" + "-1";
                            isSucced = Bag._instance.TryAddArticleToBag(bagName);
                        }
                        else
                        {
                            //不成功则只添加到999;
                            int CanAdd = 999 - (consumableDic[name].number - 1) % 999 - 1;
                            consumableDic[name].number += CanAdd;
                            articleDic[name].totalNumber += CanAdd;
                            num -= CanAdd;
                            //TODO
                            //显示信息
                            if (i == articleDic[name].types.Count - 1)
                            {
                                //TODO
                                //表示所有可装背包都满了
                            }
                            Bag._instance.UpdateGridShow();
                        }
                        break;
                    case ArticleMessage.Type.Collection:
                        while (num > 999)
                        {
                            bagName = name + "_" + "Collection" + "_" + (collectionDic[name].number / 999).ToString();
                            isSucced = Bag._instance.TryAddArticleToBag(bagName);
                            if (isSucced)
                            {
                                num -= 999;
                                collectionDic[name].number += 999;
                                articleDic[name].totalNumber += 999;
                            }
                            else
                                break;
                        }
                        if (isSucced && collectionDic[name].number / 999 != (collectionDic[name].number + num) / 999)
                        {
                            bagName = name + "_" + "Collection" + "_" + (collectionDic[name].number / 999).ToString();
                            isSucced = Bag._instance.TryAddArticleToBag(bagName);
                        }
                        if (isSucced)
                        {
                            collectionDic[name].number += num;
                            articleDic[name].totalNumber += num;
                            bagName = name + "_" + "Collection" + "_" + "-1";
                            isSucced = Bag._instance.TryAddArticleToBag(bagName);
                        }
                        else
                        {
                            //不成功则只添加到999;
                            int CanAdd = 999 - (collectionDic[name].number - 1) % 999 - 1;
                            collectionDic[name].number += CanAdd;
                            articleDic[name].totalNumber += CanAdd;
                            num -= CanAdd;
                            //TODO
                            //显示信息
                            if (i == articleDic[name].types.Count - 1)
                            {
                                //TODO
                                //表示所有可装背包都满了
                            }
                            Bag._instance.UpdateGridShow();
                        }
                        break;
                    case ArticleMessage.Type.QuestItem:
                        while (num > 999)
                        {
                            bagName = name + "_" + "QuestItem" + "_" + (questItemDic[name].number / 999).ToString();
                            isSucced = Bag._instance.TryAddArticleToBag(bagName);
                            if (isSucced)
                            {
                                num -= 999;
                                questItemDic[name].number += 999;
                                articleDic[name].totalNumber += 999;
                            }
                            else
                                break;
                        }
                        if (isSucced && questItemDic[name].number / 999 != (questItemDic[name].number + num) / 999)
                        {
                            bagName = name + "_" + "QuestItem" + "_" + (questItemDic[name].number / 999).ToString();
                            isSucced = Bag._instance.TryAddArticleToBag(bagName);
                        }
                        if (isSucced)
                        {
                            questItemDic[name].number += num;
                            articleDic[name].totalNumber += num;
                            bagName = name + "_" + "QuestItem" + "_" + "-1";
                            isSucced = Bag._instance.TryAddArticleToBag(bagName);
                        }
                        else
                        {
                            //不成功则只添加到999;
                            int CanAdd = 999 - (questItemDic[name].number - 1) % 999 - 1;
                            questItemDic[name].number += CanAdd;
                            articleDic[name].totalNumber += CanAdd;
                            num -= CanAdd;
                            //TODO
                            //显示信息
                            if (i == articleDic[name].types.Count - 1)
                            {
                                //TODO
                                //表示所有可装背包都满了
                            }
                            Bag._instance.UpdateGridShow();
                        }
                        break;
                }                
                if (isSucced)
                    break;
            }
        }
    }

    /// <summary>
    /// 减少物品，用正数表示要减少的数量，添加调用AddArticle
    /// </summary>
    /// <param name="num">不能为负数</param>
    public void DeleteArticle(string name, int num = 1, ArticleMessage.Type type = ArticleMessage.Type.Normal)
    {
        if (articleDic[name] == null)
        {
            //表示没有该物品
            //TODO
        }
        if (type == ArticleMessage.Type.Normal)
            type = articleDic[name].types[0];
        string bagName = null;
        switch (type)
        {
            case ArticleMessage.Type.Goods:
                if(goodsDic[name].number<num)
                {
                    //TODO
                    //表示不够
                    //articleDic[name].totalNumber = 0;
                    //goodsDic[name].number = 0;
                    //Bag._instance.DeleteArticleByBag(name + "_" + "Goods" + "_" + "-1");
                    break;
                }                
                while(num>999)
                {
                    bagName = name + "_" + "Goods" + "_" + (goodsDic[name].number / 999 - 1).ToString();
                    Bag._instance.DeleteArticleByBag(bagName);
                    articleDic[name].totalNumber -= 999;
                    goodsDic[name].number -= 999;
                    num -= 999;
                }
                if((goodsDic[name].number - num) /999 != goodsDic[name].number/999)
                {
                    //删除图标
                    bagName = name + "_" + "Goods" + "_" + (goodsDic[name].number / 999 - 1).ToString();
                    Bag._instance.DeleteArticleByBag(bagName);
                }
                articleDic[name].totalNumber -= num;
                goodsDic[name].number -= num;
                if(goodsDic[name].number <= 0)
                {
                    Bag._instance.DeleteArticleByBag(name + "_" + "Goods" + "_" + "-1");
                }
                break;
            case ArticleMessage.Type.Consumable:
                if (goodsDic[name].number < num)
                {
                    //TODO
                    //表示不够
                    break;
                }
                while (num > 999)
                {
                    bagName = name + "_" + "Consumable" + "_" + (consumableDic[name].number / 999 - 1).ToString();
                    Bag._instance.DeleteArticleByBag(bagName);
                    articleDic[name].totalNumber -= 999;
                    consumableDic[name].number -= 999;
                    num -= 999;
                }
                if ((consumableDic[name].number - num) / 999 != consumableDic[name].number / 999)
                {
                    bagName = name + "_" + "Consumable" + "_" + (consumableDic[name].number / 999 - 1).ToString();
                    Bag._instance.DeleteArticleByBag(bagName);
                }
                articleDic[name].totalNumber -= num;
                consumableDic[name].number -= num;
                if (consumableDic[name].number <= 0)
                {
                    Bag._instance.DeleteArticleByBag(name + "_" + "Consumable" + "_" + "-1");
                }
                break;
            case ArticleMessage.Type.Collection:
                if (goodsDic[name].number < num)
                {
                    //TODO
                    //表示不够
                    break;
                }
                while (num > 999)
                {
                    bagName = name + "_" + "Collection" + "_" + (collectionDic[name].number / 999 - 1).ToString();
                    Bag._instance.DeleteArticleByBag(bagName);
                    articleDic[name].totalNumber -= 999;
                    collectionDic[name].number -= 999;
                    num -= 999;
                }
                if ((collectionDic[name].number - num) / 999 != collectionDic[name].number / 999)
                {
                    bagName = name + "_" + "Collection" + "_" + (collectionDic[name].number / 999 - 1).ToString();
                    Bag._instance.DeleteArticleByBag(bagName);
                }
                articleDic[name].totalNumber -= num;
                collectionDic[name].number -= num;
                if (collectionDic[name].number <= 0)
                {
                    Bag._instance.DeleteArticleByBag(name + "_" + "Collection" + "_" + "-1");
                }
                break;
            case ArticleMessage.Type.QuestItem:
                if (goodsDic[name].number < num)
                {
                    //TODO
                    //表示不够
                    break;
                }
                while (num > 999)
                {
                    bagName = name + "_" + "QuestItem" + "_" + (questItemDic[name].number / 999 - 1).ToString();
                    Bag._instance.DeleteArticleByBag(bagName);
                    articleDic[name].totalNumber -= 999;
                    questItemDic[name].number -= 999;
                    num -= 999;
                }
                if ((questItemDic[name].number - num) / 999 != questItemDic[name].number / 999)
                {
                    bagName = name + "_" + "QuestItem" + "_" + (questItemDic[name].number / 999 - 1).ToString();
                    Bag._instance.DeleteArticleByBag(bagName);
                }
                articleDic[name].totalNumber -= num;
                questItemDic[name].number -= num;
                if (questItemDic[name].number <= 0)
                {
                    Bag._instance.DeleteArticleByBag(name + "_" + "QuestItem" + "_" + "-1");
                }
                break;
        }
        Bag._instance.UpdateGridShow();
    }

    void ReadInformation()
    {
        //读取保存全部物品信息的文本
        string text = articleInfoText.text;
        string[] articleInfos = text.Split('\n');
        for (int i = 1; i < articleInfos.Length; i++)
        {        
            string[] articleMessage = articleInfos[i].Split(';');
            ArticleMessage temp = new ArticleMessage
            {
                name = articleMessage[0].Trim(),
                iconName = articleMessage[5].Trim(),
                describe = articleMessage[6].Trim(),
            };
            temp.totalNumber += int.Parse(articleMessage[4].Trim());
            string type = articleMessage[2].Trim();
            switch (type)
            {
                case "Goods":
                    if (articleDic.ContainsKey(temp.name))
                    {
                        if (!articleDic[temp.name].types.Contains(ArticleMessage.Type.Goods))
                            articleDic[temp.name].types.Add(ArticleMessage.Type.Goods);
                    }
                    else
                    {
                        temp.types.Add(ArticleMessage.Type.Goods);
                    }
                    if (goodsDic.ContainsKey(temp.name))
                    {
                        goodsDic[temp.name].number += int.Parse(articleMessage[4].Trim());
                    }
                    else
                    {
                        GoodsMessage goodsTemp = new GoodsMessage
                        {
                            number = int.Parse(articleMessage[4].Trim()),
                            weight = int.Parse(articleMessage[3].Trim())
                        };
                        goodsDic.Add(temp.name, goodsTemp);
                    }                    
                    break;
                case "Consumable":
                    if (articleDic.ContainsKey(temp.name))
                    {
                        if (!articleDic[temp.name].types.Contains(ArticleMessage.Type.Consumable))
                            articleDic[temp.name].types.Add(ArticleMessage.Type.Consumable);
                    }
                    else
                    {
                        temp.types.Add(ArticleMessage.Type.Consumable);
                    }
                    if (consumableDic.ContainsKey(temp.name))
                    {
                        consumableDic[temp.name].number += int.Parse(articleMessage[4].Trim());
                    }
                    else
                    {
                        ConsumableMessage consumableTemp = new ConsumableMessage
                        {
                            number = int.Parse(articleMessage[4].Trim()),
                            weight = int.Parse(articleMessage[3].Trim())
                        };
                        consumableDic.Add(temp.name, consumableTemp);
                    }
                    break;
                case "Collection":
                    if (articleDic.ContainsKey(temp.name))
                    {
                        if (!articleDic[temp.name].types.Contains(ArticleMessage.Type.Collection))
                            articleDic[temp.name].types.Add(ArticleMessage.Type.Collection);
                    }
                    else
                    {
                        temp.types.Add(ArticleMessage.Type.Collection);
                    }
                    if (collectionDic.ContainsKey(temp.name))
                    {
                        collectionDic[temp.name].number += int.Parse(articleMessage[4].Trim());
                    }
                    else
                    {
                        CollectionMessage collectionTemp = new CollectionMessage
                        {
                            number = int.Parse(articleMessage[4].Trim())
                        };
                        collectionDic.Add(temp.name, collectionTemp);
                    }
                    break;
                case "QuestItem":
                    if (articleDic.ContainsKey(temp.name))
                    {
                        if (!articleDic[temp.name].types.Contains(ArticleMessage.Type.QuestItem))
                            articleDic[temp.name].types.Add(ArticleMessage.Type.QuestItem);
                    }
                    else
                    {
                        temp.types.Add(ArticleMessage.Type.QuestItem);
                    }
                    if (questItemDic.ContainsKey(temp.name))
                    {
                        questItemDic[temp.name].number += int.Parse(articleMessage[4].Trim());
                    }
                    else
                    {
                        QuestItemMessage questItemTemp = new QuestItemMessage
                        {
                            number = int.Parse(articleMessage[4].Trim())
                        };
                        questItemDic.Add(temp.name, questItemTemp);
                    }
                    break;
            }
            if (!articleDic.ContainsKey(temp.name))
            {
                articleDic.Add(temp.name, temp);
                IdName.Add(articleMessage[1].Trim(), temp.name);
            }
        }
    } 
}
