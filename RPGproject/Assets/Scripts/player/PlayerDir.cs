using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDir : MonoBehaviour {

    public GameObject effectClickPrefab;
    public Vector3 targetPosition = Vector3.zero;
    private bool isMoving = false;//表示鼠标是否按下
    private PlayerMove playerMove;
    private PlayerAttack attack;

    private void Start()
    {
        targetPosition = transform.position;
        playerMove = GetComponent<PlayerMove>();
        attack = this.GetComponent<PlayerAttack>();
    }

    // Update is called once per frame
    void Update () {
        if (attack.state == PlayerState.Death)
            return;
		if(!PlayerAttack._instance.isLockingTarget && Input.GetMouseButtonDown(0) && UICamera.hoveredObject == null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            bool isCollider = Physics.Raycast(ray, out hitInfo);
            if(isCollider && hitInfo.collider.tag == Tags.ground)
            {
                isMoving = true;
                ShowClickEffect(hitInfo.point);
                LookAtTarget(hitInfo.point);
            }
        }
        else if(Input.GetMouseButtonUp(0))
        {
            isMoving = false;
        }

        if(isMoving)
        {
            //得到要移动的位置，让主角朝向目标位置
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            bool isCollider = Physics.Raycast(ray, out hitInfo);
            if (isCollider && hitInfo.collider.tag == Tags.ground)
            {
                LookAtTarget(hitInfo.point);
            }
            
        }
        else if(playerMove.isMoving)
        {
            LookAtTarget(targetPosition);
        }
	}

   //实例化出点击的效果       
    void ShowClickEffect(Vector3 hitPoint)
    {
        hitPoint = new Vector3(hitPoint.x, hitPoint.y + 0.1f, hitPoint.z);
        GameObject.Instantiate(effectClickPrefab, hitPoint, Quaternion.identity);
    }

    void LookAtTarget(Vector3 point)
    {
        targetPosition = new Vector3(point.x, transform.position.y, point.z);
        this.transform.LookAt(targetPosition);
    }
}
