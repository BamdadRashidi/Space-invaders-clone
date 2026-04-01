using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bunker : Enemy
{
    private bool deathAnimBegin = false;
    [SerializeField] private GameObject shatterParticle;
    private ParticleSystem shatPart;
    private int stages = 0;
    private void Start()
    {
        shatPart = shatterParticle.GetComponentInChildren<ParticleSystem>();
    }

    void Update()
    {
        if (this.HP <= 75 && this.HP > 50 && stages == 0)
        {
            aud.pitch = 1.25f;
            aud.PlayOneShot(clips[0]);
            shatPart.Play();
            renderer.sprite = sprites[1];
            stages = 1;
        }
        else if (this.HP <= 50 && this.HP > 25 && stages == 1)
        {
            aud.pitch = 1.5f;
            aud.PlayOneShot(clips[0]);
            if (!shatPart.isPlaying)
            {
                shatPart.Play();
            }
            renderer.sprite = sprites[2];
            stages = 2;
        }
        else if (this.HP <= 25 && this.HP > 0 && stages == 2)
        {
            aud.pitch = 1.75f;
            aud.PlayOneShot(clips[0]);
            if (!shatPart.isPlaying)
            {
                shatPart.Play();
            }
            renderer.sprite = sprites[3];
            stages = 3;
        }
        else if (this.HP <= 0 && !deathAnimBegin && stages == 3)
        {
            StartCoroutine("PlayDeathAnim");
            stages = 4;
        }
        else
        {
            return;
        }
    }

    public override void shoot()
    {
        return;
    }

    protected override void GiveScore()
    {
        return;
    }
    

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bullet") || other.gameObject.layer == LayerMask.NameToLayer("EnemyBullet"))
        {
            takeDamage(1);
            Destroy(other.gameObject);
        }
    }
    
    IEnumerator PlayDeathAnim()
    {
        aud.PlayOneShot(clips[1]);
        deathAnimBegin = true;
        anim.enabled = true;
        anim.Play("Bunker",0,0f);
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        Destroy(gameObject,1);
    }
}
