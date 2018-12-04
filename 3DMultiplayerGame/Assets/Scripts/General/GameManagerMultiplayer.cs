using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManagerMultiplayer : GameManager
{
    protected override void Start()
    {
        NetworkManager.singleton.StartHost();
        ChangeState(GameState.GAME);
    }
}