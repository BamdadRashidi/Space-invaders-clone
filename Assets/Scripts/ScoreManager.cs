using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    private static int totalScore;
    private static int hi_score;
    
    private void Awake()
    {
        totalScore = 0;
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void addScore(int ScoreValue)
    {
        totalScore += ScoreValue;
        Debug.Log(totalScore);
    }

    public void setHighScore()
    {
        if (hi_score < totalScore)
        {
            hi_score = totalScore;
            //TODO: Save the high_score somewhere
        }
    }
    
    
}