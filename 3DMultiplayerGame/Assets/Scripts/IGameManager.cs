public interface IGameManager
{
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