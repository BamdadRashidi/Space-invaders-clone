using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private BoxCollider2D collider;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float DestroyTimer;
    
    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        GameObject.Destroy(gameObject,DestroyTimer);
    }
    private void Update()
    {
        transform.position += Vector3.up * Time.deltaTime * speed;
    }
    
}
