using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Wave : MonoBehaviour
{
    [SerializeField] private GameObject ThinEnemy;
    [SerializeField] private GameObject ThickEnemy;
    [SerializeField] private GameObject FollowEnemy;
    [SerializeField] private GameObject player;
    [SerializeField] private int rows;
    [SerializeField] private int columns;
    [SerializeField] private float distance;
    [SerializeField] private float speedTimer;
    private WaveManager wavemanager;
    private bool needToDescend;
    private Vector2 direction;
    private GameObject enemy;
    private float timer;
    private BoxCollider2D left;
    private BoxCollider2D right;
    public int AlienCount;
    public float tempSpeedTimer;
    private int coinflip;
    private const int AlienCountInit = 40;
    private bool isboosted = false;
    public AudioSource aud;
    void Start()
    {
        AlienCount = AlienCountInit;
        aud = GetComponent<AudioSource>();
        direction = Vector2.right;
        tempSpeedTimer = speedTimer;
        wavemanager = FindObjectOfType<WaveManager>();
        right = left = GetComponent<BoxCollider2D>();
        needToDescend = false;
        timer = speedTimer;
        CreateWave();
    }
    
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            if (wavemanager.waveEnded)
            {
                aud.Stop();
                return;
            }
            if (!needToDescend)
            {
                moveFormationH();
            }
            if (needToDescend)
            {
                moveFormationV();
            }

            ChangeSprite();
        }

        if (transform.childCount <= Mathf.Floor(0.3f * AlienCountInit) && !isboosted)
        {
            Debug.Log("Boosted");
            BoostAliens();
        }

        if (transform.position.y <= 3)
        {
            player.GetComponent<Player>().MakePlayerAggressive();
        }
    }

    public void CreateWave()
    {
        removeChildren();
        float width = (columns - 1) * distance;
        float height = (rows - 1) * distance;
        Vector3 offset = new Vector3(width / 2f, height / 2f, 0);
        
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if (i < 2)
                {
                    enemy = ThinEnemy;
                }

                else if (i >= 2 && i < 4)
                {
                    enemy = ThickEnemy;
                }
                
                else if (i >= 4)
                {
                    enemy = FollowEnemy;
                }
                
                Vector3 position = new Vector3(j * distance, i * distance,0) - offset;
                Instantiate(enemy, position, Quaternion.identity, transform);
            }
        }
        AlienCount = transform.childCount;
        
        if (AlienCount > AlienCountInit)
        {
            AlienCount = AlienCountInit;
        }
        
        if (!LifeManager.isInDeathSequence)
        {
            MakeEnemiesAggressive();
        }
    }

    public void moveFormationH()
    {
        aud.pitch = Random.Range(0.95f, 1.05f);
        aud.Play();
        transform.position += (Vector3)(direction * distance/2f);
        timer = tempSpeedTimer;
        return;
    }

    public void moveFormationV()
    {
        aud.pitch = Random.Range(0.95f, 1.05f);
        aud.Play();
        transform.position -= new Vector3(0,distance/2f);
        needToDescend = false;
        timer = tempSpeedTimer;
        return;
    }

    public void flipFormation()
    {
        direction *= -1;
        return;
    }

    public void resetWaveAtStart()
    {
        wavemanager.waveEnded = false;
        aud.volume = 0.35f;
        coinflip = Random.Range(0, 2);
        switch (coinflip)
        {
            case 0:
                direction = Vector2.left;
                break;
            case 1:
                direction = Vector2.right;
                break;
        }
        needToDescend = false;
        wavemanager.waveCount++;
        this.transform.position = new Vector3(0,17,0);
        isboosted = false;
        increaseDifficultyPerWave();
        CreateWave();
        UIManager.instance.UpdateWave(wavemanager.waveCount);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            needToDescend = true;
            flipFormation();
        }
    }

    public void reduceAlien()
    {
        AlienCount--;
        int aliensKilled = AlienCountInit - AlienCount;
        // Debug.Log(AlienCount);
        if (AlienCount <= 0 && !wavemanager.waveEnded)
        {
            wavemanager.waveEnded = true;
            MusicManager.instance.PlayVictory();
        }
        if (AlienCount != 0)
        {
            tempSpeedTimer = speedTimer - (0.015f * aliensKilled);
            tempSpeedTimer = Mathf.Max(tempSpeedTimer, 0.35f);
        }
    }

    public void increaseDifficultyPerWave()
    {
        speedTimer -= 0.1f;
        if (speedTimer <= 0.2f)
        {
            speedTimer = 0.25f;
        }
        tempSpeedTimer = speedTimer;
    }
    private void ChangeSprite()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.GetComponent<Enemy>().flipSprite();
        }
    }

    private void MakeEnemiesAggressive()
    {
        float difficultyValue = 0.03f * (wavemanager.waveCount - 1);
        foreach (Transform child in transform)
        {
            Enemy childScript = child.gameObject.GetComponent<Enemy>();
            childScript.firstOfRange -= difficultyValue;
            childScript.lastOfRange -= difficultyValue;
            childScript.firstOfRange = Mathf.Max(childScript.firstOfRange, 0.25f);
            childScript.lastOfRange = Mathf.Max(childScript.lastOfRange, 0.25f);
        }
    }
    private void BoostAliens()
    {
        foreach (Transform child in transform)
        {
            Enemy childScript = child.gameObject.GetComponent<Enemy>();
            childScript.firstOfRange -= Mathf.Floor(childScript.firstOfRange * 0.25f);
            childScript.lastOfRange -= Mathf.Floor(childScript.lastOfRange * 0.25f);
            childScript.firstOfRange = Mathf.Max(childScript.firstOfRange, 0.25f);
            childScript.lastOfRange = Mathf.Max(childScript.lastOfRange, 0.25f);
        }
        isboosted = true;
    }

    public void ResetWaveAtDeath()
    {
        removeChildren();
        
        transform.position = new Vector3(0, 17, 0);
        tempSpeedTimer = speedTimer;
        needToDescend = false;
        isboosted = false;
        AlienCount = AlienCountInit;
        CreateWave();
    }

    public void removeChildren()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

}