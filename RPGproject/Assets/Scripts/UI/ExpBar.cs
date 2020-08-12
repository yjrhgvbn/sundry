using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpBar : MonoBehaviour {

    public static ExpBar _instance;
    private UISlider progressBar;

    private void Awake()
    {
        _instance = this;
        progressBar = this.GetComponent<UISlider>();
    }

    public void SetValue(float value)
    {
        progressBar.value = value;
    }
}
