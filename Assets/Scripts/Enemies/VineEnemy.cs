using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineEnemy : Enemy
{
    [Header("Attack")] 
    protected bool isChargingAttack;
    public float attackRange;
    public float attackChargeDuration;
    public float attackCooldown;
    public GameObject vineHitBox;
    
    protected override void AggroedBehaviour()
    {
        //if isAggroed, check if the player is within range
        //if in range, start attacking
        //if not, move towards the player
        if (!isChargingAttack)
        {
            Vector2 target = PlayerManager.instance.player.transform.position;
            if (Vector3.Distance(transform.position, target) < attackRange)
            {
                Attack();
            }
            else
            {
                if (CanMoveForward())
                {
                    _animator.Play("run");

                    facingRight = transform.position.x < target.x;
                    _spriteRenderer.flipX = facingRight;
                    if (facingRight)
                    {
                        _rigidbody2D.MovePosition(transform.position +
                                                  (new Vector3(chaseSpeed, 0, 0) * Time.deltaTime));
                    }
                    else
                    {
                        _rigidbody2D.MovePosition(transform.position -
                                                  (new Vector3(chaseSpeed, 0, 0) * Time.deltaTime));
                    }
                }
                else
                {
                    _animator.Play("idle");
                }
            }
        }
    }

    IEnumerator ChargeAttack()
    {
        isChargingAttack = true;
        
        //face towards the player when the attack starts
        Vector2 target = PlayerManager.instance.player.transform.position;
        facingRight = transform.position.x < target.x;
        _spriteRenderer.flipX = facingRight;
        
        //start charging attack, wait for duration
        _animator.Play("chargeAttack");
        yield return new WaitForSeconds(attackChargeDuration);
        
        _animator.Play("attack");
        //spawn hitBox for attack based on the face direction that was determined above     
        if (facingRight)
        {
            var EnemyVine = Resources.Load<AudioClip>("Sounds/EnemyVine");
            AudioManager.instance.PlaySound(EnemyVine);
            
            GameObject hitBox = Instantiate(vineHitBox, transform.position + new Vector3(1f, 0.5f, 0), Quaternion.identity);

        }
        else
        {
            var EnemyVine = Resources.Load<AudioClip>("Sounds/EnemyVine");
            AudioManager.instance.PlaySound(EnemyVine);
            
            GameObject hitBox = Instantiate(vineHitBox, transform.position + new Vector3(-1f, 0.5f, 0), Quaternion.identity);
        }
        
        //wait for attackCooldown duration, so no new attack starts
        yield return new WaitForSeconds(attackCooldown);
        
        isChargingAttack = false;
    }

    protected override void Attack()
    {

        StartCoroutine(ChargeAttack());
    }
}
