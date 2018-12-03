using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerData
{
    public static string PlayerName;
    public static int PlayerScore;

    public static string RandomName()
    {
        return "guest" + Random.Range(1000, 9999);
    }
}
