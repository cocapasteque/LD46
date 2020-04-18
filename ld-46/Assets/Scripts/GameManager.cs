using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public class GameManager : Singleton<GameManager>
{
    private GameState _state;
    public GameState State
    {
        get => _state;
        set
        {
            switch (value)
            {
                case GameState.Preparing:
                    OnLevelPrepare?.Invoke();
                    break;
                case GameState.Running:
                    OnLevelRun?.Invoke();
                    break;
            }

            _state = value;
        }
    }

    public UnityAction OnLevelRun;
    public UnityAction OnLevelPrepare;
    public UnityAction OnPlayerDied;

    void Awake()
    {
        State = GameState.MainMenu;
    }
    
    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
        SceneManager.LoadScene("Game Overlay", LoadSceneMode.Additive);
        State = GameState.Preparing;
    }

    public void StartLevel()
    {
        State = GameState.Running;
    }

    public void KillPlayer()
    {
        OnPlayerDied?.Invoke();
        State = GameState.Preparing;
    }
}

public enum GameState
{
    MainMenu,
    Preparing,
    Running
}