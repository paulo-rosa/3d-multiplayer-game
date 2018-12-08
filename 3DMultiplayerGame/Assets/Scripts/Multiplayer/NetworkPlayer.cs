using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkPlayer : NetworkBehaviour
{
    public static event Action<NetworkPlayer, string> OnNameUpdated;

    [SerializeField]
    protected GameObject m_LobbyPrefab;
    [SyncVar(hook ="OnNameChange")]
    private string _playerName;
    [SyncVar(hook = "OnReadyUpdated")]
    private bool _isReady = false;

    public LobbyPlayer _lobbyObject;
    private MyNetworkManager _myNetworkManager;

    public event Action OnReadyChange;

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
        CmdSetPlayerReady(_isReady);
        return _isReady;
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

    #region Hooks

    private void OnReadyUpdated(bool value)
    {
        _isReady = value;
        if (OnReadyChange != null)
        {
            OnReadyChange();
        }
    }

    public void OnNameChange(string name)
    {
        _playerName = name;
        CmdSetPlayerName(_playerName);
    }

    #endregion

    #region COMMAND CALLS

    [Command]
    private void CmdSetPlayerReady(bool value)
    {
        if (isServer)
            Debug.Log("Player ready");
        _isReady = value;

    }

    [Command]
    private void CmdSetPlayerName(string name)
    {
        _playerName = name;

        if(OnNameUpdated != null)
        {
            OnNameUpdated(this, name);
        }
    }

    #endregion

    #region RPC

    #endregion
}
