using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionBar : MonoBehaviour {

    public void OnStatusClick()
    {
        Status._instance.TransFormStatus();
    }
    public void OnBagClick()
    {
        inventory._instance.TransformState();
    }
    public void OnEqipClick()
    {
        EquipmentUI._instance.TransformState();
    }
    public void OnSkillClick()
    {
        SkillUI._instance.TransformState();
    }
    public void ONSettingClick()
    {
    }
}
