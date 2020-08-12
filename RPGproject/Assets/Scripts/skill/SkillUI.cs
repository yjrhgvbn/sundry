using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUI : MonoBehaviour {

    public static SkillUI _instance;
    private TweenPosition tween;
    private bool isShow = false;
    private PlayerStates ps;

    public UIGrid UIGrid;
    public GameObject skillItemPerfab;
    public int[] magicianSkillIdLisdt;
    public int[] swordmanSkillIdList;

    private void Awake()
    {
        _instance = this;
        tween = this.GetComponent<TweenPosition>();
    }

    private void Start()
    {
        ps = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerStates>();
        int[] idList = null;
        switch(ps.herotype)
        {
            case HeroType.Magician:
                idList = magicianSkillIdLisdt;
                break;
            case HeroType.Swordman:
                idList = swordmanSkillIdList;
                break;
        }

        foreach(int id in idList)
        {
            GameObject itemGo = NGUITools.AddChild(UIGrid.gameObject, skillItemPerfab);
            UIGrid.AddChild(itemGo.transform);
            itemGo.GetComponent<SkillItem>().SetID(id);
        }
    }

    public void TransformState()
    {
        if (isShow)
        {
            tween.PlayReverse();
            isShow = false;
        }
        else
        {
            tween.PlayForward();
            isShow = true;
            UpdateShow();
        }
    }

    void UpdateShow()
    {
        SkillItem[] items = this.GetComponentsInChildren<SkillItem>();
        foreach(SkillItem item in items)
        {
            item.UpdateShow(ps.level);
        }
    }
}
