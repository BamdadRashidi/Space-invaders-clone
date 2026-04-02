using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.IO;
public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public TextMeshProUGUI Score;
    public TextMeshProUGUI HiScore;
    public TextMeshProUGUI Wave;
    public TextMeshProUGUI LiveCount;
    public GameObject GameOverPanel;
    public TextMeshProUGUI ScoreTextGO;
    public TextMeshProUGUI HiScoreTextGo;
    public TextMeshProUGUI WaveTextGo;
    public TextMeshProUGUI HiWaveTextGo;
    private static string filePath;
    void Awake()
    {
        instance = this;
        GameOverPanel.SetActive(false);
    }

    private void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, "Save.json");
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

    public void UpdateGameOver(int score, int waves)
    {
        ScoreTextGO.text = "Total score: " + score;
        WaveTextGo.text = "Wave reached: " + waves;
        string json = File.ReadAllText(filePath);
        SaveEntity data = JsonUtility.FromJson<SaveEntity>(json);
        HiScoreTextGo.text = "Hi-Score: " + data.highScore;
        HiWaveTextGo.text = "Highest wave: " + data.highestWave;
    }
}
