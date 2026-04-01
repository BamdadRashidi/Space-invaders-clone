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
    [SerializeField] private AudioClip[] clips;
    [SerializeField] private GameObject movementAud;
    public AudioSource movementsrc;
    private ParticleSystem muzzlePart;
    void Start()
    {
        muzzlePart = movementAud.GetComponent<ParticleSystem>();
        initialPosition = new Vector3(0,-25,0);
        died = false;
        timerCounter = fireRate;
        collider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        particles = GetComponent<ParticleSystem>();
        renderer = GetComponent<SpriteRenderer>();
        aud = GetComponent<AudioSource>();
        movementsrc = movementAud.GetComponent<AudioSource>();
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
        if (direction != 0)
        {
            if (!movementsrc.isPlaying)
            {
                movementsrc.pitch = Random.Range(0.95f, 1.05f);
                movementsrc.Play();
            }
        }
        else
        {
            if (movementsrc.isPlaying)
                movementsrc.Stop();
        }
        rb.velocity = new Vector2(direction * speed, rb.velocity.y);
    }

    public void Shoot()
    {
        muzzlePart.Play();
        aud.clip = clips[0];
        aud.volume = 0.5f;
        aud.pitch = Random.Range(0.95f, 1.05f);
        aud.Play();
        Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Bullet") &&
            other.gameObject.layer != LayerMask.NameToLayer("Wall"))
        {
            died = true;
            Destroy(other.gameObject,0.2f);
        }
    }
    
    public void resetPlayer()
    {
        transform.position = initialPosition;
        collider.enabled = true;
        renderer.enabled = true;
        died = false;
        movementsrc.volume = 0.32f;
    }


    public void Die()
    {
        muzzlePart.Stop();
        aud.clip = clips[1];
        aud.volume = 0.8f;
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
    

}
