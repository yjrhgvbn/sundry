using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ControllWalkState
{
    Moving,
    Idle
}
public class PlayerMove : MonoBehaviour {

    public float speed = 1f;
    public ControllWalkState state = ControllWalkState.Idle;
    private PlayerDir dir;
    private CharacterController controller;
    public bool isMoving = false;
    private PlayerAttack attack;

    private void Start()
    {
        dir = this.GetComponent<PlayerDir>();
        controller = GetComponent<CharacterController>();
        attack = this.GetComponent<PlayerAttack>();
    }

    // Update is called once per frame
    void Update () {
        if (attack.state == PlayerState.ControlWalk)
        {
            float distance = Vector3.Distance(dir.targetPosition, transform.position);
            if (distance > 0.1f)
            {
                isMoving = true;
                controller.SimpleMove(transform.forward * speed);
                state = ControllWalkState.Moving;
            }
            else
            {
                isMoving = false;
                state = ControllWalkState.Idle;
            }
        }
	}

    public void SimpleMove(Vector3 targetPos)
    {
        transform.LookAt(targetPos);
        controller.SimpleMove(transform.forward * speed);
    }

}
