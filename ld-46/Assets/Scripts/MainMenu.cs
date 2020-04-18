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

    private Dictionary<string, bool> levels;

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
                    GameManager.Instance.LoadLevel(level.Key);
                });
            }
        }
    }

    private void CheckUnlockState()
    {
        var pref = PlayerPrefs.GetString("unlocked_levels", "['Tutorial','Game']");
        string[] unlocked = JsonConvert.DeserializeObject<string[]>(pref);

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
}