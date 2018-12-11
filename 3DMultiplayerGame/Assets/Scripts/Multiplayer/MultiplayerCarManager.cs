using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Multiplayer
{
    public class MultiplayerCarManager : NetworkBehaviour
    {
        //private static MultiplayerCarManager _instance;
        //public static MultiplayerCarManager Instance
        //{
        //    get
        //    {
        //        if (_instance == null)
        //        {
        //            _instance = FindObjectOfType<MultiplayerCarManager>();
        //        }
        //        return _instance;
        //    }
        //}



        private MultiplayerGameManager _gameManager;

        private CarBehaviour _carBehaviour;

        [SyncVar]
        private int _death = 0;

        [SyncVar]
        private int _victims = 0;

        public override void OnStartAuthority()
        {
            base.OnStartAuthority();

        }

        void Start()
        {
            _carBehaviour = GetComponent<CarBehaviour>();
            _gameManager = MultiplayerGameManager.Instance;
            Initialize();
        }

        private void Initialize()
        {
            _gameManager.CmdPlayerConnected();
            _carBehaviour.OnPlayerDied += OnPlayerDie;
            if (hasAuthority)
            {
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
    }

}
