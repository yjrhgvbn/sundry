using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {        
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == Tags.islandCube)
                {

                }
            }
        }
	}

    private void TestReadXml()
    {
        XmlDocument xml = new XmlDocument();
        xml.Load(Application.dataPath + "/Script/island/EvenTest.xml");
        //xml.Load("C:/Users/13414/Desktop/学习/EvenTest.xml");
        XmlNodeList ForestNodes = xml.SelectSingleNode("Forest").ChildNodes;
        foreach (XmlElement Event in ForestNodes)
        {
            print("Event Text:" + Event.GetAttribute("Text"));
            print("Event ID:" + Event.GetAttribute("ID"));            
            foreach (XmlElement Choice in Event.SelectNodes("Choice"))
            {
                print("Choice Text:" + Choice.GetAttribute("Text"));
                print("Choice ID:" + Choice.GetAttribute("ID"));
                foreach (XmlElement Result in Choice.SelectNodes("Result"))
                {
                    print("Result name:" + Result.Name);
                    print("Result ID:" + Result.GetAttribute("ID"));
                    if (Result.HasAttribute("Reduce"))
                    {
                        print("Reduce:" + Result.GetAttribute("Reduce"));
                    }
                    if (Result.HasAttribute("Increase"))
                    {
                        print("Increase:" + Result.GetAttribute("Increase"));
                    }
                }            
            }
        }
    }
    private void LoadXml()
    {
        //创建xml文档
        XmlDocument xml = new XmlDocument();
        xml.Load("C:/test/Xmldata.xml");
        //得到Object节点下的所有子节点
        XmlNodeList xmlNodeList = xml.SelectSingleNode("Object").ChildNodes;
        foreach (XmlElement xl1 in xmlNodeList)
        {
            if (xl1.GetAttribute("Id") == "1")
            {
                //继续遍历Id为1的节点下的子节点
                foreach (XmlElement xl2 in xl1)
                {
                    //判断是否是Name == Any的
                    if (xl2.GetAttribute("Name") == "Any")
                    {
                        print(xl2.GetAttribute("Name") + " : " + xl2.InnerText);
                    }
                    //判断是否是Task == First的
                    if (xl2.GetAttribute("Task") == "First")
                    {
                        print(xl2.GetAttribute("Task") + " : " + xl2.InnerText);
                    }
                }
            }
        }
        print(xml.OuterXml);
    }
}
