using System.Collections;
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
                    Cursor.SetCursor(addFanIcon, Vector2.zero, CursorMode.ForceSoftware);
                    break;
                case GameState.Running:
                    OnLevelRun?.Invoke();
                    Cursor.SetCursor(noMoreFanIcon, Vector2.zero, CursorMode.ForceSoftware);
                    break;
            }

            _state = value;
        }
    }

    public UnityEvent OnLevelRun;
    public UnityEvent OnLevelPrepare;
    public UnityEvent OnPlayerDied;

    public Texture2D addFanIcon;
    public Texture2D noMoreFanIcon;
    
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        State = GameState.MainMenu;
    }

    public void LoadLevel(string levelName)
    {
        StartCoroutine(Work());
        IEnumerator Work()
        {
            var level = SceneManager.LoadSceneAsync(levelName);
            var overlay = SceneManager.LoadSceneAsync("Game Overlay", LoadSceneMode.Additive);

            while (!level.isDone && !overlay.isDone) yield return null;
            
            // Hacky hack
            yield return null;
            yield return null;
            yield return null;
            yield return null;

            State = GameState.Preparing;
        }
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