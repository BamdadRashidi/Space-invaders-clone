using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

public class UFOSpawner : MonoBehaviour
{
    public int CoinFlip;
    [SerializeField] private Transform leftSpawner;
    [SerializeField] private Transform rightSpawner;
    [SerializeField] private GameObject UFO;
    private WaveManager waveManager;
    public static UFOSpawner instance;
    private float SpawnTimer;
    void Start()
    {
        CoinFlip = Random.Range(0, 2);
        waveManager = FindObjectOfType<WaveManager>();
        SpawnTimer = Random.Range(300,1200); // 300,1200
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }
    
    void Update()
    {
        SpawnTimer -= Time.deltaTime;
        if (SpawnTimer <= 0 || waveManager.waveCount % 20 == 0 || waveManager.waveCount % 35 == 0 || waveManager.waveCount == 10)
        {
            if (CoinFlip == 1)
            {
                Instantiate(UFO.gameObject, rightSpawner.position, Quaternion.identity);
            }
            else
            {
                Instantiate(UFO.gameObject, leftSpawner.position, Quaternion.identity);
            }
            SpawnTimer = SpawnTimer = Random.Range(900, 1800);
            CoinFlip = Random.Range(0, 2);
        }
    }

    public void Reroll()
    {
        SpawnTimer = Random.Range(300,1200);
    }
}
