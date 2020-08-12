using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WolfState
{
    Idle,
    Walk,
    Attack,
    Death
}


public class WolfBaby : MonoBehaviour {

    public WolfState state = WolfState.Idle;

    public string animation_now;
    public string animation_death;
    public string animation_idle;
    public string animation_walk;
    public float time = 1f;
    private float timer = 0f;
    public float speed = 1f;    

    public AudioClip miss_sound;

    public int hp = 100;
    public int exp = 20;
    public float miss_rate = 0.2f;
    public int attack = 10;

    private CharacterController cc;   
    private Color normal;   
    public float red_time = 1f;//显示被击中的时间   

    private GameObject hudTextFollow;
    private GameObject hudTextGo;
    public GameObject hudTextPrefab;

    private HUDText hudText;
    private UIFollowTarget followTarget;
    public GameObject body;

    public string animation_normalAttack;
    public float time_normal_attack;
    public string animation_crazyAttack;
    public float time_crazy_attack;
    public float crazy_attack_rate;

    public string animation_attackNow;
    public float attack_rate = 1;          //攻击速率 每秒
    private float attack_timer;

    public Transform target;
    public float minDistance = 2f;
    public float maxDistance = 5f;

    public WolfBabySpawn spawn;
    private PlayerStates ps;

    private Transform localPos;
    private float MaxDis = 5f;

    private void Awake()
    {        
        normal = body.GetComponent<Renderer>().material.color;
        animation_now = animation_idle;
        cc = this.GetComponent<CharacterController>();
        hudTextFollow = transform.Find("HUDText").gameObject;
    }

    private void Start()
    {
        localPos = this.transform;
        hudTextGo = NGUITools.AddChild(HUDTextParent._instance.gameObject, hudTextPrefab);
        hudText = hudTextGo.GetComponent<HUDText>();
        followTarget = hudTextGo.GetComponent<UIFollowTarget>();
        followTarget.target = hudTextFollow.transform;
        followTarget.gameCamera = Camera.main;
        ps = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerStates>();
       // followTarget.uiCamera = UICamera.current.gameObject.GetComponent<Camera>();
    }

    private void Update()
    {
        if (state == WolfState.Death)
        {
            this.GetComponent<Animation>().CrossFade(animation_death);
        }
        else if (state == WolfState.Attack)                                 //攻击
        {
            AutoAttack();
        }
        else //巡逻
        {                           
            this.GetComponent<Animation>().CrossFade(animation_now);
            if(animation_now == animation_walk)
            {
                cc.SimpleMove(transform.forward * speed);
            }
            timer += Time.deltaTime;
            if(timer>=time)
            {
                timer = 0;
                RandomState();
            }
        }
    }

    private void OnMouseEnter()
    {
        if (!PlayerAttack._instance.isLockingTarget)
            CursorManager._instance.SetAttack();
    }
    private void OnMouseExit()
    {
        if (!PlayerAttack._instance.isLockingTarget)
            CursorManager._instance.SetNormal();
    }

    void RandomState()
    {
        if (Vector3.Distance(localPos.position, transform.position) > MaxDis)
        {
            transform.LookAt(localPos);
            animation_now = animation_walk;
            return;
        }
        int value = Random.Range(0, 2);
        if(value == 0)
        {
            animation_now = animation_idle;
        }
        else
        {
            if (animation_now != animation_walk)
            {
                transform.Rotate(transform.up * Random.Range(0, 361));
            }

            animation_now = animation_walk;
        }
    }

    public void TakeDamage(int attack)//受到伤害
    {
        if (state == WolfState.Death)
            return;
        target = GameObject.FindGameObjectWithTag(Tags.player).transform;
        state = WolfState.Attack;
        float value = Random.Range(0f, 1f);
        if (value < miss_rate)                  //miss
        {
            AudioSource.PlayClipAtPoint(miss_sound, transform.position);
            hudText.Add("Miss", Color.gray, 1f);
        }
        else     //打中
        {
            hudText.Add("-" + attack, Color.red, 1f);
            this.hp -= attack;
            StartCoroutine(ShowBodyRed());
            if(this.hp<=0)
            {
                state = WolfState.Death;
                Destroy(this.gameObject, 2f);
            }
        }
    }

    IEnumerator ShowBodyRed()
    {
        body.GetComponent<Renderer>().material.color = Color.red;
        yield return new WaitForSeconds(1f);
        body.GetComponent<Renderer>().material.color = normal;
    }

    private void OnDestroy()
    {
        spawn.MinusNumber();
        ps.GetExp(this.exp);
        BarNPC._instance.OnKillWolf();
        GameObject.Destroy(hudTextGo);
    }

    void AutoAttack()
    {
        if(target!=null)
        {
            if(target.GetComponent<PlayerAttack>().state == PlayerState.Death)
            {
                target = null;
                state = WolfState.Idle;
                return;
            }
            float distance = Vector3.Distance(target.position, transform.position);
            if(distance > maxDistance)//停止自动攻击
            {
                target = null;
                state = WolfState.Idle;
            }
            else if(distance<=minDistance)//自动攻击
            {
                attack_timer += Time.deltaTime;
                GetComponent<Animation>().CrossFade(animation_attackNow);
                if(animation_attackNow == animation_normalAttack)
                {
                    if(attack_timer > time_normal_attack)
                    {
                        //产生伤害 
                        target.GetComponent<PlayerAttack>().TakeDamage(attack);
                        animation_attackNow = animation_idle;
                    }
                }
                else if(animation_attackNow == animation_crazyAttack)
                {
                    if(attack_timer>time_crazy_attack)
                    {
                        //
                        target.GetComponent<PlayerAttack>().TakeDamage(attack);
                        animation_attackNow = animation_idle;
                    }
                }

                if(attack_timer>(1f/attack_rate))//再次进行攻击
                {
                    RandonAttack();
                    attack_timer = 0f;
                }
            }
            else     //朝目标移动
            {
                transform.LookAt(target);
                cc.SimpleMove(transform.forward * speed);
                GetComponent<Animation>().CrossFade(animation_walk);
            }
        }
        else
        {
            state = WolfState.Idle;
        }
    }

    void RandonAttack()
    {
        float value = Random.Range(0f, 1f);
        if (value < crazy_attack_rate)
            animation_attackNow = animation_crazyAttack;
        else
            animation_attackNow = animation_normalAttack;
    }

}
