using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public TextMeshProUGUI Score;
    public TextMeshProUGUI HiScore;
    public TextMeshProUGUI Wave;
    public TextMeshProUGUI LiveCount;
    public TextMeshProUGUI GameOverText;
    void Awake()
    {
        instance = this;
        if (GameOverText != null)
        {
            GameOverText = GameObject.Find("Game Over").GetComponent<TextMeshProUGUI>();
        }
        GameOverText.gameObject.SetActive(false);
    }

    private void Start()
    {
        if (ScoreManager.Instance != null)
        {
            if (ScoreManager.Instance != null)
            {
                UpdateHighScore(ScoreManager.Instance.GetHighScore());
            }
        }
    }

    public void UpdateScore(int score)
    {
        Score.text = "Score: " + score;
    }

    public void UpdateHighScore(int hiscore)
    {
        HiScore.text = "Hi-Score: " + hiscore;
    }
    
    public void UpdateWave(int wave)
    {
        Wave.text = "Wave: " + wave;
    }
    
    public void UpdateLives(int life)
    {
        LiveCount.text = "Lives: " + life;
    }
}
