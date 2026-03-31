using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour

{
    private Vector2 direction = Vector2.zero;
    private Vector2 initialPosition;
    [SerializeField] public int speed;
    [SerializeField] private float fireRate; 
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] public bool died;
    private float timerCounter;
    private BoxCollider2D collider;
    private ParticleSystem particles;
    private SpriteRenderer renderer;
    private AudioSource aud;
    private int AudioCoinFlip;
    [SerializeField] private AudioClip[] clips; 
    void Start()
    {
        initialPosition = new Vector3(0,-25,0);
        died = false;
        timerCounter = fireRate;
        collider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        particles = GetComponent<ParticleSystem>();
        renderer = GetComponent<SpriteRenderer>();
        aud = GetComponent<AudioSource>();
    }
    
    void Update()
    {
        timerCounter -= Time.deltaTime;
        if (Input.GetKey(KeyCode.Space) && timerCounter <= 0f)
        {
            Shoot();
            timerCounter = fireRate;
        }
    }
    
    void FixedUpdate()
    {
        float direction = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(direction * speed, rb.velocity.y);
    }

    public void Shoot()
    {
        RollVolume();
        aud.volume = 0.6f;
        aud.pitch = Random.Range(0.9f, 1.1f);
        aud.Play();
        Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Bullet") &&
            other.gameObject.layer != LayerMask.NameToLayer("Wall"))
        {
            died = true;
            Destroy(other.gameObject);
        }
    }
    
    public void resetPlayer()
    {
        transform.position = initialPosition;
        collider.enabled = true;
        renderer.enabled = true;
        died = false;
    }


    public void Die()
    {
        aud.clip = clips[3];
        aud.volume = 1f;
        aud.pitch = Random.Range(0.9f, 1f);
        aud.Play();
        var main = particles.main;
        main.useUnscaledTime = true;
        particles.Play();
        collider.enabled = false;
        renderer.enabled = false;
        rb.velocity = Vector2.zero;
        this.enabled = false;
    }

    private void RollVolume()
    {
        AudioCoinFlip = Random.Range(0, 3);
        switch (AudioCoinFlip)
        {
            case 0:
                aud.clip = clips[0];
                break;
            case 1:
                aud.clip = clips[1];
                break;
            case 2:
                aud.clip = clips[2];
                break;
        }
    }

}
