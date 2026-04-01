using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class UFO : Enemy
{
    private float lifeTimer;
    private float speedTimer;
    private int speed; 
    private Vector2 movementdir;
    private int[] speedValues = { 101, 75, 120, 130, 36, 127, 140, 26, 69, 148, 150, 142, 138, 67, 164, 420 };
    private UFOSpawner spawner;
    private WaveManager waveM;
    public static UFO instance { get; private set; }
    public override void shoot()
    {
        return;
    }
    
    void Start()
    {
        anim.enabled = true;
        anim.Play("UFO");
        waveM = FindObjectOfType<WaveManager>();
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        spawner = FindObjectOfType<UFOSpawner>();
        lifeTimer = Random.Range(30, 70);
        speed = 60;
        timer = Random.Range(2, 5);
        speedTimer = Random.Range(2,5);
        if (spawner.CoinFlip == 1)
        {
            movementdir = Vector2.left;
        }
        else
        {
            movementdir = Vector2.left;
        }
    }

   
    void Update()
    {
        transform.position += new Vector3(movementdir.x * speed * Time.deltaTime, movementdir.y, 0);
        timer -= Time.deltaTime;
        speedTimer -= Time.deltaTime;
        lifeTimer -= Time.deltaTime;
        if (timer <= 0)
        {
            flipDirection();
            timer = Random.Range(0.5f, 2.5f);
        }
        if (speedTimer <= 0)
        {
            alterSpeed();
        }

        if (lifeTimer <= 0)
        {
            vanish();
        }
        if (this.transform.position.x > 125 || this.transform.position.x < -125)
        {
            flipDirection();
            timer = Random.Range(2f, 4.5f);
        }

        if (waveM.waveEnded)
        {
            collider.enabled = false;
        }
        else
        {
            collider.enabled = true;
        }

    }

    private void flipDirection()
    {
        movementdir *= -1;
    }

    private void alterSpeed()
    {
        int indexes = Random.Range(0, speedValues.Length);
        speed = speedValues[indexes];
        if (indexes == 15)
        {
            speedTimer = 0.5f;
        }
        else
        {
            speedTimer = Random.Range(0.5f,5.3f);
        }
    }

    private void vanish()
    {
        //TODO: add vanishing animation, audio and sound
        Destroy(this.gameObject,1);
    }
    protected override void GiveScore()
    {
        int index = Random.Range(9, 11);
        ScoreManager.Instance.addScore(scores[index]);
    }



    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}