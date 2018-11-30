using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerUIManager : MonoBehaviour {

    public GameObject LobbyPanel;
    public GameObject RoomPanel;

    private GameObject _currentPanel;
    private MultiplayerMenuManager _multiplayerMenuManager;

	private void Start ()
    {
        _multiplayerMenuManager = GetComponent<MultiplayerMenuManager>();
        DisableAll();
        _multiplayerMenuManager.onChangeState += ChangePanel;
    }


    private void OnDisable()
    {
        _multiplayerMenuManager.onChangeState -= ChangePanel;
    }

    private void ChangePanel(MenuState state)
    {
        switch (state)
        {
            case MenuState.LOBBY:
                ChangePanel(LobbyPanel);
                break;
            case MenuState.ROOM:
                ChangePanel(RoomPanel);
                break;
        }
    }

    private void ChangePanel(GameObject panel)
    {
        if(_currentPanel!= null)
            _currentPanel.SetActive(false);

        _currentPanel = panel;

        _currentPanel.SetActive(true);
    }

    private void DisableAll()
    {
        LobbyPanel.SetActive(false);
        RoomPanel.SetActive(false);
    }
}
