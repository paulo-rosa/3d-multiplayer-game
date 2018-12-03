using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomPanel : MonoBehaviour {

    public Text txtPlayerName;

    public void OnEnable()
    {
        txtPlayerName.text = PlayerData.PlayerName;
    }

}
