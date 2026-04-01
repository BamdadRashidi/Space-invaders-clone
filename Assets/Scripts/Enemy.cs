using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

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
    [SerializeField] protected AudioClip[] clips;
    protected AudioSource aud;
    public void Awake()
    {
        aud = GetComponent<AudioSource>();
        firstOfRangeinit = firstOfRange;
        lastOfRangeinit = lastOfRange;
        anim = GetComponent<Animator>();
        anim.enabled = false;
        collider = GetComponent<BoxCollider2D>();
        renderer = GetComponent<SpriteRenderer>();
        particle = GetComponent<ParticleSystem>();
        wave = gameObject.GetComponentInParent<Wave>();
        if (!(this is UFO))
        {
            renderer.sprite = sprites[0];
        }
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
            anim.enabled = true;

            if (this is ThinEnem)
                anim.Play("ThinEnemy",0,0f);
            else if (this is ThickEnemy)
                anim.Play("ThickEnemy",0,0f);
            else if (this is FollowEnemy)
                anim.Play("FollowEnemy",0,0f);
            else if (this is UFO)
                anim.Play("UFO_Hurt",0,0f);
            
            takeDamage(1);
            Destroy(other.gameObject);
        }
    }

    protected abstract void GiveScore();

    protected void Die()
    {
        aud.volume = 0.6f;
        aud.pitch = Random.Range(0.95f, 1.05f);
        aud.clip = clips[1];
        aud.Play();
        collider.enabled = false;
        particle.Play();
        if (!(this is UFO) && !(this is Bunker))
        {
            wave.reduceAlien();
        }

        if (!(this is Bunker))
        {
            renderer.enabled = false;
            Destroy(gameObject,0.3f);
        }
        if (this is UFO)
        {
            LifeManager.Instance.lives += 5;
            UIManager.instance.UpdateLives(LifeManager.Instance.lives);
        }
    }

    public void flipSprite()
    {
        renderer.sprite = (renderer.sprite == sprites[0]) ? sprites[1] : sprites[0];
    }
    
}
