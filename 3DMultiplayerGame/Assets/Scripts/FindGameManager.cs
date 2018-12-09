using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Multiplayer;

public class GeneralThings : MonoBehaviour
{
    
    public static IGameManager FindGameManager()
    {
        return MultiplayerGameManager.Instance;

        //if (MenuManager.Instance.GetGameState() == Screens.SINGLEPLAYER)
        //{
        //    return GameManager.Instance;
        //}
        //else if (MenuManager.Instance.GetGameState() == Screens.MULTIPLAYER)
        //{
        //    return MultiplayerGameManager.Instance;
        //}
        //return null;
    }
}
