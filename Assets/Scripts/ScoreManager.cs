using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    private static int totalScore;
    private static int hi_score;
    private static string filePath;
    public int ScoreForThisWaveInit = 0;
    private void Awake()
    {
        // ResetSave();
        totalScore = 0;
        filePath = Path.Combine(Application.persistentDataPath, "Save.json");
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }

        LoadFromFile();
    }

    public void FinalizeScore()
    {
        ScoreForThisWaveInit = totalScore;
    }

    public void ResetScoreBack()
    {
        totalScore = ScoreForThisWaveInit;
        UIManager.instance.UpdateScore(totalScore);
    }
    public void addScore(int ScoreValue)
    {
        totalScore += ScoreValue;
        UIManager.instance.UpdateScore(totalScore);
    }

    public void setHighScore()
    {
        if (hi_score < totalScore)
        {
            hi_score = totalScore;
            UIManager.instance.UpdateHighScore(hi_score);
        }
    }
    
    //Saves at C:\Users\<You>\AppData\LocalLow\<CompanyName>\<ProductName>\save.json

    public void SaveToFile(int highestWave)
    {
        SaveEntity saveEntity = new SaveEntity()
        {
            highScore = hi_score,
            highestWave = highestWave
        };
        string json = JsonUtility.ToJson(saveEntity,true);
        File.WriteAllText(filePath, json);
    }

    public void LoadFromFile()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            SaveEntity data = JsonUtility.FromJson<SaveEntity>(json);
            hi_score = data.highScore;
            if (UIManager.instance != null)
            {
                UIManager.instance.UpdateHighScore(hi_score);
            }
        }
        else
        {
            if (UIManager.instance != null)
            {
                UIManager.instance.UpdateHighScore(0);
            }
        }
    }
    
    public void ResetSave()
    {
        if (File.Exists(filePath)) File.Delete(filePath);
        totalScore = 0;
        hi_score = 0;
        Debug.Log("Save file deleted.");
    }

    public void ResetScore()
    {
        totalScore = 0;
        UIManager.instance.UpdateScore(totalScore);
    }

    public int GetHighScore()
    {
        return hi_score;
    }
}

[System.Serializable]
public class SaveEntity
{
    public int highScore;
    public int highestWave;
}