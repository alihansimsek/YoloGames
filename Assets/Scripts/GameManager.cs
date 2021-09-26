using UnityEngine;

public class GameManager : Singleton<GameManager>   //Game manager listens and throws events to control the flow of the game
{                                                   //PlayerController and Collectables throws events for game manager to handle
    #region Events
    public delegate void CollectableAction(int score);
    public event CollectableAction CollectableHit;
    public delegate void GameStateChange();
    public event GameStateChange GameOverEvent;
    public event GameStateChange GameReadyEvent;
    public event GameStateChange GameResetEvent;
    public event GameStateChange GameNextLevelEvent;
    #endregion
    private int currentLevel = 0;
    public int score = 0;
    private void OnEnable()
    {
        
        PlayerController.PlayerDeathEvent += GameOver;
        PlayerController.PlayerSwipeEvent += GameReady;
        PlayerController.PlayerResetEvent += GameReset;
        PlayerController.PlayerNextLevelEvent += NextLevel;
        Collectable.collectableHitEvent += collectableCollected;
    }
    private void OnDestroy()
    {
        PlayerController.PlayerDeathEvent -= GameOver;
        PlayerController.PlayerSwipeEvent -= GameReady;
        PlayerController.PlayerResetEvent -= GameReset;
        PlayerController.PlayerNextLevelEvent -= NextLevel;
        Collectable.collectableHitEvent -= collectableCollected;
    }
    void collectableCollected()
    {
        score++;
        if(CollectableHit != null)
            CollectableHit(score);
    }
    void NextLevel()
    {
        currentLevel++; //redundant
        if(GameNextLevelEvent != null)
            GameNextLevelEvent();
    }
    void GameOver()
    {
        if(GameOverEvent != null)
            GameOverEvent();
        score = 0;
        if (CollectableHit != null)
            CollectableHit(score);
    }
    void GameReady()
    {
        PlayerController.PlayerSwipeEvent -= GameReady;
        if(GameReadyEvent != null)
            GameReadyEvent();
    }
    void GameReset()
    {
        if (GameResetEvent != null)
            GameResetEvent();
    }
}
