using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndLevelPanel : MonoBehaviour {

    public Text txtScore;

    private GameManager _gameManager;

	private void Start ()
    {
        _gameManager = GameManager.Instance;
	}

    private void OnEnable()
    {
        txtScore.text = "Score: " + _gameManager.GetScore();
    }
}
