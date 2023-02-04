using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int score = 0;

    private UIManager uiManager;
    private void Awake()
    {
        uiManager = FindObjectOfType<UIManager>();
    }

    public void AddScore(int amount)
    {
        score += amount;

        DisplayScore();
    }

    private void DisplayScore()
    {
        uiManager.DisplayScore(score);
    }

    public GameObject HighScoreOwner()
    {
        float highScore = 0; // Default
        GameObject owner = GameObject.FindGameObjectWithTag("Player"); // Default

        MovementBase[] objects = GameObject.FindObjectsOfType(typeof(MovementBase)) as MovementBase[];

        // We check here the high score.
        for (int i = 0; i < objects.Length; i++)
        {
            int newScore = objects[i].score;
            if (newScore > highScore)
            {
                highScore = newScore;
                owner = objects[i].gameObject;
            }
            
        }

        return owner;
    }
}
