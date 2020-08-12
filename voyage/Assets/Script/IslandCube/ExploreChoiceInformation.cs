using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CubeTagEvent
{
    public int ID;
    public string text;
    public float rate;
    public List<Choice> choices = new List<Choice>();
    public class Choice
    {
        public int ID;
        public string text;
        public List<Result> results = new List<Result>();
        public class Result
        {
            public int ID;
            public string acquire = null;
            public string text;
            public float rate;
        }
    }
}

public enum IslandCubeType
{
    Forest
}

public class ExploreChoiceInformation : MonoBehaviour {

    public GameObject ReslutInfo;
    public GameObject ChoiceButton;

    private GameObject EventCanvas;
    private GameObject ResultCanvas;
    private Dictionary<IslandCubeType, List<CubeTagEvent>> ExploreEvents = new Dictionary<IslandCubeType, List<CubeTagEvent>>();

    private void Awake()
    {
        LoadEven();
        EventCanvas = transform.GetChild(2).gameObject;
        ResultCanvas = transform.GetChild(3).gameObject;        
    }

    public void NormalExploreEvent(IslandCubeType type)
    {
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);
        EventCanvas.SetActive(true);
        ResultCanvas.SetActive(false);
        CubeTagEvent Event = new CubeTagEvent();
        float randomDice = Random.Range(0f, 1f);
        foreach (CubeTagEvent e in ExploreEvents[type])
        {
            if (randomDice < e.rate)
            {
                Event = e;
                break;
            }
            randomDice -= e.rate;
        }
        EventCanvas.GetComponentInChildren<Text>().text = Event.text;
        Vector3 pos = new Vector3(250f, -40f, 0f);
        foreach (CubeTagEvent.Choice choice in Event.choices)
        {
            GameObject go = Canvas.Instantiate(ChoiceButton, EventCanvas.transform);
            go.GetComponent<RectTransform>().localPosition = pos;
            go.transform.Find("Text").GetComponent<Text>().text = choice.text;
            go.GetComponent<Button>().onClick.AddListener(() => ResultAct(choice));
            go.name = "ChoiceButton";
            pos -= new Vector3(0, 50, 0);
        }
    }   

    public void ResultAct(CubeTagEvent.Choice choice)
    {
        //显示结果UI
        //删除事件信息
        while (true)
        {
            Transform temp = EventCanvas.transform.Find("ChoiceButton");
            if (temp == null)
                break;
            temp.SetParent(null);
            Destroy(temp.gameObject);
        }
        EventCanvas.SetActive(false);
        ResultCanvas.SetActive(true);
        //随机结果
        float randomDice = Random.Range(0f, 1f);
        CubeTagEvent.Choice.Result result = new CubeTagEvent.Choice.Result();
        foreach (CubeTagEvent.Choice.Result r in choice.results)
        {
            if (randomDice < r.rate)
            {
                result = r;
                break;
            }
            randomDice -= r.rate;
        }
        //显示UI        
        ResultCanvas.GetComponentInChildren<Text>().text = result.text;
        Vector3 pos = new Vector3(120, -80, 0);
        if (result.acquire != null)
        {
            foreach (string acquire in result.acquire.Split(';'))
            {
                string[] tempStr = acquire.Split(':');
                string acquireName = tempStr[0];
                int acquireNumber = int.Parse(tempStr[1]);
                GameObject go = Canvas.Instantiate(ReslutInfo, ResultCanvas.transform);
                go.GetComponent<RectTransform>().localPosition = pos;
                string iconName = Article._instance.GetArticleByName(acquireName).iconName;
                if (iconName != null)
                    go.GetComponent<Image>().sprite = Resources.Load<Sprite>(ResourcePath.ArticleIconPath + iconName);
                else
                    print("can't find" + acquireName + "icon");
                go.name = "RseultInfo";
                string Symbol = " +";
                if (acquireNumber < 0)
                    Symbol = " ";
                go.transform.GetComponentInChildren<Text>().text = acquireName + Symbol + acquireNumber.ToString();
                pos -= new Vector3(0f, 40f, 0f);
            }
        }
        //任意左击继续功能
        GameObject gameObject = Canvas.Instantiate(ChoiceButton, ResultCanvas.transform);
        Destroy(gameObject.transform.GetChild(0).gameObject);
        gameObject.transform.DetachChildren();
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
        gameObject.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
        gameObject.GetComponent<Button>().onClick.AddListener(() => Clear());
        gameObject.name = "RseultInfo";
    }

    public void Clear()
    {
        while (true)
        {
            Transform temp = ResultCanvas.transform.Find("RseultInfo");
            if (temp == null)
                break;
            temp.SetParent(null);
            Destroy(temp.gameObject);
        }
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void LoadEven()
    {        
        //加载各种选项
        XmlDocument Xml = new XmlDocument();
        Xml.Load(Application.dataPath + "/Script/island/EvenTest.xml");
        foreach(IslandCubeType enumName in Enum.GetValues(typeof(IslandCubeType)))
        {
            ExploreEvents.Add(enumName, new List<CubeTagEvent>());
            XmlNodeList Events = Xml.SelectSingleNode(enumName.ToString()).ChildNodes;
            foreach (XmlElement Event in Events)
            {
                CubeTagEvent tempTagEvent = new CubeTagEvent
                {
                    text = Event.GetAttribute("Text"),
                    ID = int.Parse(Event.GetAttribute("ID")),
                    rate = float.Parse(Event.GetAttribute("Rate"))
                };
                foreach (XmlElement choice in Event.SelectNodes("Choice"))
                {
                    CubeTagEvent.Choice tempChoice = new CubeTagEvent.Choice
                    {
                        text = choice.GetAttribute("Text"),
                        ID = int.Parse(choice.GetAttribute("ID"))
                    };
                    foreach (XmlElement result in choice.SelectNodes("Result"))
                    {
                        CubeTagEvent.Choice.Result tempresult = new CubeTagEvent.Choice.Result
                        {
                            text = result.InnerText,
                            ID = int.Parse(result.GetAttribute("ID")),
                            rate = float.Parse(result.GetAttribute("Rate"))
                        };
                        if (result.HasAttribute("Acquire"))
                            tempresult.acquire = result.GetAttribute("Acquire");
                        tempChoice.results.Add(tempresult);
                    }
                    tempTagEvent.choices.Add(tempChoice);
                }
                ExploreEvents[enumName].Add(tempTagEvent);
            }
        }
    }
}
