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
    void Awake()
    {
        player = FindObjectOfType<Player>();
        wave = FindObjectOfType<Wave>();
        waveEnded = false;
        timer = timerToNextWave;
        waveCount = 1;
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
        }

        if (player.died && !waveEnded)
        {
            setHighestWave();
        }

        if (state == waveCount)
        {
            GameObject[] ExistingBunkers = GameObject.FindGameObjectsWithTag("Bunker");
            if (ExistingBunkers.Length == 0)
            {
                Instantiate(bunkers, new Vector3(11,-8,0.2f),Quaternion.identity);
            }
            LifeManager.Instance.lives += 2;
            UIManager.instance.UpdateLives(LifeManager.Instance.lives);
            state += 5; 
        }
    }

    private void setHighestWave()
    {
        highWave = waveCount;
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
    }
    
}
