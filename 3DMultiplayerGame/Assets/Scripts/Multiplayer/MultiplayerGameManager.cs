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

        public event Action<GameState> OnStateChange; 
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
        [SyncVar]
        private bool _isTimerEnabled = false;
        private int teste = 0;
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
                _timerCounter -= Time.deltaTime;
                _multiplayerInterface.UpdateTime(_timerCounter);
            }

            if(isServer && _timerCounter <= 0)
            {
                RpcEndRoud();
            }
        }

        private void StartTimer()
        {
            if (isServer)
            {
                _timerCounter = ROUND_TIME;
                _isTimerEnabled = true;
                RpcStartTimer(_timerCounter, _isTimerEnabled);
            }
        }

        private void CmdStartTimer()
        {

        }

        [ClientRpc]
        private void RpcEndRoud()
        {
            _isTimerEnabled = false;
            _timerCounter = 0;
            _multiplayerInterface.UpdateTime(_timerCounter);
            ChangeState(GameState.EndLevel);
        }

        [ClientRpc]
        private void RpcStartTimer(float time, bool isTimer)
        {
            StartTimerClient(time, isTimer);
            teste = 20;
        }

        private void StartTimerClient(float roundTime, bool isTimerEnable)
        {
            _timerCounter = roundTime;
            _isTimerEnabled = isTimerEnable;
        }

        private void Teste()
        {
            Debug.Log("tete");
        }

        #region States
        private void OnEndLevel()
        {

        }
        #endregion

        #region Interface implementation
        public void ChangeState(GameState state)
        {
            var previousState = _currentState;
            _currentState = state;
            if (OnStateChange != null)
            {
                OnStateChange(_currentState);
            }

            switch (state)
            {
                case GameState.Game:
                    OnStartGame(previousState);
                    break;

                case GameState.EndLevel:
                    OnEndLevel();
                    break;
                case GameState.GameOver:
                    OnGameOver();
                    break;
            }
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
