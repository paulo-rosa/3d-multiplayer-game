using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkPlayer : NetworkBehaviour
{
    [SerializeField]
    protected GameObject m_LobbyPrefab;
    [SyncVar]
    private string _playerName;
    [SyncVar]
    private bool _isReady = false;

    public LobbyPlayer _lobbyObject;
    private MyNetworkManager _myNetworkManager;
    
    public string GetPlayerName()
    {
        return _playerName;
    }
	private void Start ()
    {
        _myNetworkManager = MyNetworkManager.Instance;

        if (hasAuthority)
        {
            _playerName = PlayerData.PlayerName;
            CmdSetPlayerName(_playerName);
        }
        _myNetworkManager.RegisterNetworkPlayer(this);
    }

    public bool GetReadyState()
    {
        return _isReady;
    }

    public bool SetReady()
    {
        _isReady = !_isReady;
        return _isReady;
    }

    // Update is called once per frame
    private void Update ()
    {
		
	}

    [Client]
    public override void OnStartClient()
    {
        DontDestroyOnLoad(this);


        base.OnStartClient();
        Debug.Log("Client Network Player start:" + hasAuthority);
        if (_myNetworkManager == null)
        {
            _myNetworkManager = MyNetworkManager.Instance;
        }
       
    }



    [Client]
    public void OnEnterLobbyScene()
    {

        Debug.Log("OnEnterLobbyScene:" + hasAuthority);
        if (_lobbyObject == null)
        {
            CreateLobbyObject();
        }
    }

  

    private void CreateLobbyObject()
    {
        Debug.Log("CreateLobbyObject:" + hasAuthority);
        var go = Instantiate(m_LobbyPrefab, PlayersHolder.Instance.GetPlayersHolder());
        _lobbyObject = go.GetComponent<LobbyPlayer>();
        _lobbyObject.Init(this);
    }

    #region RPC CALLS
    
    [Command]
    private void CmdSetPlayerName(string name)
    {
        _playerName = name;
    }

    #endregion
}
