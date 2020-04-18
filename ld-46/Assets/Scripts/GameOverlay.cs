using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameOverlay : MonoBehaviour
{
    public TMP_Text gameState;
    public TMP_Text levelName;
    
    public Color runningColor;
    public Color preparingColor;
    public UIButton startButton;

    public Level level;
    
    private void Awake()
    {
        GameManager.Instance.OnLevelPrepare.AddListener(SetPrepare);
        GameManager.Instance.OnLevelRun.AddListener(SetRunning);
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
}
