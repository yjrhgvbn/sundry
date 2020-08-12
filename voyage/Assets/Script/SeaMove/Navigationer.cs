using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Navigationer : MonoBehaviour
{

    public NavMeshAgent agent;
    public Vector3 target;

    private bool IsToLand = false;
    private int LoadID;

    // Use this for initialization
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        Camera.main.transform.position = new Vector3(transform.position.x, Camera.main.transform.position.y, transform.position.z);
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == Tags.land)
                {
                    target = hit.collider.GetComponent<IslandTag>().BerthPoint;
                    IsToLand = true;
                    LoadID = hit.collider.GetComponent<IslandTag>().ScenId;
                }
                else
                {
                    target = hit.point;
                    IsToLand = false;
                }
            }
        }
        if (agent != null)
        {
            agent.SetDestination(target);
        }
        if(IsToLand && agent.isStopped)
        {
            //根据岛ID加载场景
            //***TODO           
        }
    }
}
