using System;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Multiplayer
{
    public class MultiplayerGameManager : NetworkBehaviour, IGameManager
    {

        private static MultiplayerGameManager _instance;

        public static MultiplayerGameManager Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = FindObjectOfType<MultiplayerGameManager>();
                }
                return _instance;
            }
        }

        private UserInterfaceManager _userInterfaceManager;
        private GameState _currentState;


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

        public Transform _player;
        public Transform _spawnPosition;

        private MyNetworkManager _myNetworkManager;
        private const float ROUND_TIME = 180;
        private MultiplayerInterface _multiplayerInterface;
        private float _timerCounter;
        private bool _isTimerEnabled = false;
        #region multiplayer variables
        //Equipes
        //
        private Teams _playerTeam;
        #endregion
        private void Start()
        {
            _multiplayerInterface = MultiplayerInterface.Instance;
            StartTimer();
        }

        private void Update()
        {
            CountTime();
        }

        private void CountTime()
        {
            if (_isTimerEnabled)
            {
                _timerCounter -= Time.deltaTime * 1;
                _multiplayerInterface.UpdateTime(_timerCounter);
            }
        }

        private void StartTimer()
        {
            if (isServer)
            {
                _timerCounter = ROUND_TIME;
                _isTimerEnabled = true;
                RpcStartTimer();
            }
        }

        private void CmdStartTimer()
        {

        }

        private void RpcStartTimer()
        {
            _timerCounter = ROUND_TIME;
            _isTimerEnabled = true;
        }

        #region Interface implementation
        public void ChangeState(GameState state)
        {
            throw new NotImplementedException();
        }

        public bool Die()
        {
            throw new NotImplementedException();
        }

        public void ExitToMenu()
        {
            throw new NotImplementedException();
        }

        public void GameOver()
        {
            throw new NotImplementedException();
        }

        public GameState GetState()
        {
            throw new NotImplementedException();
        }



        public void GiveScore(int score)
        {
            throw new NotImplementedException();
        }

        public void OnGameOver()
        {
            throw new NotImplementedException();
        }

        public void OnStartGame(GameState previousState)
        {
            throw new NotImplementedException();
        }

        public void Pause()
        {
            throw new NotImplementedException();
        }

        public void PlayerRespawn()
        {
            throw new NotImplementedException();
        }

        public void UpdateUI()
        {
            throw new NotImplementedException();
        }

        public string GetScore()
        {
            throw new NotImplementedException();
        }

    #endregion 
    }
}

public enum Teams
{
    ONE,
    TWO
}

public enum GAME_STATES
{
   PreGame,
   Game,
   Freeze,
   EndRound
}
