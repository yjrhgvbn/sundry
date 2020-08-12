using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillItem : MonoBehaviour {

    public int ID;
    private SkillInfo info;
    private UISprite iconName_sprite;
    private UILabel name_label;
    private UILabel applyType_label;
    private UILabel des_label;
    private UILabel mp_label;

    private GameObject icon_mask;

    void InitProperty()
    {
        iconName_sprite = transform.Find("icon_name").GetComponent<UISprite>();
        name_label = transform.Find("property/name_bg/name").GetComponent<UILabel>();
        applyType_label = transform.Find("property/applyType_bg/applyType").GetComponent<UILabel>();
        des_label = transform.Find("property/des_bg/des").GetComponent<UILabel>();
        mp_label = transform.Find("property/MP_bg/MP").GetComponent<UILabel>();

        icon_mask = transform.Find("icon_mask").gameObject;
        icon_mask.SetActive(false);
    }

    public void UpdateShow(int level)
    {
        if (info.level <= level)//表示技能可用
        {
            icon_mask.SetActive(false);
            iconName_sprite.GetComponent<SkillItemIcon>().enabled = true;
        }
        else
        {
            icon_mask.SetActive(true);
            iconName_sprite.GetComponent<SkillItemIcon>().enabled = false;
        }
    }

    public void SetID(int ID)
    {
        InitProperty();
        this.ID = ID;
        info = SkillsInfo._instance.GetSkillInfoByID(ID);
        iconName_sprite.spriteName = info.icon_name;
        name_label.text = info.name;
        switch(info.applyType)
        {
            case ApplyType.Passive:
                applyType_label.text = "增益";
                break;
            case ApplyType.Buff:
                applyType_label.text = "增强";
                break;
            case ApplyType.SingleTarget:
                applyType_label.text = "单个目标";
                break;
            case ApplyType.MultiTarget:
                applyType_label.text = "群体技能";
                break;
        }
        des_label.text = info.des;
        mp_label.text = info.mp + "MP";
    }

}
