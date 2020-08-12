using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShortCutType
{
    Skill,
    Drug,
    None
}

public class ShortCutGrid : MonoBehaviour {

    public KeyCode KeyCode;
    private UISprite icon;
    private ShortCutType type = ShortCutType.None;
    private int id;
    private SkillInfo skillInfo;
    private ObjectInfo objectInfo;
    private PlayerStates playerState;
    private PlayerAttack playerAttack;

    private void Awake()
    {
        icon = transform.Find("icon").GetComponent<UISprite>();
        icon.gameObject.SetActive(false);
    }

    private void Start()
    {
        playerState = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerStates>();
        playerAttack = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerAttack>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode))
        {
            if(type == ShortCutType.Drug)
            {
                OnDrugUse();
            }
            else if(type == ShortCutType.Skill)
            {
                if(!playerState.TakeMp(skillInfo.mp))
                {

                }
                else
                {
                    playerAttack.UseSkill(skillInfo);
                }
            }            
        }
    }

    public void SetSkill(int ID)
    {
        this.id = ID;
        this.skillInfo = SkillsInfo._instance.GetSkillInfoByID(ID);
        icon.gameObject.SetActive(true);
        icon.spriteName = skillInfo.icon_name;
        type = ShortCutType.Skill;
    }

    public void SetInventory(int ID)
    {
        this.id = ID;
        objectInfo = ObjectsInfo._instance.GetObjectByID(ID);
        if (objectInfo.type == ObjectType.Drug)
        {
            icon.gameObject.SetActive(true);
            icon.spriteName = objectInfo.icon_name;
            type = ShortCutType.Drug;            
        }
    }

    public void OnDrugUse()
    {
        bool success = inventory._instance.MinusId(id,1);      
        if(success)
        {
            playerState.GetDrug(objectInfo.hp, objectInfo.mp);               
        }
        else
        {
            type = ShortCutType.None;
            icon.gameObject.SetActive(false);
            this.id = 0;
            skillInfo = null;
            objectInfo = null;
        }
    }
}
