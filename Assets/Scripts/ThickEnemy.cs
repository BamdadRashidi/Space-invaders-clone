using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThickEnemy : Enemy
{
    [SerializeField] private Transform bulletSpawnPos;
    [SerializeField] private GameObject bullet;
    public override void shoot()
    {
        aud.pitch = Random.Range(0.9f, 1.1f);
        aud.volume = Random.Range(0.1f, 0.8f);
        aud.clip = clips[0];
        aud.Play();
        Instantiate(bullet, bulletSpawnPos.position, Quaternion.identity);
    }

    void Start()
    {
        timer = Random.Range(firstOfRange, lastOfRange);
    }
    
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            shoot();
            timer = Random.Range(firstOfRange, lastOfRange);
        }
    }
    
    protected override void GiveScore()
    {
        int index = Random.Range(3, 5);
        ScoreManager.Instance.addScore(scores[index]);
    }
    

}