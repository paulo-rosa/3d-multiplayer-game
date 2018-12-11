using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersHolder : MonoBehaviour {

    private static PlayersHolder _instance;

    public static PlayersHolder Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<PlayersHolder>();
            }

            return _instance;
        }
    }


    public Transform GetPlayersHolder()
    {
        return transform;
    }
}
