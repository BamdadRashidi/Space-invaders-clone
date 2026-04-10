using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private float timerToNextWave;
    [SerializeField] private GameObject bunkers;
    private Player player;
    public int waveCount;
    private int highWave;
    private float timer;
    public Wave wave;
    public bool waveEnded;
    private int state = 5;
    private bool diedInthisWave = false;
    void Awake()
    {
        player = FindObjectOfType<Player>();
        wave = FindObjectOfType<Wave>();
        waveEnded = false;
        timer = timerToNextWave;
        waveCount = 1;
    }

    private void Start()
    {
        SaveEntity data = ScoreManager.Instance.LoadFromFile();
        highWave = data.highestWave;
    }

    void Update()
    {
        if (!waveEnded || LifeManager.isInDeathSequence) return;

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            ScoreManager.Instance.FinalizeScore();
            wave.resetWaveAtStart();
            timer = timerToNextWave;
            player.chargeChances();
        }

        if (player.died && !waveEnded)
        {
            diedInthisWave = true;
            setHighestWave();
        }

        if (state == waveCount)
        {
            CreateBunkers();
            LifeManager.Instance.lives += 3;
            UIManager.instance.UpdateLives(LifeManager.Instance.lives);
            state += 5; 
            GameManager.instance.PlaySound();
        }
    }

    public void setHighestWave()
    {
        if (highWave < waveCount)
        {
            highWave = waveCount;
        }
        UIManager.instance.UpdateWave(highWave);
        return;
    }

    public void ResetWaveNumber()
    {
        waveCount = 1;
        UIManager.instance.UpdateWave(1);
    }
    
    public void ResetWaveState()
    {
        waveEnded = false;
        timer = timerToNextWave;
        diedInthisWave = false;
    }

    public void RemoveBunkers()
    {
        GameObject[] existingBunkers = GameObject.FindGameObjectsWithTag("Bunker");
        foreach (GameObject bunker in existingBunkers)
        {
            Destroy(bunker);
        }
    }

    public void CreateBunkers()
    {
        Destroy(bunkers);
        GameObject[] ExistingBunkers = GameObject.FindGameObjectsWithTag("Bunker");
        if (ExistingBunkers.Length < 3 && !diedInthisWave)
        {
            Instantiate(bunkers, new Vector3(11,-8,0.2f),Quaternion.identity);
        }
    }

    public int getHighestWave()
    {
        return highWave;
    }
    
}
