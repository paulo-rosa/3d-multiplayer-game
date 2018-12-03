using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkPlayer : NetworkBehaviour
{
    [SerializeField]
    protected GameObject m_LobbyPrefab;

    public LobbyPlayer _lobbyObject;
    private MyNetworkManager _myNetworkManager;

	// Use this for initialization
	private void Start ()
    {
        _myNetworkManager = MyNetworkManager.Instance;
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
        _myNetworkManager.RegisterNetworkPlayer(this);
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
        _lobbyObject = Instantiate(m_LobbyPrefab).GetComponent<LobbyPlayer>();
        _lobbyObject.Init();
    }
}
