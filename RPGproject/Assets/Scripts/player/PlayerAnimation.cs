using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {

    private PlayerMove move;
    private PlayerAttack attack;
	// Use this for initialization
	void Start () {
        move = GetComponent<PlayerMove>();
        attack = this.GetComponent<PlayerAttack>();
	}
	
	// Update is called once per frame
	void LateUpdate () {
        if (attack.state == PlayerState.ControlWalk)
        {
            if (move.state == ControllWalkState.Moving)
            {
                PlayAnim("Run");
            }
            else if (move.state == ControllWalkState.Idle)
            {
                PlayAnim("Idle");
            }
        }
        else if(attack.state == PlayerState.NormalAttack)
        {
            if(attack.attack_state == AttackStae.Moving)
            {
                PlayAnim("Run");
            }
        }
	}

    void PlayAnim(string animName)
    {
        GetComponent<Animation>().CrossFade(animName);
    }
}
