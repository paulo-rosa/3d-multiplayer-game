using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Multiplayer
{
    public class MultiplayerCarManager : NetworkBehaviour
    {
        private MultiplayerGameManager _gameManager;

        private CarBehaviour _carBehaviour;

        [SyncVar]
        private string _playerName;

        public string PlayerName
        {
            get
            {
                return _playerName;
            }
            set
            {
                _playerName = value;
            }
        }

        [SyncVar(hook = "OnDeathUpdated")]
        private int _death = 0;

        public int Deaths
        {
            get
            {
                return _death;
            }
            set
            {
                _death = value;
            }
        }

        [SyncVar(hook ="OnKillUpdated")]
        private int _kills = 0;

        public int Kills
        {
            get
            {
                return _kills;
            }
            set
            {
                _kills = value;
            }
        }

        [SyncVar(hook ="OnPlayerIdUpdated")]
        private int _playerId;

        public int PlayerId
        {
            get
            {
                return _playerId;
            }
            set
            {
                _playerId = value;
            }
        }

        private bool _isInitialized;
        private void Start()
        {
            _carBehaviour = GetComponent<CarBehaviour>();
            _gameManager = MultiplayerGameManager.Instance;
            _gameManager.AddConnectedPlayer(this);
            Initialize();
        }

        private void Initialize()
        {
            _carBehaviour.OnPlayerDied += OnPlayerDie;

            if (hasAuthority)
            {
                _gameManager.CmdPlayerConnected();
                _gameManager.SetCarManager(this);
            }
        }

        public void OnPlayerDie()
        {
            if (hasAuthority)
            {
                ReSpawnCar(); 
            }
        }

        private void ReSpawnCar()
        {
            transform.position = SpawnsPoints.Instance.GetSpawnPoint().position;
        }
        
        void Update()
        {
            if(_gameManager.localPlayer == null && hasAuthority)
            {
                _gameManager.SetCarManager(this);
            }
        }

        public void Died()
        {
            _death++;
            Debug.LogFormat("Player {0} was killed for the {1} time. Loser", PlayerId, _death);
        }

        public void KillSomeone()
        {
            _kills++;
            Debug.LogFormat("Player {0} killed someone for the {1}th time", PlayerId, _kills);
            //UpdatePlacar;
        }

        private void OnKillUpdated(int kill)
        {
            _kills = kill;
            _gameManager.UpdateScoreBoard();
        }
        private void OnDeathUpdated(int death)
        {
            _death = death;
            _gameManager.UpdateScoreBoard();
        }
        private void OnPlayerIdUpdated(int id)
        {
            _playerId = id;
        }

        [ClientRpc]
        public void RpcEndGame()
        {
            Time.timeScale = 0;
        }
    }

}
