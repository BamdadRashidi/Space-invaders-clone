using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomCollider : MonoBehaviour
{
    private LifeManager lifemanager;
    void Start()
    {
        lifemanager = FindObjectOfType<LifeManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            lifemanager.reduceLifeAndContinue();
        }
        else if(other.gameObject.layer == LayerMask.NameToLayer("EnemyBullet"))
        {
            Destroy(other.gameObject);
        }
    }
}
