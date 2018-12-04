using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreGiver : MonoBehaviour {

    private GameManager _gameManager;
    public int _scoreAmount;

    private void Start ()
    {
        _gameManager = GameManager.Instance;
    }

    public void GiveScore()
    {
        _gameManager.GiveScore(_scoreAmount);
    }

    // Update is called once per frame
    void Update ()
    {
		
	}
}
