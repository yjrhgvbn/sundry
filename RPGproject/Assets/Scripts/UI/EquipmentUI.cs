using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentUI : MonoBehaviour {

    public static EquipmentUI _instance;

    public GameObject equipmentItem;

    private TweenPosition tween;
    private bool isShow = false;

    private GameObject headgear;
    private GameObject armor;
    private GameObject rightHand;
    private GameObject leftHand;
    private GameObject shoe;
    private GameObject accessory;

    private PlayerStates ps;

    public int attack = 0;
    public int def = 0;
    public int speed = 0;

    private void Awake()
    {
        _instance = this;
        tween = this.GetComponent<TweenPosition>();

        headgear = transform.Find("headgear").gameObject;
        armor = transform.Find("armor").gameObject;
        rightHand = transform.Find("right_hand").gameObject;
        leftHand = transform.Find("left_hand").gameObject;
        shoe = transform.Find("shoe").gameObject;
        accessory = transform.Find("accessory").gameObject;

    }

    private void Start()
    {
        
        ps = GameObject.FindWithTag(Tags.player).GetComponent<PlayerStates>();
    }
    public void TransformState()
    {
        if(!isShow)
        {
            isShow = true;
            tween.PlayForward();
        }
        else
        {
            isShow = false;
            tween.PlayReverse();
        }
    }

    public bool Dress(int ID)  //穿上装备
    {
        ObjectInfo info = ObjectsInfo._instance.GetObjectByID(ID);
        if (info.type != ObjectType.Equip)
            return false;
        if (ps.herotype == HeroType.Magician)
        {
            if (info.applicationType == ApplicationType.Swordman)
                return false;
        }
        if (ps.herotype == HeroType.Swordman)
        {
            if (info.applicationType == ApplicationType.Magician)
                return false;
        }

        GameObject parent = null;
        switch(info.dressType)
        {
            case DressType.Headgear:
                parent = headgear;
                break;
            case DressType.Armor:
                parent = armor;
                break;
            case DressType.RightHand:
                parent = rightHand;
                break;
            case DressType.LeftHand:
                parent = leftHand;
                break;
            case DressType.Shoe:
                parent = shoe;
                break;
            case DressType.Accessory:
                parent = accessory;
                break;
        }

        EquipmentItem item = parent.GetComponentInChildren<EquipmentItem>();
        if (item) //判断有没有穿戴同类型的装备
        {
            inventory._instance.GetID(item.ID);//卸下当前装备
            item.SetInfo(info);
        }
        else
        {
            GameObject itemGo = NGUITools.AddChild(parent, equipmentItem);
            itemGo.transform.localPosition = Vector3.zero;
            itemGo.GetComponent<EquipmentItem>().SetInfo(info);
            itemGo.GetComponent<UISprite>().depth = 22;

        }
        UpdateProperty();
        return true;
    }

    public void TakeOff(int ID, GameObject gameObject)
    {
        inventory._instance.GetID(ID);
        GameObject.Destroy(gameObject);
        UpdateProperty();
    }

    void UpdateProperty()
    {
        this.attack = 0;
        this.def = 0;
        this.speed = 0;
        EquipmentItem Item = headgear.GetComponentInChildren<EquipmentItem>();    
        PlusPropety(Item);
        Item = leftHand.GetComponentInChildren<EquipmentItem>();
        PlusPropety(Item);
        Item = armor.GetComponentInChildren<EquipmentItem>();
        PlusPropety(Item);
        Item = rightHand.GetComponentInChildren<EquipmentItem>();
        PlusPropety(Item);
        Item = shoe.GetComponentInChildren<EquipmentItem>();
        PlusPropety(Item);
        Item = accessory.GetComponentInChildren<EquipmentItem>();
        PlusPropety(Item);


    }
    void PlusPropety(EquipmentItem item)
    {
        if (item)
        {
            ObjectInfo info = ObjectsInfo._instance.GetObjectByID(item.ID);
            this.attack += info.attack;
            this.def = info.def;
            this.speed += info.speed;
        }
    }
}
