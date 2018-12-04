using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour
{

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

    public string GetScore()
    {
        return _score.ToString();
    }

    private int _lifes;
    private int _score = 0;
    private UserInterfaceManager _userInterfaceManager;
    private GameState _currentState;

    
    public GameObject _playerPrefab;
    public Transform _spawnPosition;
    public Transform _player;
    public event Action<GameState> onStateChange;

	protected virtual void Start ()
    {
        _userInterfaceManager = UserInterfaceManager.Instance;
        StartGame();
    }
	
	private void Update ()
    {
        PauseGame();
	}
    private void PauseGame()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (_currentState == GameState.GAME)
                Pause();
            else
                Resume();
        }
    }
    public void GiveScore(int score)
    {
        _score += score;
        _userInterfaceManager.UpdateScore(_score);
    }

    public bool Die()
    {
        DeacreaseLife();
        if(_lifes == 0)
        {
            //GAME OVER;
            ChangeState(GameState.GAME_OVER);
            return false;
        }
        return true;
    }

    public void Pause()
    {
        ChangeState(GameState.PAUSE);
    }

    public void Resume()
    {
        ChangeState(GameState.GAME);
    }

    private void PlayerRespawn()
    {
        
    }

    #region States Change
    public void StartGame()
    {
        NetworkManager.singleton.StartHost();
        Instantiate(_playerPrefab, _spawnPosition.position, _spawnPosition.rotation);
        ChangeState(GameState.GAME);
    }

    protected void GameOver()
    {
        ChangeState(GameState.GAME_OVER);
    }

    public void EndLevel()
    {
        ChangeState(GameState.END_LEVEL);
    }
    #endregion

    public GameState GetState()
    {
        return _currentState;
    }

    protected void ChangeState(GameState state)
    {
        var previousState = _currentState;
        _currentState = state;
        if(onStateChange != null)
        {
            onStateChange(_currentState);
        }

        switch (state)
        {
            case GameState.GAME:
                OnStartGame(previousState);
                break;
            case GameState.PAUSE:
                OnPauseGame();
                break;
            case GameState.END_LEVEL:
                OnEndLevel();
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

    public void RestartLevel()
    {
        MenuManager.Instance.SwitchScreen(Screens.SINGLEPLAYER);
    }
    
    public void ExitToMenu()
    {
        MenuManager.Instance.SwitchScreen(Screens.MAIN_MENU);
    }
    #region 
    public void SetSpawnPosition(Transform position)
    {
        _spawnPosition = position;
    }
    #endregion

    #region STATES
    private void OnStartGame(GameState previousState)
    {
        if (previousState == GameState.PAUSE)
        {
            OnResumeGame();
            return;
        }

        _lifes = 3;
        _score = 0;
        UpdateUI();
    }

    private void OnGameOver()
    {
        
    }

    private void OnPauseGame()
    {
        Time.timeScale = 0;
    }

    private void OnResumeGame()
    {
        Time.timeScale = 1;
    }

    private void OnEndLevel()
    {
        //_userInterfaceManager
    }
    #endregion
}

public enum GameState
{
    FREEZE,
    GAME,
    PAUSE,
    END_LEVEL,
    GAME_OVER
}