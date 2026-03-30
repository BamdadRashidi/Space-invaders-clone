using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bunker : Enemy
{
    private bool deathAnimBegin = false;
    
    void Update()
    {
        if (this.HP <= 75 && this.HP > 50)
        {
            renderer.sprite = sprites[1];
        }
        else if (this.HP <= 50 && this.HP > 25)
        {
            renderer.sprite = sprites[2];
        }
        else if (this.HP <= 25 && this.HP > 0)
        {
            renderer.sprite = sprites[3];
        }
        else if (this.HP <= 0 && !deathAnimBegin)
        {
            StartCoroutine("PlayDeathAnim");
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
        deathAnimBegin = true;
        anim.enabled = true;
        anim.Play("Bunker",0,0f);
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        Destroy(gameObject,1);
    }
}
