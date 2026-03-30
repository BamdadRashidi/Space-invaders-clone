using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private LifeManager lifemanager;
    private Player player;
    private Wave wave;
    private GameObject[] Bullets;
    private bool isPaused = false;
    void Start()
    {
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
        
        player.resetPlayer();
        player.enabled = true;
        
        DestroyAllBullets();
        Reset();
        
        Time.timeScale = 1f;

        LifeManager.isInDeathSequence = false;
    }

    private void Update()
    {
        // Pausing
        
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

}
