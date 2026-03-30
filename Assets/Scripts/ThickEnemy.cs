using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThickEnemy : Enemy
{
    [SerializeField] private Transform bulletSpawnPos;
    [SerializeField] private GameObject bullet;
    public override void shoot()
    {
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