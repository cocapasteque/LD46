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

    public Level level;

    public GameObject FanInfo;
    public Slider ForceSlider;
    public TextMeshProUGUI ForcePercentage;
    public Slider RotationSlider;
    public TextMeshProUGUI RotationAngle;
    
    private void Awake()
    {
        GameManager.Instance.OnLevelPrepare.AddListener(SetPrepare);
        GameManager.Instance.OnLevelRun.AddListener(SetRunning);
        DeselectFan();
    }

    private void Update()
    {
        ForcePercentage.text = (ForceSlider.normalizedValue * 100).ToString("F0") + "%";
        RotationAngle.text = (RotationSlider.normalizedValue * 360).ToString("F0");
    }

    public void StartGame()
    {
        GameManager.Instance.StartLevel();
    }
    void SetPrepare()
    {
        gameState.text = "Preparing";
        gameState.color = preparingColor;
        startButton.EnableButton();
        
        level = GameObject.FindObjectOfType<Level>();
        levelName.text = level.levelName;
    }
    void SetRunning()
    {
        gameState.text = "Running";
        gameState.color = runningColor;
        startButton.DisableButton();
    }   
    
    public void SelectFan()
    {
        FanInfo.SetActive(true);
    }

    public void DeselectFan()
    {
        FanInfo.SetActive(false);
    }
}
