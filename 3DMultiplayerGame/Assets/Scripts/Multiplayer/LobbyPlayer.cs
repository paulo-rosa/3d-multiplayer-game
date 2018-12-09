using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPlayer : MonoBehaviour {

    public Text txtPlayerName;
    public Button btnReady;

    private NetworkPlayer _networkPlayer;


    public void Init(NetworkPlayer player)
    {
        NetworkPlayer.OnNameUpdated += OnNameUpdated;
        player.OnReadyChange += OnReadyChange;                                         
        _networkPlayer = player;
        txtPlayerName.text = _networkPlayer.GetPlayerName();
        Debug.Log("init the lobby plaeyr");
        ChangeButtonState(_networkPlayer.GetReadyState());
    }

    public void OnReadyChange()
    {
        if (_networkPlayer.GetReadyState())
        {
            ButtonGreen();
        }
        else
        {
            ButtonRed();
        }
    }

    private void OnNameUpdated(NetworkPlayer player, string name)
    {
        if (player != _networkPlayer) return;

        txtPlayerName.text = _networkPlayer.GetPlayerName();
        //
    }

    public void SetReady()
    {
        ChangeButtonState(_networkPlayer.SetReady());
    }

    private void ChangeButtonState(bool value)
    {
        if(_networkPlayer.hasAuthority)
        {
            if (value)
            {
                ButtonGreen();
                btnReady.interactable = false;
            }
            else
            {
                ButtonRed();
                btnReady.interactable = true;
            }
        }
        else
        {
            ButtonRed();
            btnReady.interactable = false;
        }
    }

    private void ButtonGreen()
    {
        btnReady.GetComponent<Image>().color = Color.green;
    }

    private void ButtonRed()
    {
        btnReady.GetComponent<Image>().color = Color.red;
    }
}
