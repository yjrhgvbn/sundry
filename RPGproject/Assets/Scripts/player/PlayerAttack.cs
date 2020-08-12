using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    ControlWalk,
    NormalAttack,
    SkillAttack,
    Death
}
public enum AttackStae      //攻击时的状态
{
    Moving,
    Idle,
    Attack
}

public class PlayerAttack : MonoBehaviour {

    public static PlayerAttack _instance;

    public PlayerState state = PlayerState.ControlWalk;
    public AttackStae attack_state = AttackStae.Idle;

    public string animation_normal_attack;//普攻的动画
    public string animation_idle;
    public string animation_now;
    public float time_normal_attack;//普攻的时间
    public float rate_normal_attack = 1f;
    private float timer = 0f;
    public float min_distance = 5f;
    private Transform target_normal_attack;

    public GameObject effect;

    private PlayerMove move;
    private bool showEffect = false;
    private PlayerStates ps;
    public float miss_rate = 0.25f;
    public GameObject hudTextPrefab;
    private GameObject hudTextGo;
    private HUDText hudText;
    private GameObject hudTextFollow;
    public AudioClip missSound;
    public GameObject body;
    private Color normal;
    UIFollowTarget followTarget;
    
    public string animation_death;

    public GameObject[] efxArray;
    private Dictionary<string, GameObject> efxDic = new Dictionary<string, GameObject>();
    public bool isLockingTarget = false;
    private SkillInfo info = null;

    private void Awake()
    {
        _instance = this;
        normal = body.GetComponent<Renderer>().material.color;
        move = this.GetComponent<PlayerMove>();
        ps = this.GetComponent<PlayerStates>();
        hudTextFollow = transform.Find("HUDText").gameObject;

        foreach(GameObject go in efxArray)
        {
            efxDic.Add(go.name, go);
        }
    }

    private void Start()
    {
        hudTextGo = NGUITools.AddChild(HUDTextParent._instance.gameObject, hudTextPrefab);
        hudText = hudTextGo.GetComponent<HUDText>();
        followTarget = hudTextGo.GetComponent<UIFollowTarget>();
        followTarget.target = hudTextFollow.transform;
        followTarget.gameCamera = Camera.main;
    }

    private void Update()
    {        
        if(!isLockingTarget && Input.GetMouseButtonDown(0) && state != PlayerState.Death)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            bool isCollider = Physics.Raycast(ray, out hitInfo);
            if (isCollider && hitInfo.collider.tag == Tags.enemy)            //点击敌人时
            {
                target_normal_attack = hitInfo.collider.transform;
                state = PlayerState.NormalAttack;
                timer = 0f;
                showEffect = false;
            }
            else
            {
                state = PlayerState.ControlWalk;
                target_normal_attack = null;
            }
        }

        if(state == PlayerState.NormalAttack)
        {
            if (target_normal_attack == null)
            {
                state = PlayerState.ControlWalk;
            }
            else
            {
                float distance = Vector3.Distance(transform.position, target_normal_attack.position);
                if (distance <= min_distance)   //进行攻击
                {
                    transform.LookAt(target_normal_attack.position);
                    attack_state = AttackStae.Attack;
                    timer += Time.deltaTime;
                    GetComponent<Animation>().CrossFade(animation_now);
                    if (timer >= time_normal_attack)
                    {
                        animation_now = animation_idle;
                        if (showEffect == false)                //播放特性
                        {
                            showEffect = true;
                            GameObject.Instantiate(effect, target_normal_attack.position, Quaternion.identity);
                            target_normal_attack.GetComponent<WolfBaby>().TakeDamage(GetAttack());
                        }
                    }
                    if (timer >= (1f / rate_normal_attack))
                    {
                        timer = 0f;
                        showEffect = false;
                        animation_now = animation_normal_attack;
                    }
                }
                else                                //移动到敌人
                {
                    attack_state = AttackStae.Moving;
                    move.SimpleMove(target_normal_attack.position);
                }
            }
        }
        else if(state == PlayerState.Death)
        {
            GetComponent<Animation>().CrossFade(animation_death);
        }

        if(isLockingTarget && Input.GetMouseButtonDown(0))
        {
            OnLockTaget();
        }
    }

    public int GetAttack()
    {
        return (int)(EquipmentUI._instance.attack + ps.attack + ps.attack_plus);
    }

    public void TakeDamage(int attack)
    {
        if (state == PlayerState.Death)
            return;
        float def = EquipmentUI._instance.def + ps.def + ps.def_plus;
        int temp = (int)(attack * ((200 - def) / 200));
        if(temp<1)
        {
            temp = 1;
        }
        float value = Random.Range(0f, 1f);
        if (value < miss_rate)
        {
            AudioSource.PlayClipAtPoint(missSound, transform.position);
            hudText.Add("MISS", Color.gray, 1f);
        }
        else
        {
            hudText.Add("-" + temp, Color.red, 1f);
            ps.hp_remain -= temp;
            StartCoroutine(ShowBodyRed());
            if (ps.hp_remain <= 0)
            {
                state = PlayerState.Death;
            }
        }

        HeadStatusUI._instance.UpdateShow();
    }

    IEnumerator ShowBodyRed()
    {
        body.GetComponent<Renderer>().material.color = Color.red;
        yield return new WaitForSeconds(1f);
        body.GetComponent<Renderer>().material.color = normal;
    }

    private void OnDestroy()
    {
        GameObject.Destroy(hudTextGo);
    }

    public void UseSkill(SkillInfo info)
    {
        if(ps.herotype == HeroType.Magician && info.applicableRole == ApplicableRole.Swordman)                  
                return;                   
        if (ps.herotype == HeroType.Swordman && info.applicableRole == ApplicableRole.Magician)               
                return;

        switch (info.applyType)
        {
            case ApplyType.Passive:
                StartCoroutine(OnPassiveSkillUse(info));
                break;
            case ApplyType.Buff:
                StartCoroutine(OnBuffSkillUse(info));
                break;
            case ApplyType.SingleTarget:
                OnSingleTargetSkillUse(info);
                break;
            case ApplyType.MultiTarget:
                OnMulitTargetSkillUse(info);
                break;
        }
    }

    IEnumerator OnPassiveSkillUse(SkillInfo info)
    {
        state = PlayerState.SkillAttack;
        GetComponent<Animation>().CrossFade(info.aniname);
        yield return new WaitForSeconds(info.anitime);
        state = PlayerState.ControlWalk;
        int hp = 0, mp = 0;       
        if (info.applyProperty == ApplyProperty.Hp)
        {
            hp = info.applyValue;           
        }
        else if (info.applyProperty == ApplyProperty.Mp)
        {
            mp = info.applyValue;           
        }
        ps.GetDrug(hp, mp);
        GameObject perfab = null;
        efxDic.TryGetValue(info.effectName, out perfab);
        GameObject.Instantiate(perfab, transform.position, Quaternion.identity);
    }

    IEnumerator OnBuffSkillUse(SkillInfo info)

    {
        state = PlayerState.SkillAttack;
        GetComponent<Animation>().CrossFade(info.aniname);
        yield return new WaitForSeconds(info.anitime);

        GameObject perfab = null;
        efxDic.TryGetValue(info.effectName, out perfab);
        GameObject.Instantiate(perfab, transform.position, Quaternion.identity);

        state = PlayerState.ControlWalk;
        switch(info.applyProperty)
        {
            case ApplyProperty.Attack:
                ps.attack *= (info.applyValue / 100f);
                break;
            case ApplyProperty.AttackSpeed:
                rate_normal_attack *= (info.applyValue / 100f);
                break;
            case ApplyProperty.Def:
                ps.def *= (info.applyValue / 100f);
                break;
            case ApplyProperty.Speed:
                move.speed *= (info.applyValue / 100f);
                break;
        }
        yield return new WaitForSeconds(info.applyTime);
        switch (info.applyProperty)
        {
            case ApplyProperty.Attack:
                ps.attack /= (info.applyValue / 100f);
                break;
            case ApplyProperty.AttackSpeed:
                rate_normal_attack /= (info.applyValue / 100f);
                break;
            case ApplyProperty.Def:
                ps.def /= (info.applyValue / 100f);
                break;
            case ApplyProperty.Speed:
                move.speed /= (info.applyValue / 100f);
                break;
        }

    }

    void OnSingleTargetSkillUse(SkillInfo info)
    {
        state = PlayerState.SkillAttack;
        CursorManager._instance.SetLockTarget();
        isLockingTarget = true;
        this.info = info;
    }

    void OnLockTaget()
    {
        isLockingTarget = false;
        switch (info.applyType)
        {
            case ApplyType.SingleTarget:
                StartCoroutine(OnLockSingleTaget());
                break;
            case ApplyType.MultiTarget:
                StartCoroutine(OnLockMultiTarget());
                break;
        }
    }

    IEnumerator OnLockSingleTaget()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        bool isCollider = Physics.Raycast(ray, out hitInfo);
        if(isCollider && hitInfo.collider.tag == Tags.enemy)
        {
            GetComponent<Animation>().CrossFade(info.aniname);
            yield return new WaitForSeconds(info.anitime);
            state = PlayerState.ControlWalk;
            GameObject perfab = null;
            efxDic.TryGetValue(info.effectName, out perfab);
            GameObject.Instantiate(perfab, hitInfo.collider.transform.position, Quaternion.identity);

            hitInfo.collider.GetComponent<WolfBaby>().TakeDamage((int)(info.applyValue * (GetAttack() / 100f)));
        }
        else
        {
            state = PlayerState.NormalAttack;
        }
        CursorManager._instance.SetNormal();
    }

    void OnMulitTargetSkillUse(SkillInfo info)
    {
        state = PlayerState.SkillAttack;
        CursorManager._instance.SetLockTarget();
        isLockingTarget = true;
        this.info = info;
    }

    IEnumerator OnLockMultiTarget()
    {
        CursorManager._instance.SetNormal();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        bool isCollider = Physics.Raycast(ray, out hitInfo);
        if (isCollider && hitInfo.collider.tag == Tags.ground)
        {
            GetComponent<Animation>().CrossFade(info.aniname);
            yield return new WaitForSeconds(info.anitime);
            state = PlayerState.ControlWalk;

            GameObject perfab = null;
            efxDic.TryGetValue(info.effectName, out perfab);
            GameObject go = GameObject.Instantiate(perfab, hitInfo.point + Vector3.up * 0.5f, Quaternion.identity);
            go.GetComponent<MagicSphere>().attack = (int)(GetAttack() * (info.applyValue / 100f));
        }
        else
        {
            state = PlayerState.NormalAttack;
        }
        
    }
}

