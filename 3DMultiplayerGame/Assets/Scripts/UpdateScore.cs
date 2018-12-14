using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateScore : MonoBehaviour {

    public Text TxtScore;

    // Use this for initialization

    private void OnEnable()
    {
        TxtScore.text = "Final Score: " + GameManager.Instance.GetScore();
    }
}
