using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class MiniMapDestination
{
    public Vector3 destination;
    public Color IconColor;
}

public class MiniCamera : MonoBehaviour {

    public Transform target;
    public GameObject IndicatePerfab;

    private GameObject MiniMap;
    private List<MiniMapDestination> miniDestinations = new List<MiniMapDestination>();//>150小图标
    private Dictionary<MiniMapDestination, GameObject> desIcon = new Dictionary<MiniMapDestination, GameObject>();

    private void Start()
    {
        MiniMap = GameObject.Find("/Canvas/MiniMap");
    }

    private void Update()
    {
        transform.position = new Vector3(0f, 400f, 0f) + target.transform.position;
        DirIconControll();               
    }

    private void DirIconControll()
    {
        foreach (var miniDestination in miniDestinations)
        {
            //当船与目标地不在小地图内时，在小地图旁边创建位置指示符号            
            GameObject go = null;
            if (Vector3.Distance(target.position, miniDestination.destination) > 150)
            {               
                Vector3 t_dir = miniDestination.destination - target.position;
                Vector3 dir = new Vector3(t_dir.z, -t_dir.x, 0f).normalized;
                if (!desIcon.ContainsKey(miniDestination))
                {                                   
                    go = Instantiate(IndicatePerfab, MiniMap.transform);
                    go.GetComponent<RawImage>().color = miniDestination.IconColor;
                    desIcon.Add(miniDestination, go);
                }
                else
                {
                    go = desIcon[miniDestination];
                }
                go.GetComponent<RectTransform>().localPosition = dir * 45;
                float angle = Vector3.Angle(Vector3.right, dir);
                Vector3 normal = Vector3.Cross(Vector3.right, dir);
                angle *= Mathf.Sign(Vector3.Dot(normal, Vector3.forward));
                go.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0f, 0f, angle);           
            }
            else
            {
                if (desIcon.ContainsKey(miniDestination))
                {
                    Destroy(desIcon[miniDestination]);
                    desIcon.Remove(miniDestination);
                }
            }
        }
    }

    public void AddTaskTarget(Vector3 point, Color color)
    {
        //添加小地图的指示显示
        MiniMapDestination ToAdd = new MiniMapDestination();
        ToAdd.destination = point;
        ToAdd.IconColor = color;
        miniDestinations.Add(ToAdd);
    }

    public void RemoveTaskTarget(Vector3 point)
    {
        //按位置删除任务小地图显示
        MiniMapDestination ToRemove = null;
        foreach(var d in miniDestinations)
        {
            if (d.destination == point)
            {
                ToRemove = d;
                break;
            }
        }
        if (ToRemove != null)
        {
            Destroy(desIcon[ToRemove]);
            desIcon.Remove(ToRemove);
        }    
    }
}
