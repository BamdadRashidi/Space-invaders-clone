using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveSetter : MonoBehaviour
{
    public static WaveSetter instance;
    private Wave wave;
    private WaveManager waveM;
    private int targetWave = -1;
    private void Awake()
    {
        if (instance != this && instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    

    void Update()
    {
        if (SceneManager.GetActiveScene().name != "Maingame")
        {
            return;
        }

        if (wave == null)
        {
            wave = FindObjectOfType<Wave>();
        }

        if (waveM == null)
        {
            waveM = FindObjectOfType<WaveManager>();
        }

        if (wave != null && waveM != null && targetWave > 0)
        {
            ApplyWave();
        }
    }

    public void SetWave(int waveNumber)
    {
        Debug.Log("Set Wave number: " + waveNumber);
        targetWave = waveNumber;
    }

    public void ApplyWave()
    {
        waveM.waveCount = targetWave - 1;
        for (int i = 1; i < targetWave; i++)
        {
            wave.increaseDifficultyPerWave();
        }
        wave.resetWaveAtStart();
        UIManager.instance.UpdateWave(targetWave);
        targetWave = -1;
    }
}
