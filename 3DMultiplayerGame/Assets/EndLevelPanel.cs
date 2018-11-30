using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndLevelPanel : MonoBehaviour {

    public Text txtScore;

    private void OnEnable()
    {
        txtScore.text = "Score: " + GameManager.Instance.GetScore();
    }
}
