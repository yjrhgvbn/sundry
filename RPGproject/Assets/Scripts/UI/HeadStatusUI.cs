using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadStatusUI : MonoBehaviour {

    public static HeadStatusUI _instance;

    private UILabel Name;
    private UISlider hpBar;
    private UISlider mpBar;

    private UILabel hpLabel;
    private UILabel mpLabel;
    private PlayerStates ps;

    private void Awake()
    {
        _instance = this;
        Name = transform.Find("name").GetComponent<UILabel>();
        hpBar = transform.Find("hp").GetComponent<UISlider>();
        mpBar = transform.Find("mp").GetComponent<UISlider>();

        hpLabel = transform.Find("hp/Thumb/Label").GetComponent<UILabel>();
        mpLabel = transform.Find("mp/Thumb/Label").GetComponent<UILabel>();
       
    }

    private void Start()
    {
        ps = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerStates>();
        UpdateShow();
    }

    public void UpdateShow()
    {
        Name.text = "LV." + ps.level + " " + ps.playerName;
        hpBar.value = ps.hp_remain / ps.hp;
        mpBar.value = ps.mp_remain / ps.mp;

        hpLabel.text = ps.hp_remain + "/" + ps.hp;
        mpLabel.text = ps.mp_remain + "/" + ps.mp;
    }   
}
