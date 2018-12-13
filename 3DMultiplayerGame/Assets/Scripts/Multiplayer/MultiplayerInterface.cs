using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiplayerInterface : MonoBehaviour
{
    private static MultiplayerInterface _instance;

    public static MultiplayerInterface Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<MultiplayerInterface>();
            }
            return _instance;
        }
    }

    public Text _txtTimer;

    internal void UpdateTime(float timerCounter)
    {
        var time = (int)timerCounter;
        _txtTimer.text = time.ToString();
    }
}
