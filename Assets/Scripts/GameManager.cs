using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;
using Input = UnityEngine.Input;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private LifeManager lifemanager;
    private WaveManager waveManager;
    private Player player;
    private Wave wave;
    private ScoreManager ScoreManager;
    private GameObject[] Bullets;
    private bool isPaused = false;
    void Start()
    {
        waveManager = FindObjectOfType<WaveManager>();
        wave = FindObjectOfType<Wave>();
        player = FindObjectOfType<Player>();
        lifemanager = FindObjectOfType<LifeManager>();
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    
    public void StartDeathSequence()
    {
        StartCoroutine(DeathSequence());
    }

    IEnumerator DeathSequence()
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(1f);
        
        player.Die();
        yield return new WaitForSecondsRealtime(2f);
        
        if (LifeManager.isGameOvered)
        {
            StartCoroutine("GameOverSequence");
        }
        ScoreManager.Instance.ResetScoreBack();
        player.resetPlayer();
        player.enabled = true;
        
        DestroyAllBullets();
        Reset();
        
        Time.timeScale = 1f;
        UIManager.instance.GameOverText.enabled = false;
        LifeManager.isInDeathSequence = false;
    }

    private void Update()
    {
        //Pausing
        if (!isPaused && Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0;
            isPaused = true;
            player.GetComponent<Player>().enabled = false;
        }
        else if (isPaused && Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 1;
            isPaused = false;
            player.GetComponent<Player>().enabled = true;
        }
    }

    public void Reset()
    {
        wave.ResetWaveAtDeath();
    }
    
    void DestroyAllBullets()
    {
        foreach (GameObject obj in FindObjectsOfType<GameObject>())
        {
            if (obj.layer == LayerMask.NameToLayer("Bullet") ||
                obj.layer == LayerMask.NameToLayer("EnemyBullet"))
            {
                Destroy(obj);
            }
        }
    }

    IEnumerator GameOverSequence()
    {
        ScoreManager.Instance.setHighScore();
        WaveManager wm = FindObjectOfType<WaveManager>();
        int reachedWave = wm ? wm.waveCount : 1;
        ScoreManager.Instance.SaveToFile(reachedWave);
        
        UIManager.instance.GameOverText.enabled = true;
        
        yield return new WaitForSecondsRealtime(3f);
        
        // reset everything
        waveManager.ResetWaveNumber();
        ScoreManager.Instance.ResetScore();
        lifemanager.ResetLives();
        Time.timeScale = 1f;
    }
    
}
