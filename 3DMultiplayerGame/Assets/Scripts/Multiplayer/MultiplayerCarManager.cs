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
        private int _death = 0;

        [SyncVar]
        private int _victims = 0;

        [SyncVar]
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

        public override void OnStartAuthority()
        {
            base.OnStartAuthority();

        }

        
        void Start()
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
            if (isServer)
            {
                _death++;
            }
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

        }

        public void Died()
        {

        }

        public void Killed()
        {
            _victims++;
            //UpdatePlacar;
        }
    }

}
