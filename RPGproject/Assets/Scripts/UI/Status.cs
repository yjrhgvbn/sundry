using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour {

    public static Status _instance;

    private TweenPosition tween;
    private bool isShow = false;
    private UILabel attackLabel;
    private UILabel defLabel;
    private UILabel speedLabel;
    private UILabel pointRemainLabel;
    private UILabel SummaryLabel;

    private GameObject attackButton;
    private GameObject defButton;
    private GameObject speedBtton;

    private PlayerStates playerState;

    private void Awake()
    {
        _instance = this;
        tween = GetComponent<TweenPosition>();

        attackLabel = transform.Find("attack").GetComponent<UILabel>();
        defLabel = transform.Find("def").GetComponent<UILabel>();
        speedLabel = transform.Find("speed").GetComponent<UILabel>();
        pointRemainLabel = transform.Find("point_remian").GetComponent<UILabel>();
        SummaryLabel = transform.Find("summary").GetComponent<UILabel>();
        attackButton = transform.Find("attack_plus").gameObject;
        defButton = transform.Find("def_plus").gameObject;
        speedBtton = transform.Find("speed_plus").gameObject;
        playerState = GameObject.FindWithTag(Tags.player).GetComponent<PlayerStates>();
    }

   public void TransFormStatus()
    {
        if(!isShow)
        {
            UpdateShow();
            tween.PlayForward();
            isShow = true;
        }
        else
        {
            tween.PlayReverse();
            isShow = false;
        }
    }

    void UpdateShow()
    {
        attackLabel.text = playerState.attack + "+" + playerState.attack_plus;
        defLabel.text = playerState.def + "+" + playerState.def_plus;
        speedLabel.text = playerState.speed + "+" + playerState.speed_plus;
        pointRemainLabel.text = playerState.point_remian.ToString();
        SummaryLabel.text = "伤害;" + (playerState.attack + playerState.attack_plus) + " " + "防御:" + (playerState.def + playerState.def_plus) + " " + "速度;" + (playerState.speed + playerState.speed_plus);

        if (playerState.point_remian > 0)
        {
            attackButton.SetActive(true);
            defButton.SetActive(true);
            speedBtton.SetActive(true);
        }
        else
        {
            attackButton.SetActive(false);
            defButton.SetActive(false);
            speedBtton.SetActive(false);
        }
    }

    public void OnAttackPlusClick()
    {
        if (playerState.GetPoint())
            playerState.attack_plus++;
    }

    public void OnDefPlusClick()
    {
        if (playerState.GetPoint())
            playerState.def_plus++;
    }
    public void OnSpeedPlusClick()
    {
        if (playerState.GetPoint())
            playerState.speed_plus++;
    }
}
