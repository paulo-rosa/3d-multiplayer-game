using Assets.Scripts.Multiplayer;
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

    public Text txtTimer;
    public Text txtCounter;
    public Text txtWinner;
    public GameObject EndPanel;
    private float _counter;
    private bool _finalCounter = false;

    internal void UpdateTime(float timerCounter)
    {
        var time = (int)timerCounter;
        txtTimer.text = time.ToString();
    }

    public void ShowEndPanel(string winner)
    {
        if(EndPanel.activeSelf == false)
        {
            EndPanel.SetActive(true);
            txtWinner.text = winner;
            _finalCounter = true;
            _counter = 5f;
        }
        
    }

    public void UpdateCounter(int seconds)
    {
        txtCounter.text = "Leaving in... " + seconds;
    }

    private void Update()
    {
       if (_finalCounter)
        {
            _counter -= Time.unscaledDeltaTime;
            UpdateCounter((int)_counter);
            if (_counter <= 0)
            {
                MultiplayerGameManager.Instance.LeaveToLobby();
            }
        }

        
    }
}
