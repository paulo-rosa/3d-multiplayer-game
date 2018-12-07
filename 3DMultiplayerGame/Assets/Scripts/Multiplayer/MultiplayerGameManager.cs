using System;
using UnityEngine;

namespace Assets.Scripts.Multiplayer
{
    public class MultiplayerGameManager : MonoBehaviour, IGameManager
    {
        private UserInterfaceManager _userInterfaceManager;
        private GameState _currentState;

        private void Start()
        {
            
        }

        private void Update()
        {
            
        }

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
    }
}
