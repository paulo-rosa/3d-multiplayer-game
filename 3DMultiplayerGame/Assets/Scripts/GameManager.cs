using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour {

    private static GameManager _instance;
    
    public static GameManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
            }
            return _instance;
        }
    }

    private int _lifes;
    private int _score;
    private UserInterfaceManager _userInterfaceManager;
    private GameState _currentState;

    
    public GameObject _playerPrefab;
    public Transform _spawnPosition;
    public Transform _player;

    public event Action<GameState> onStateChange;

	private void Start ()
    {
        _userInterfaceManager = UserInterfaceManager.Instance;
        ChangeState(GameState.MENU);

	}
	
	private void Update ()
    {
		
	}

    public void GiveScore(int score)
    {
        _score += score;
        _userInterfaceManager.UpdateScore(_score);
    }

    public bool Die()
    {
        Debug.Log("MOrreu");
        DeacreaseLife();
        if(_lifes == 0)
        {
            //GAME OVER;
            ChangeState(GameState.GAME_OVER);
            return false;
        }
        return true;
    }

    private void PlayerRespawn()
    {
        
    }


    #region States Change
    public void StartGame()
    {
        NetworkManager.singleton.StartHost();

        ChangeState(GameState.GAME);
    }

    private void GameOver()
    {
        ChangeState(GameState.GAME_OVER);
    }

    private void EndLevel()
    {
        ChangeState(GameState.END_LEVEL);

    }
    #endregion

    private void ChangeState(GameState state)
    {
        _currentState = state;
        if(onStateChange != null)
        {
            onStateChange(_currentState);
        }

        switch (state)
        {
            case GameState.GAME:
                OnStartGame();
                break;
            case GameState.GAME_OVER:
                OnGameOver();
                break;
        }
    }

    private void DeacreaseLife()
    {
        _lifes--;
        UpdateUI();
    }

    private void IncreaseLife()
    {
        if (_lifes >= 3) return;

        _lifes++;
        UpdateUI();
    }

    private void UpdateUI()
    {
        _userInterfaceManager.UpdateLifes(_lifes);
        _userInterfaceManager.UpdateScore(_score);
    }

    #region 
    public void SetSpawnPosition(Transform position)
    {
        _spawnPosition = position;
    }
    #endregion

    #region STATES
    private void OnStartGame()
    {
        _lifes = 3;
        _score = 0;
        UpdateUI();
    }

    private void OnGameOver()
    {
        
    }
    #endregion
}

public enum GameState
{
    MENU,
    GAME,
    END_LEVEL,
    GAME_OVER
}