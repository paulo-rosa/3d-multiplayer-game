using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour, IGameManager
{
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
            }
            return _instance;
        }
    }


    Transform IGameManager._player
    {
        get
        {
            return _player;
        }

        set
        {
            _player = value;
        }
    }

    Transform IGameManager._spawnPosition
    {
        get
        {
            return _spawnPosition;
        }
        set
        {
            _spawnPosition = value;
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

    private void Start()
    {
        _userInterfaceManager = UserInterfaceManager.Instance;
        StartGame();
    }

    private void Update()
    {
        PauseGame();
    }

    private void PauseGame()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (_currentState == GameState.Game)
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
        if (_lifes == 0)
        {
            //GAME OVER;
            ChangeState(GameState.GameOver);
            return false;
        }
        return true;
    }

    public void Pause()
    {
        ChangeState(GameState.Pause);
    }

    public void Resume()
    {
        ChangeState(GameState.Game);
    }

    public void PlayerRespawn()
    {

    }

    #region States Change
    public void StartGame()
    {
        NetworkManager.singleton.StartHost();
        Instantiate(_playerPrefab, _spawnPosition.position, _spawnPosition.rotation);
        ChangeState(GameState.Game);
    }

    public void GameOver()
    {
        ChangeState(GameState.GameOver);
    }

    public void EndLevel()
    {
        ChangeState(GameState.EndLevel);

    }
    #endregion

    public GameState GetState()
    {
        return _currentState;
    }

    public void ChangeState(GameState state)
    {
        var previousState = _currentState;
        _currentState = state;
        if (onStateChange != null)
        {
            onStateChange(_currentState);
        }

        switch (state)
        {
            case GameState.Game:
                OnStartGame(previousState);
                break;
            case GameState.Pause:
                OnPauseGame();
                break;
            case GameState.EndLevel:
                OnEndLevel();
                break;
            case GameState.GameOver:
                OnGameOver();
                break;
        }
    }

    public void DeacreaseLife()
    {
        _lifes--;
        UpdateUI();
    }

    public void IncreaseLife()
    {
        if (_lifes >= 3) return;

        _lifes++;
        UpdateUI();
    }

    public void UpdateUI()
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

    public void SetSpawnPosition(Transform position)
    {
        _spawnPosition = position;
    }

    #region states
    public void OnStartGame(GameState previousState)
    {
        if (previousState == GameState.Pause)
        {
            OnResumeGame();
            return;
        }

        _lifes = 3;
        _score = 0;
        UpdateUI();
    }

    public void OnGameOver()
    {

    }

    public void OnPauseGame()
    {
        Time.timeScale = 0;
    }

    public void OnResumeGame()
    {
        Time.timeScale = 1;
    }

    public void OnEndLevel()
    {
        //_userInterfaceManager
    }
    #endregion
}

public enum GameState
{
    Freeze,
    Game,
    Pause,
    EndLevel,
    GameOver
}