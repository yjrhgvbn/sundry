using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HeroType{
    Swordman,
    Magician,
}

public class PlayerStates : MonoBehaviour {

    public HeroType herotype = HeroType.Magician;

    public int level = 1;//100+level*30
    public string playerName = "默认名称";
    public int hp = 100;
    public float hp_remain = 100f;
    public int mp = 100;
    public float mp_remain = 100f;
    public int coin = 200;
    public float exp = 0f;//当前获得的经验值
    
    public float attack = 20;
    public int attack_plus = 0;
    public float def = 20;
    public int def_plus = 0;
    public float speed = 20;
    public int speed_plus = 0;

    public int point_remian = 0;

    private void Start()
    {
        GetExp(0);
    }

    public void GetDrug(int hp, int mp)
    {
        hp_remain += hp;
        mp_remain += mp;
        if(hp_remain>this.hp)
        {
            hp_remain = this.hp;
        }
        if(mp_remain>this.mp)
        {
            mp_remain = this.mp;
        }
        HeadStatusUI._instance.UpdateShow();
    }

    public bool GetPoint(int point = 1)
    {
        if(point_remian>=point)
        {
            point_remian -= point;
            return true;           
        }
        return false;
    }

    public void GetExp(float exp)
    {
        this.exp += exp;
        while (this.exp >= level * 30 + 100)
        {
            this.exp -= level * 30 + 100;
            level++;
        }

        ExpBar._instance.SetValue(this.exp / (level * 30 + 100));
    }

    public bool TakeMp(int count)
    {
        if(mp_remain >= count)
        {
            mp_remain -= count;
            HeadStatusUI._instance.UpdateShow();
            return true;
        }
        return false;
    }
}
