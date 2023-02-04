using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float roundTime = 60f;

    public float countDownTime = 3f;

    public bool canEnemiesAttack = true;

    private bool isLevelWon = false;

    private bool isLevelLost = false;

    private UIManager uiManager;

    private void Awake()
    {
        uiManager = FindObjectOfType<UIManager>();
        Time.timeScale = 1;
        isLevelWon = false;
        isLevelLost = false;
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void StartGame() => MovementBase.isGameStarted = true;

    public void LoseGame()
    {
        isLevelLost = true;
        if (!isLevelWon)
        {
            MovementBase.isGameFinished = true;
            uiManager.LoseGame();
            uiManager.isLevelFinished = true;
        }

    }

    public void WinGame()
    {
        isLevelWon = true;
        if (!isLevelLost)
        {
            MovementBase.isGameFinished = true;
            uiManager.WinGame();
            uiManager.isLevelFinished = true;
        }

    }

    public void TimeOut()
    {
        isLevelWon = true;
        isLevelLost = true;

        MovementBase.isGameFinished = true;
        uiManager.TimeOut();
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
    }
}
