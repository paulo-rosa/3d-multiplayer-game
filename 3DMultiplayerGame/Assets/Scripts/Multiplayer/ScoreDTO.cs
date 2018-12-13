using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreDTO
{
    public string PlayerName { get; set; }
    public int Kills { get; set; }
    public int Deaths { get; set; }

    public ScoreDTO(string name, int kills, int deaths)
    {
        PlayerName = name;
        Kills = kills;
        Deaths = deaths;
    }
}
