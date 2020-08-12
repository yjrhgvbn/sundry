using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsInfo : MonoBehaviour {

    public static ObjectsInfo _instance;

    private Dictionary<int, ObjectInfo> ObjectInfoDic = new Dictionary<int, ObjectInfo>();

    public TextAsset objectsInfoText;

    private void Awake()
    {
        _instance = this;
        ReadInfo();
    }

    public ObjectInfo GetObjectByID(int ID)
    {
        ObjectInfo info = new ObjectInfo();
        ObjectInfoDic.TryGetValue(ID, out info);
        return info;
    }
    void ReadInfo()
    {
        string test = objectsInfoText.text;
        string[] strArray = test.Split('\n');

        foreach(string str in strArray)
        {
            string[] proArray = str.Split(',');        
            int ID = int.Parse(proArray[0]);
            string name = proArray[1];
            string icon_name = proArray[2];
            string strType = proArray[3];
            ObjectType type = ObjectType.Drug;
            switch(strType)
            {
                case "Drug":
                    type = ObjectType.Drug;
                    break;
                case "Equip":
                    type = ObjectType.Equip;
                    break;
                case "Mat":
                    type = ObjectType.Mat;
                    break;
                default:break;
            }

            ObjectInfo Info = new ObjectInfo();
            Info.name = name;
            Info.ID = ID;
            Info.icon_name = icon_name;
            Info.type = type;
            if (type == ObjectType.Drug)
            {
                int hp = int.Parse(proArray[4]);
                int mp = int.Parse(proArray[5]);
                int price_sell = int.Parse(proArray[6]);
                int price_buy = int.Parse(proArray[7]);
                Info.hp = hp;
                Info.mp = mp;
                Info.price_buy = price_buy;
                Info.price_sell = price_sell;
            }
            else if(type == ObjectType.Equip)
            {
                Info.attack = int.Parse(proArray[4]);
                Info.def = int.Parse(proArray[5]);
                Info.speed = int.Parse(proArray[6]);
                Info.price_sell = int.Parse(proArray[9]);
                Info.price_buy = int.Parse(proArray[10]);

                string str_dressType = proArray[7];
                switch (str_dressType)
                {
                    case "Headgear":Info.dressType = DressType.Headgear;break;
                    case "Armor":Info.dressType = DressType.Armor;break;
                    case "RightHand":Info.dressType = DressType.RightHand;break;
                    case "LeftHand":Info.dressType = DressType.LeftHand;break;
                    case "Shoe":Info.dressType = DressType.Shoe;break;
                    case "Accessory":Info.dressType = DressType.Accessory;break;
                }

                string str_applicationType = proArray[8];
                switch(str_applicationType)
                {
                    case "Swordman":Info.applicationType = ApplicationType.Swordman;break;
                    case "Magician":Info.applicationType = ApplicationType.Magician;break;
                    case "Commmon":Info.applicationType = ApplicationType.Commmon;break;
                }
            }

            ObjectInfoDic.Add(ID, Info);
        }
    }
}

//ID           0
//名称           1
//icon名称   2
//类型     3
//加血量值     4
//加魔法值   5
//出售价       6
//购买    7

public enum ObjectType
{
    Drug,
    Equip,
    Mat
}

public enum DressType
{
    Headgear,
    Armor,
    RightHand,
    LeftHand,
    Shoe,
    Accessory
}

public enum ApplicationType
{
    Swordman,
    Magician,
    Commmon
}

public class ObjectInfo
{
    public int ID;
    public string name;
    public string icon_name;
    public ObjectType type;
    public int hp;
    public int mp;
    public int price_sell;
    public int price_buy;

    public int attack;
    public int def;
    public int speed;
    public DressType dressType;
    public ApplicationType applicationType;
}
