using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowEnemy : Enemy
{
    [SerializeField] private Transform bulletSpawnPos;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject player;
    public override void shoot()
    {
        if (!player.GetComponent<Player>().died)
        {
            Vector2 aim = (player.transform.position - bulletSpawnPos.transform.position).normalized;
            float angle = Mathf.Atan2(aim.y, aim.x) * Mathf.Rad2Deg;
            aud.pitch = Random.Range(0.9f, 1.1f);
            aud.volume = Random.Range(0.1f, 0.8f);
            aud.clip = clips[0];
            aud.Play();
            GameObject b = Instantiate(bullet, bulletSpawnPos.position, Quaternion.Euler(0,0,angle - 90f));
            b.GetComponent<EnemyBullets>().setDirection(aim);
        }
        else
        {
            return;
        }
    }

    void Start()
    {
        timer = Random.Range(firstOfRange, lastOfRange);
        player = GameObject.FindWithTag("Player");
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
        int index = Random.Range(6, 8);
        ScoreManager.Instance.addScore(scores[index]);
    }
    

}
