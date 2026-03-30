using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyBullets : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private BoxCollider2D collider;
    [SerializeField] private float DestroyTimer;
    [SerializeField] private int bulletId;
    private Vector2 direction;
    
    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        GameObject.Destroy(gameObject,DestroyTimer);
    }
    public void setDirection(Vector2 dir)
    {
        this.direction = dir.normalized;
    }
    private void Update()
    {
        if (bulletId != 2)
        {
            transform.position += Vector3.down * Time.deltaTime * speed;
        }
        else
        {
            transform.position += (Vector3)(direction * speed * Time.deltaTime);
        }
    }
    
}