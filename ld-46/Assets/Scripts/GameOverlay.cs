using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverlay : MonoBehaviour
{
    public void StartGame()
    {
        GameManager.Instance.StartLevel();
    }
}
