using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPlayer : MonoBehaviour {

    public Text txtPlayerName;
    public Button btnReady;

    private NetworkPlayer _networkPlayer;

	void Start () {
		
	}
	
	void Update () {
		
	}

    public void Init(NetworkPlayer player)
    {
        _networkPlayer = player;
        txtPlayerName.text = _networkPlayer.GetPlayerName();
        Debug.Log("init the lobby plaeyr");
        ChangeButtonState(_networkPlayer.GetReadyState());
    }

    public void SetReady()
    {
        ChangeButtonState(_networkPlayer.SetReady());
    }

    private void ChangeButtonState(bool value)
    {
        if(_networkPlayer.isLocalPlayer)
        {
            if (value)
            {
                btnReady.GetComponent<Image>().color = Color.green;
                btnReady.interactable = false;
            }
            else
            {
                btnReady.GetComponent<Image>().color = Color.red;
                btnReady.interactable = true;
            }
        }
        else
        {
            btnReady.GetComponent<Image>().color = Color.red;
            btnReady.interactable = false;
        }
    }
}
