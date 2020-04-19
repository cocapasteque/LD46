using System.Collections;
using System.Collections.Generic;
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
    public UnityEvent OnBeforePlayerDied;
    public UnityEvent OnPlayerDied;
    public UnityEvent OnLevelCompleted;
    
    public Texture2D addFanIcon;
    public Texture2D noMoreFanIcon;

    public int currentLevel;
    public GameObject fanPrefab;
    public float pressThreshold = 0.3f;
    public List<string> scenesInBuild = new List<string>();

    void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(transform.root);
        }
        
        State = GameState.MainMenu;

        for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            int lastSlash = scenePath.LastIndexOf("/");
            scenesInBuild.Add(scenePath.Substring(lastSlash + 1, scenePath.LastIndexOf(".") - lastSlash - 1));
        }
    }

    public void LoadLevel(string levelName)
    {
        if (levelName.StartsWith("Level"))
        {
            currentLevel = int.Parse(levelName.Split(' ')[1]);
        }
        else
        {
            currentLevel = 0;
        }
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
        StartCoroutine(Work());
        IEnumerator Work()
        {
            OnBeforePlayerDied?.Invoke();
            yield return new WaitForSeconds(2);
            OnPlayerDied?.Invoke();
            State = GameState.Preparing;
            GameOverlay.Instance.level.AddTry();
        }
    }

    public void ReachedFinishLine()
    {
        OnLevelCompleted?.Invoke();
        State = GameState.Completed;
    }

    public void LoadMainMenu()
    {
        StartCoroutine(Work());
        
        IEnumerator Work()
        {
            var menu = SceneManager.LoadSceneAsync("Main Menu");
            var overlay = SceneManager.UnloadSceneAsync("Game Overlay");

            while (!menu.isDone && !overlay.isDone) yield return null;
        }
    }
}

public enum GameState
{
    MainMenu,
    Preparing,
    Running,
    Completed
}