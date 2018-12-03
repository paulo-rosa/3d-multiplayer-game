using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoomButton : MonoBehaviour {

    public Text RoomName;

    public void CreateRoom()
    {
        var gameName = RoomName.text;
        if (string.IsNullOrEmpty(gameName))
            return;

        MyNetworkManager.Instance.CreateInternetMatch(gameName, (success, matchName) => {

            if (success)
            {
                Debug.Log("Sala Criada");
            }
        });
    }
}
