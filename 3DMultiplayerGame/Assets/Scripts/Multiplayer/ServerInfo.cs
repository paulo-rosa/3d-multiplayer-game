using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Types;
using UnityEngine.UI;

public class ServerInfo : MonoBehaviour {

    public Text MatchName;
    public Button ButtonJoin;

    private ulong _networkID;
    private MyNetworkManager _myNetworkManager;
    private MultiplayerMenuManager _multiplayerMenuManager;

    private void Start ()
    {
        _myNetworkManager = MyNetworkManager.Instance;
        _multiplayerMenuManager = MultiplayerMenuManager.Instance;
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void Initialize(string matchName, ulong networkId)
    {
        MatchName.text = matchName;
        _networkID = networkId;
        ButtonJoin.onClick.AddListener(JoinMatch);
    }

    public void JoinMatch()
    {
        _myNetworkManager.JoinMatch((NetworkID)_networkID, (success, amtchInfo) => {
            if (success)
            {
                _multiplayerMenuManager.ChangeState(MenuState.ROOM);
            }
        });
    }
}
