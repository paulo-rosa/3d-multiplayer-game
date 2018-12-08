using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerMenuManager : MonoBehaviour {

    private static MultiplayerMenuManager _instance;


    private MenuManager _menuManager;

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
        _menuManager = MenuManager.Instance;
        ChangeState(MenuState.LOBBY);
	}
    public void BackToMainMenu()
    {
        _menuManager.SwitchScreen(Screens.MAIN_MENU);
    }
    public void ChangeState(MenuState state)
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