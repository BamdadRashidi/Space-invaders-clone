using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected int HP;
    [SerializeField] public float firstOfRange;
    [SerializeField] public float lastOfRange;
    public float firstOfRangeinit;
    public float lastOfRangeinit;
    protected Animator anim;
    protected float timer;
    private int score;
    protected Wave wave;
    protected SpriteRenderer renderer;
    protected ParticleSystem particle;
    protected BoxCollider2D collider;
    protected int[] scores = {100, 200, 300, 400, 500, 600, 700, 800, 900, 1000, 1500, 2000};
    [SerializeField] protected Sprite[] sprites;
    
    public void Awake()
    {
        firstOfRangeinit = firstOfRange;
        lastOfRangeinit = lastOfRange;
        anim = GetComponent<Animator>();
        collider = GetComponent<BoxCollider2D>();
        renderer = GetComponent<SpriteRenderer>();
        particle = GetComponent<ParticleSystem>();
        wave = gameObject.GetComponentInParent<Wave>();
        if (!(this is UFO))
        {
            renderer.sprite = sprites[0];
        }
        anim.enabled = false;
    }

    public abstract void shoot();

    public void takeDamage(int damage)
    {
        this.HP = Mathf.Max(this.HP - damage, 0);
        if (this.HP <= 0)
        {
            GiveScore();
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            takeDamage(1);
            Destroy(other.gameObject);
        }
    }

    protected abstract void GiveScore();

    protected void Die()
    {
        //TODO: add audio
        collider.enabled = false;
        particle.Play();
        if (!(this is UFO) && !(this is Bunker))
        {
            wave.reduceAlien();
        }

        if (!(this is Bunker))
        {
            renderer.enabled = false;
            Destroy(gameObject,1);
        }
    }

    public void flipSprite()
    {
        renderer.sprite = (renderer.sprite == sprites[0]) ? sprites[1] : sprites[0];
    }
    
}
