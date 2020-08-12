using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsInfo : MonoBehaviour {

    public static SkillsInfo _instance;
    public TextAsset skillInfoText;
    private Dictionary<int, SkillInfo> skillInfoDic = new Dictionary<int, SkillInfo>();

    private void Awake()
    {
        _instance = this;
        InitSkillInfoDic();
    }

    void InitSkillInfoDic()
    {
        string text = skillInfoText.text;
        string[] skillInfoArrary = text.Split('\n');
        foreach (string skillInfoStr in skillInfoArrary)
        {
            string[] pa = skillInfoStr.Split(',');
            SkillInfo info = new SkillInfo();
            info.ID = int.Parse(pa[0]);
            info.name = pa[1];
            info.icon_name = pa[2];
            info.des = pa[3];            
            switch (pa[4])
            {
                case "Passive":
                    info.applyType = ApplyType.Passive;
                    break;
                case "Buff":
                    info.applyType = ApplyType.Buff;
                    break;
                case "SingleTarget":
                    info.applyType = ApplyType.SingleTarget;
                    break;
                case "MultiTarget":
                    info.applyType = ApplyType.MultiTarget;
                    break;
            }            
            switch(pa[5])
            {
                case "Attack":
                    info.applyProperty = ApplyProperty.Attack;
                    break;
                case "Def":
                    info.applyProperty = ApplyProperty.Def;
                    break;
                case "Speed":
                    info.applyProperty = ApplyProperty.Speed;
                    break;
                case "AttackSpeed":
                    info.applyProperty = ApplyProperty.AttackSpeed;
                    break;
                case "HP":
                    info.applyProperty = ApplyProperty.Hp;
                    break;
                case "MP":
                    info.applyProperty = ApplyProperty.Mp;
                    break;
            }
            info.applyValue = int.Parse(pa[6]);
            info.applyTime = int.Parse(pa[7]);
            info.mp = int.Parse(pa[8]);
            info.coldTime = int.Parse(pa[9]);
            switch(pa[10])
            {
                case "Swordman":
                    info.applicableRole = ApplicableRole.Swordman;
                    break;
                case "Magician":
                    info.applicableRole = ApplicableRole.Magician;
                    break;
            }
            info.level = int.Parse(pa[11]);
            switch (pa[12])
            {
                case "Self":
                    info.releaseType = ReleaseType.Self;
                    break;
                case "Enemy":
                    info.releaseType = ReleaseType.Enemy;
                    break;
                case "Position":
                    info.releaseType = ReleaseType.Position;
                    break;
            }
            info.distance = float.Parse(pa[13]);
            info.effectName = pa[14];
            info.aniname = pa[15];
            info.anitime = float.Parse(pa[16]);
            skillInfoDic.Add(info.ID, info);
        }
    }

    //根据ID查找技能
    public SkillInfo GetSkillInfoByID(int ID)
    {
        SkillInfo info = new SkillInfo();
        skillInfoDic.TryGetValue(ID, out info);
        return info;
    }

}

//适用角色
public enum ApplicableRole
{
    Swordman,
    Magician
}

//适用类型
public enum ApplyType
{
    Passive,
    Buff,
    SingleTarget,
    MultiTarget
}

//作用属性
public enum ApplyProperty
{
    Attack,
    Def,
    Speed,
    AttackSpeed,
    Hp,
    Mp
}

//释放类型
public enum ReleaseType
{
    Self,
    Enemy,
    Position
}

public class SkillInfo
{
    public int ID;
    public string name;
    public string icon_name;
    public string des;
    public ApplyType applyType;
    public ApplyProperty applyProperty;
    public int applyValue;
    public int applyTime;
    public int mp;
    public int coldTime;
    public ApplicableRole applicableRole;
    public int level;
    public ReleaseType releaseType;
    public float distance;
    public string effectName;
    public string aniname;
    public float anitime = 0f;

}
