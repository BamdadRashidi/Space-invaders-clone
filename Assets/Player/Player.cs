using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    private Vector2 direction = Vector2.zero;
    private Vector2 initialPosition;
    private float initialFireRate;
    private int initialSpeed;
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
    private int chances = 2;
    private bool isHurt = false;
    private float IFramesInit = 2f;
    private float IframesTimer;
    [SerializeField] private GameObject smokeParticleGo;
    private ParticleSystem smokePart;    
    [SerializeField] private GameObject sparkelParticleGO;
    private ParticleSystem sparks;
    private Animator anim;
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
        initialFireRate = fireRate;
        initialSpeed = speed;
        IframesTimer = IFramesInit;
        smokePart = smokeParticleGo.GetComponent<ParticleSystem>();
        sparks = sparkelParticleGO.GetComponent<ParticleSystem>();
        anim = GetComponent<Animator>();
        anim.enabled = false;
    }
    
    void Update()
    {

        if (transform.position.x <= -59 || transform.position.x >= 59)
        {
            transform.position = new Vector3(0, -25, 0);
        }
        
        if (isHurt)
        {
            IframesTimer -= Time.deltaTime;
            if (IframesTimer > 0)
            {
                collider.enabled = false;
            }
            if (IframesTimer <= 0)
            {
                isHurt = false;
                IframesTimer = IFramesInit;
                collider.enabled = true;
                anim.enabled = false;
            }
        }
        
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
            
            if (chances > 0)
            {
                aud.PlayOneShot(clips[2]);
            }
            hurtPlayer();
            Destroy(other.gameObject,0.2f);
        }
    }

    public void hurtPlayer()
    {
        anim.enabled = true;
        if (!smokePart.isPlaying && !sparks.isPlaying)
        {
            smokePart.Play();
            sparks.Play();
        }
        anim.Play("PlayerHurt");
        chances--;
        isHurt = true;
        //TODO: add effects
        if (chances <= 0)
        {
            died = true;
        }
    }
    
    public void resetPlayer()
    {
        smokePart.Stop();
        speed = initialSpeed;
        fireRate = initialFireRate;
        transform.position = initialPosition;
        collider.enabled = true;
        renderer.enabled = true;
        died = false;
        movementsrc.volume = 0.32f;
        isHurt = false;
        IframesTimer = IFramesInit;
        chargeChances();
        sparks.Stop();
        smokePart.Stop();
        anim.enabled = false;
    }


    public void Die()
    {
        muzzlePart.Stop();
        aud.pitch = 0.6f;
        aud.clip = clips[1];
        aud.volume = 0.8f;
        aud.Play();
        var main = particles.main;
        main.useUnscaledTime = true;
        particles.Play();
        collider.enabled = false;
        renderer.enabled = false;
        rb.velocity = Vector2.zero;
        this.enabled = false;
        sparks.Stop();
        smokePart.Stop();
    }


    public void MakePlayerAggressive()
    {
        if (fireRate == 0.3f)
        {
            fireRate -= 0.1f;
            speed += 15;
        }
    }

    public void chargeChances()
    {
        chances = 2;
        sparks.Stop();
        smokePart.Stop();
    }
}
