using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

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
    void Start()
    {
        initialPosition = new Vector3(0,-25,0);
        died = false;
        timerCounter = fireRate;
        collider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
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
        Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Bullet") &&
            other.gameObject.layer != LayerMask.NameToLayer("Wall"))
        {
            KillOrResurrect();
            Destroy(other.gameObject);
        }
    }

    public void KillOrResurrect()
    {
        died = !died;
    }

    public void resetPlayer()
    {
        transform.position = initialPosition;
    }
}
