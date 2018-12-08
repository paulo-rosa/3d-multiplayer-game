using UnityEngine;
using System;

public interface IGameManager
{
    Transform _player { get; set; }
    Transform _spawnPosition { get; set; }

    string GetScore();
    void GiveScore(int score);
    bool Die();
    void Pause();
    void PlayerRespawn();
    void GameOver();
    GameState GetState();
    void ChangeState(GameState state);
    void UpdateUI();
    void ExitToMenu();
    void OnStartGame(GameState previousState);
    void OnGameOver();
}

