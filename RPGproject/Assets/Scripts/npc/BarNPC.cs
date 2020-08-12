using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarNPC : NPC {

    public static BarNPC _instance;

    public TweenPosition questTween;
    public UILabel UILabel;
    public GameObject acceptButton;
    public GameObject okButton;
    public GameObject cancelButton;

    public bool IsInTask = false;
    public int KillCount = 0;

    private PlayerStates status;
    private GameObject player;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        status = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerStates>();
        player = GameObject.FindGameObjectWithTag(Tags.player);
    }
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && Vector3.Distance(this.transform.position, player.transform.position) <= 3f)
        {
            GetComponent<AudioSource>().Play();
            if (IsInTask)
                ShowTaskProgress();
            else
                ShowTaskDes();
            ShowQuest();
        }
    }

    void ShowQuest()
    {
        questTween.gameObject.SetActive(true);
        questTween.PlayForward();
    }

    //任务的按钮点击处理
    public void OnCloseButtonClik()
    {
        HideQuest();
    }

    void HideQuest()
    {
        questTween.PlayReverse();
        Invoke("QuestDisablse", 0.5f);      
    }
    void QuestDisablse()
    {
        questTween.gameObject.SetActive(false);
    }

    void ShowTaskDes()
    {
        UILabel.text = "任务:\n杀死10只狼\n\n奖励:\n1000金币";
        okButton.SetActive(false);
        acceptButton.SetActive(true);
        cancelButton.SetActive(true);
    }
    void ShowTaskProgress()
    {
        UILabel.text = "任务:\n你已经杀死了" + KillCount + "\\10只狼\n\n奖励:\n1000金币";
        okButton.SetActive(true);
        acceptButton.SetActive(false);
        cancelButton.SetActive(false);
    }

    public void OnAcceptButtonClick()
    {
        ShowTaskProgress();
        IsInTask = true;
    }
    public void OnOkButtonClick()
    {
        if(KillCount>=10)
        {
            inventory._instance.AddCoin(1000);
            KillCount = 0;
            IsInTask = false;
            ShowTaskDes();
        }
        else
        {
            HideQuest();
        }
    }

    public void OnKillWolf(int count =1)
    {
        if (IsInTask)
            KillCount += count;
    }
   
}
