using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerUIManager : MonoBehaviour {

    public GameObject LobbyPanel;
    public GameObject RoomPanel;

    private GameObject _currentPanel; 


	private void Start ()
    {
	
	}
	
    private void ChangePanel(MenuStates state)
    {
        switch (state)
        {
            case MenuStates.LOBBY:
                ChangePanel(LobbyPanel);
                break;
            case MenuStates.ROOM:
                ChangePanel(RoomPanel);
                break;
        }
    }

    private void ChangePanel(GameObject panel)
    {
        _currentPanel.SetActive(false);

        _currentPanel = panel;

        _currentPanel.SetActive(true);
    }
}
