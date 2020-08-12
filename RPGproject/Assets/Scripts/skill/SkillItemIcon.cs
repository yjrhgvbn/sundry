using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillItemIcon : UIDragDropItem {

    private int skillID;

    protected override void OnDragDropStart()
    {
        base.OnDragDropStart();
        skillID = transform.parent.GetComponent<SkillItem>().ID;
        transform.parent = transform.root;
        this.GetComponent<UISprite>().depth = 40;
    }

    protected override void OnDragDropRelease(GameObject surface)
    {
        base.OnDragDropRelease(surface);
        if(surface && surface.tag == Tags.short_cut)
        {
            surface.GetComponent<ShortCutGrid>().SetSkill(skillID);
        }
    }
}
