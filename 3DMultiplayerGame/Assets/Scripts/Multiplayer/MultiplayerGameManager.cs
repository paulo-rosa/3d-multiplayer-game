using System;
using System.Collections.Generic;
using System.Linq;
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
                if (_instance == null)
                {
                    _instance = FindObjectOfType<MultiplayerGameManager>();
                }
                return _instance;
            }
        }

        public GameObject CarPrefab;
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

        [SyncVar(hook = "OnPlayersInSceneChanged")]
        private int _playerInScene = 0;

        private bool _isServer;

        #region multiplayer variables
        //Equipes
        //
        private Teams _playerTeam;
        private MultiplayerCarManager _localPlayer;
        private List<MultiplayerCarManager> _playersList = new List<MultiplayerCarManager>();
        private List<ScoreDTO> scores = new List<ScoreDTO>();
        public MultiplayerCarManager localPlayer
        {
            get
            {
                return _localPlayer;
            }
        }
        #endregion

        private void Start()
        {
            _multiplayerInterface = MultiplayerInterface.Instance;
            _myNetworkManager = MyNetworkManager.Instance;
            _isServer = MyNetworkManager._IsServer;
            if (_isServer)
            {
                StartTimer();
            }
        }

        public void AddConnectedPlayer(MultiplayerCarManager player)
        {
            if (isServer)
            {
                player.PlayerId = _playersList.Count + 1;
                RpcUpdateClientTime(_timerCounter);
            }

            _playersList.Add(player);
            UpdateScoreBoard();
        }

        public void SetCarManager(MultiplayerCarManager car)
        {
            _localPlayer = car;
        }

        [Command]
        public void CmdPlayerConnected()
        {
            _playerInScene++;
        }

        private void OnPlayersInSceneChanged(int value)
        {
            _playerInScene = value;
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
            if (_timerCounter <= 0)
            {
                Debug.Log("dsadas");
            }
            if (_isServer && _timerCounter <= 0 && _isTimerEnabled)
            {
                _isTimerEnabled = false;
                EndGame();
                RpcEndRoud();
            }
        }

        private void StartTimer()
        {
            _timerCounter = ROUND_TIME;
            _isTimerEnabled = true;
            RpcStartTimer(_timerCounter, _isTimerEnabled);
        }

        [ClientRpc]
        private void RpcUpdateClientTime(float time)
        {
            _timerCounter = time - 1;
        }

        [ClientRpc]
        private void RpcPlayersInScene(int value)
        {
            _playerInScene = value;
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
        }

        private void StartTimerClient(float roundTime, bool isTimerEnable)
        {
            _timerCounter = roundTime - 1;
            _isTimerEnabled = isTimerEnable;
        }

        public Vector3 GetSpawnPosition()
        {
            return new Vector3(0, 5, 0);
        }

        [Server]
        public void KillSomeone(int shooterId, int deadId)
        {
           var shooter = _playersList.Find(s => s.PlayerId == shooterId);
            shooter.KillSomeone();

            var victim = _playersList.Find(s => s.PlayerId == deadId);
            victim.Died();

            RpcUpdateScoreBoard();
        }

        [ClientRpc]
        private void RpcUpdateScoreBoard()
        {
            UpdateScoreBoard();
        }


        public void UpdateScoreBoard()
        {
            scores.Clear();
            foreach(var player in _playersList)
            {
                ScoreDTO  score = new ScoreDTO(player.PlayerName, player.Kills, player.Deaths);
                scores.Add(score);
            }
            scores = scores.OrderByDescending(x => x.Kills).ToList();
            LeaderBoardManager.Instance.UpdateScoreBoard(scores);
        }

        //[Command]
        //private void CmdPlayerInScene()
        //{
        //    _playerInScene++;
        //    RpcPlayersInScene(_playerInScene);
        //    StartTimer();
        //}
        //private void OnSceneEnter()
        //{
        //    CmdPlayerInScene();
        //}

        private void EndGame()
        {
            _localPlayer.RpcEndGame();
            RpcShowEndPanel();
        }
        private string GetWinner()
        {
            UpdateScoreBoard();
            return scores[0].PlayerName;
        }

        [ClientRpc]
        private void RpcShowEndPanel()
        {
            _multiplayerInterface.ShowEndPanel(GetWinner());

        }

        public void LeaveToLobby()
        {
            MenuManager.Instance.SwitchScreen(Screens.MULTIPLAYER_MENU);
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
