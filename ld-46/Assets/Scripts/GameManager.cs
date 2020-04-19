using System.Collections;
using Newtonsoft.Json;
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
            Vector2 cursorHotspot;
            switch (value)
            {
                case GameState.Preparing:
                    OnLevelPrepare?.Invoke();
                    cursorHotspot= new Vector2 (addFanIcon.width / 2, addFanIcon.height / 2);
                    Cursor.SetCursor(addFanIcon, cursorHotspot, CursorMode.ForceSoftware);
                    break;
                case GameState.Running:
                    OnLevelRun?.Invoke();
                    cursorHotspot = new Vector2 (noMoreFanIcon.width / 2, noMoreFanIcon.height / 2);
                    Cursor.SetCursor(noMoreFanIcon, cursorHotspot, CursorMode.ForceSoftware);
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
    
    public GameObject fanPrefab;
    public float pressThreshold = 0.3f;
    
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
        GameOverlay.Instance.level.AddTry();
    }

    public void ReachedFinishLine()
    {
        State = GameState.Preparing;
    }
}

public enum GameState
{
    MainMenu,
    Preparing,
    Running
}