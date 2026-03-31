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
        GameOverText.enabled = false;
        instance = this;
    }

    public void UpdateScore(int score)
    {
        Score.text = "Score: " + score;
    }

    public void UpdateHighScore(int Hiscore)
    {
        HiScore.text = "Hi-Score: " + HiScore;
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
