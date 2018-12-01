using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerMenuManager : MonoBehaviour {

    private static MultiplayerMenuManager _instance;

    public static MultiplayerMenuManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<MultiplayerMenuManager>();
            }

            return _instance;
        }
    }

    public event Action<MenuState> onChangeState;

    private MenuState _currentState;

    private void Start ()
    {
        ChangeState(MenuState.LOBBY);
	}
	
    private void ChangeState(MenuState state)
    {
        _currentState = state;
        if(onChangeState != null)
        {
            onChangeState(_currentState);
        }

        switch (_currentState)
        {
            case MenuState.LOBBY:
                OnLobbyState();
                break;
            case MenuState.ROOM:
                OnRoomState();
                break;
        }
    }

    private void OnLobbyState()
    {

    }

    private void OnRoomState()
    {
         
    }
}

public enum MenuState
{
    LOBBY,
    ROOM
}