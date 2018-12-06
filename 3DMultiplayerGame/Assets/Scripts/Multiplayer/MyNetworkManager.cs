using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;

public class MyNetworkManager : NetworkManager
{
    private static MyNetworkManager _instance;
    public static MyNetworkManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<MyNetworkManager>();
            }
            return _instance;
        }
    }

    private List<NetworkPlayer> connectedPlayers = new List<NetworkPlayer>();
    private MultiplayerMenuManager _multiPlayerMenuManager;
    private NetworkManager networkManager;
    public NetworkPlayer _networkPlayerPrefab;

    private Action<bool, MatchInfo> _nextMatchCreatedCallback;
    private Action<bool, MatchInfo> _nextMatchJoinedCallback;

    // criar sala
    // carregar salas
    // entrar na sala
    // 

    private void Start()
    {
        networkManager = NetworkManager.singleton;
        _multiPlayerMenuManager = MultiplayerMenuManager.Instance;
        networkManager.StartMatchMaker();
    }

    public void CreateInternetMatch(string matchName, Action<bool, MatchInfo> onCreate)
    {
        networkManager.StartMatchMaker();
        _nextMatchCreatedCallback = onCreate;
        networkManager.matchMaker.CreateMatch(matchName, 4, true, "", "", "", 0, 0, OnInternetMatchCreated);
    }

    private void OnInternetMatchCreated(bool success, string extendedInfo, MatchInfo responseData)
    {
        base.OnMatchCreate(success, extendedInfo, responseData);
        //mmudar o status do jogo para roompanel
        if (success)
        {
            _multiPlayerMenuManager.ChangeState(MenuState.ROOM);
        }

        if(_nextMatchCreatedCallback != null)
        {
            _nextMatchCreatedCallback(success, matchInfo);
            _nextMatchCreatedCallback = null;
        }
    }

    public void JoinMatch(NetworkID networkId, Action<bool, MatchInfo> onJoin)
    {
        _nextMatchJoinedCallback = onJoin;
        matchMaker.JoinMatch(networkId, string.Empty, string.Empty, string.Empty, 0, 0, OnMatchJoined);
    }

    public override void OnMatchJoined(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        base.OnMatchJoined(success, extendedInfo, matchInfo);
        if (success)
        {
            _nextMatchJoinedCallback(success, matchInfo);
        }
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        Debug.Log("OnClientConnect");

        ClientScene.Ready(conn);
        ClientScene.AddPlayer(0);

    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        // Intentionally not calling base here - we want to control the spawning of prefabs
        Debug.Log("OnServerAddPlayer");

        NetworkPlayer newPlayer = Instantiate<NetworkPlayer>(_networkPlayerPrefab);
        DontDestroyOnLoad(newPlayer);
        NetworkServer.AddPlayerForConnection(conn, newPlayer.gameObject, playerControllerId);
    }

    public void RegisterNetworkPlayer(NetworkPlayer newPlayer)
    {

        connectedPlayers.Add(newPlayer);
        newPlayer.OnEnterLobbyScene();
        newPlayer.onReadyChange += PlayerSetReady;
        //newPlayer.becameReady += OnPlayerSetReady;

        //if (s_IsServer)
        //{
        //    UpdatePlayerIDs();
        //}

    }

    private void PlayerSetReady(bool value)
    {

        var shouldStart = true;
        foreach (var player in connectedPlayers)
        {
            if (!player.GetReadyState())
            {
                Debug.Log("not ready");
                shouldStart = false;
                break;
            }
        }
        if (shouldStart)
        {
            //Start game;
            Debug.Log("start game");
        }


    }
}
