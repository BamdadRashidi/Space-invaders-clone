using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveSetter : MonoBehaviour
{
    public static WaveSetter instance;
    private bool isInGame = false;
    private bool gotReferences = false;
    private Wave wave;
    private WaveManager waveM;
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
        if (SceneManager.GetActiveScene().name == "Maingame")
        {
            isInGame = true;
        }

        if (!gotReferences && wave == null && waveM == null && isInGame)
        {
            wave = FindObjectOfType<Wave>();
            waveM = FindObjectOfType<WaveManager>();
            gotReferences = true;
        }
    }

    public void SetWave(int waveNumber)
    {
        Debug.Log("Set Wave number: " + waveNumber);
        if (isInGame && gotReferences)
        {
            return;
        }
    }
}
