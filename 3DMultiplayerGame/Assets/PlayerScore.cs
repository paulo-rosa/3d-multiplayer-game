using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScore : MonoBehaviour
{
    public Text txtPlayerName;
    public Text txtKills;
    public Text txtDeaths;

   

    public void Initialize(ScoreDTO score)
    {
        txtPlayerName.text = score.PlayerName;
        txtKills.text = score.Kills.ToString();
        txtDeaths.text = score.Deaths.ToString();
    }

    public void Clean()
    {
        txtPlayerName.text = ""; 
        txtKills.text = "";
        txtDeaths.text = "";
    }

    
}
