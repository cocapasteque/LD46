using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.UI;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : SerializedMonoBehaviour
{
    public string[] levelNames;

    public GameObject unlockedLevel;
    public GameObject lockedLevel;
    public Transform levelContainers;

    public LevelSelection levelSelection;
    
    private Dictionary<string, bool> levels;
    private List<GameObject> levelBtns;
    
    
    void Start()
    {
        InitializeLevels();
        CheckUnlockState();
        InstantiateLevelButtons();
    }

    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void InstantiateLevelButtons()
    {
        levelBtns = new List<GameObject>();
        foreach (var level in levels)
        {
            Debug.Log("Creating button for " + level.Key);
            var btn = Instantiate(level.Value ? unlockedLevel : lockedLevel, levelContainers);
            btn.GetComponentInChildren<TMP_Text>().text = level.Value ? level.Key : "Locked";

            // If level unlocked, hook it to level load logic
            if (level.Value)
            {
                btn.GetComponent<UIButton>().OnClick.OnTrigger.Event.AddListener(() =>
                {
                    var key = level.Key.Replace(" ", string.Empty).ToLower();
                    levelSelection.LoadLeaderboard(key);
                    levelSelection.selectedLevel = level.Key;
                    levelSelection.playButton.SetActive(true);
                });
            }
            levelBtns.Add(btn);
        }
    }

    private void CheckUnlockState()
    {
        var pref = PlayerPrefs.GetString("unlocked_levels", "['Tutorial','Level 1']");
        string[] unlocked = JsonConvert.DeserializeObject<string[]>(pref);
        if (unlocked == null) unlocked = new []{"Tutorial", "Level 1"};
        Debug.Log(string.Join(",", unlocked));
        foreach (var level in unlocked)
        {
            // unlocking level in dictionary if in player prefs.
            if (levels.ContainsKey(level))
                levels[level] = true;
        }
    }

    private void InitializeLevels()
    {
        levels = new Dictionary<string, bool>();
        foreach (var level in levelNames)
        {
            levels.Add(level, false);
        }
    }

    public void ResetProgress()
    {
        Debug.Log("Reseting progress");
        PlayerPrefs.SetString("unlocked_levels", "['Tutorial', 'Level 1']");

        foreach (var level in levelNames)
        {
            PlayerPrefs.SetInt("Tries_" + level, 1);
        }
        
        foreach (var btn in levelBtns)
        {
            Destroy(btn);
        }
        levelBtns.Clear();
        InitializeLevels();
        CheckUnlockState();
        InstantiateLevelButtons();
    }
}