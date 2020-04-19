using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameOverlay : Singleton<GameOverlay>
{
    public TMP_Text gameState;
    public TMP_Text levelName;
    
    public Color runningColor;
    public Color preparingColor;
    public UIButton startButton;
    public UIButton stopButton;

    public Level level;

    public GameObject FanInfo;
    public Slider ForceSlider;
    public TextMeshProUGUI ForcePercentage;
    public Slider RotationSlider;
    public TextMeshProUGUI RotationAngle;

    public TextMeshProUGUI Tries;
    public TextMeshProUGUI Fans;
    public TextMeshProUGUI RunTime;

    private bool running = false;
    private float currentTime;
    
    private void Awake()
    {
        GameManager.Instance.OnLevelPrepare.AddListener(SetPrepare);
        GameManager.Instance.OnLevelRun.AddListener(SetRunning);
        DeselectFan();
        stopButton.gameObject.SetActive(false);        
    }

    private void Update()
    {
        ForcePercentage.text = (ForceSlider.normalizedValue * 100).ToString("F0") + "%";
        RotationAngle.text = (RotationSlider.normalizedValue * 360).ToString("F0") + "°";
    }

    public void StartGame()
    {
        GameManager.Instance.StartLevel();
        level.DeselectAllFans();
    }

    public void StopGame()
    {
        GameManager.Instance.KillPlayer();
    }

    void SetPrepare()
    {
        gameState.text = "Preparing";
        gameState.color = preparingColor;
        startButton.gameObject.SetActive(true);
        stopButton.gameObject.SetActive(false);
        
        level = GameObject.FindObjectOfType<Level>();
        levelName.text = level.levelName;
        running = false;
        UpdateTries();
    }
    void SetRunning()
    {
        gameState.text = "Running";
        gameState.color = runningColor;
        startButton.gameObject.SetActive(false);
        stopButton.gameObject.SetActive(true);
        running = true;
        StartCoroutine(Timer());
    }   
    
    public void SelectFan()
    {
        FanInfo.SetActive(true);
    }

    public void DeselectFan()
    {
        FanInfo.SetActive(false);
    }

    public void DeleteFan()
    {
        level.RemoveCurrentFan();
        FanInfo.SetActive(false);
    }

    private IEnumerator Timer()
    {
        currentTime = 0f;
        while(running)
        {
            yield return null;
            currentTime += Time.deltaTime;
            RunTime.text = currentTime.ToString("N3");
        }
    }

    public void UpdateTries()
    {
        Tries.text = level.tries.ToString();
    }
}
