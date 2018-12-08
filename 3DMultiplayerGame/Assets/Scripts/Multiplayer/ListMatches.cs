using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;

public class ListMatches : MonoBehaviour {

    public GameObject MatchPrefab;
    public RectTransform RoomsHolder;

    private MultiplayerMenuManager _multiPlayerManager;
    private MyNetworkManager _myNetworkManager;
    private float _timeCount = 5f;
    private float _timeToUpdate = 5f;
    private short _pageIndex;
    
	// Use this for initialization
	private void Start ()
    {
        _multiPlayerManager = MultiplayerMenuManager.Instance;
        _myNetworkManager = MyNetworkManager.Instance;
	}
    protected virtual void OnEnable()
    {
        _pageIndex = 0;
        //Cache singletons
        if (_myNetworkManager == null)
        {
            _myNetworkManager = MyNetworkManager.Instance;
        }
    }
    private void Update ()
    {
        _timeCount += Time.deltaTime;

        if(_timeCount >= _timeToUpdate)
        {
            RefreshMatches(_pageIndex);
            _timeCount = 0;
        }
	}

    private void RefreshMatches(short pageIndex)
    {
        _myNetworkManager.matchMaker.ListMatches(pageIndex, 4, "", false, 0, 0, OnMatchList);
    }

    private void OnMatchList(bool flag, string extraInfo, List<MatchInfoSnapshot> response)
    {

        foreach (Transform match in RoomsHolder)
            Destroy(match.gameObject);

        if(response.Count > 0)
        {
            foreach(var match in response)
            {

                var go = Instantiate(MatchPrefab, RoomsHolder);
                go.GetComponent<ServerInfo>().Initialize(match.name, (ulong)match.networkId);
            }
        }
    }
}
