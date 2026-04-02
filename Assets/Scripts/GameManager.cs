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
    [SerializeField] private GameObject whiteFlash;
    [SerializeField] private Camera cam;
    void Start()
    {
        cam.GetComponent<Animator>().enabled = false;
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
        whiteFlash.GetComponent<Animator>().enabled = false;  
    }
    
    public void StartDeathSequence()
    {
        whiteFlash.GetComponent<Animator>().enabled = true;  
        whiteFlash.GetComponent<Animator>().Play("whiteFlash",0,0f);
        whiteFlash.GetComponent<AudioSource>().Play();
        StartCoroutine(DeathSequence());
    }

    IEnumerator DeathSequence()
    {
        
        Time.timeScale = 0;
        player.movementsrc.volume = 0;
        UFO.instance.aud.volume = 0;
        yield return new WaitForSecondsRealtime(2f);
        
        player.Die();
        cam.GetComponent<Animator>().enabled = true;
        cam.GetComponent<Animator>().Play("CameraShake",0,0f);
        yield return new WaitForSecondsRealtime(2f);
        
        if (LifeManager.isGameOvered)
        {
            StartCoroutine("GameOverSequence");
            yield break;
        }
        GeneralReset();
    }

    private void Update()
    {
        //Pausing
        if (!LifeManager.isInDeathSequence)
        {
            if (!isPaused && Input.GetKeyDown(KeyCode.Escape))
            {
                Time.timeScale = 0;
                isPaused = true;
                player.GetComponent<Player>().enabled = false;
                player.movementsrc.Stop();
                UFO.instance.aud.volume = 0;
            }
            else if (isPaused && Input.GetKeyDown(KeyCode.Escape))
            {
                Time.timeScale = 1;
                isPaused = false;
                player.GetComponent<Player>().enabled = true;
                UFO.instance.aud.volume = 0.5f;
            }
        }
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
        waveManager.setHighestWave();
        waveManager.RemoveBunkers();
        ScoreManager.Instance.SaveToFile(waveManager.getHighestWave());
        wave.removeChildren();
        waveManager.RemoveBunkers();
        DestroyAllBullets();
        UIManager.instance.GameOverPanel.SetActive(true);
        UIManager.instance.UpdateGameOver(ScoreManager.Instance.GetTotalScore(),waveManager.waveCount);
        yield return new WaitForSecondsRealtime(5f);
        
        // reset everything
        lifemanager.ResetLives();
        waveManager.ResetWaveNumber();
        waveManager.ResetWaveState();
        ScoreManager.Instance.ResetScore();
        UFOSpawner.instance.Reroll();
        GeneralReset();
        waveManager.ResetWaveNumber();
        ScoreManager.Instance.ResetScore();
        UIManager.instance.GameOverPanel.SetActive(false);
        Time.timeScale = 1f;
    }


    void GeneralReset()
    {
        ScoreManager.Instance.ResetScoreBack();
        player.resetPlayer();
        player.enabled = true;
        DestroyAllBullets();
        waveManager.ResetWaveState();
        waveManager.CreateBunkers();
        wave.ResetWaveAtDeath();
        LifeManager.isInDeathSequence = false;
        LifeManager.isGameOvered = false;
        Time.timeScale = 1f;
        UFO.instance.aud.volume = 0.5f;
    }
}
