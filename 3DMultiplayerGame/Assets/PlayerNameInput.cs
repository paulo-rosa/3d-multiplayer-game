using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameInput : MonoBehaviour {

    public Text _playerName;

   
    public void SetPlayerName()
    {
        PlayerData.PlayerName = _playerName.text;
    }
}
