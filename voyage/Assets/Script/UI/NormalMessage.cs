using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NormalMessage : MonoBehaviour {
    
    private static GameObject WarningMessage;
    private static NormalMessage normalMessage;

    private void Awake()
    {
        WarningMessage = transform.Find("WarnMessage").gameObject;
        normalMessage = this;
    }

    private void Start()
    {
        
    }
    public static void ShowWarningMessage(string showMessage)
    {
        WarningMessage.SetActive(true);
        WarningMessage.GetComponentInChildren<Text>().text = showMessage;
        normalMessage.StartCoroutine("SetDisactive", WarningMessage);
    }

    IEnumerator SetDisactive(GameObject go)
    {
        yield return new WaitForSeconds(8f);
        go.SetActive(false);
    }
}
