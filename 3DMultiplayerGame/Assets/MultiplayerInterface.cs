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

	private void Start ()
    {
		
	}
	
	private void Update ()
    {
		
	}

    internal void UpdateTime(float timerCounter)
    {
        var time = timerCounter % 60;
        _txtTimer.text = time.ToString();
    }
}
