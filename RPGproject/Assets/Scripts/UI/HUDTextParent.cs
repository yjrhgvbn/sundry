﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDTextParent : MonoBehaviour {

    public static HUDTextParent _instance;

    private void Awake()
    {
        _instance = this;
    }
}
