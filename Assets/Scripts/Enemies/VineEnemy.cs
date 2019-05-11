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

    [Header("Sprites")]
    public Sprite movingSprite;
    public Sprite waitingSprite;
    public Sprite chargingSprite;
    public Sprite attackingSprite;
    
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
                    _spriteRenderer.sprite = movingSprite;
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
            }
        }
    }

    //same as normal patrol function but we change the sprite based on state
    protected override void Patrol()
    {
        if (patrolWaitTimer > 0)
        {
            _spriteRenderer.sprite = waitingSprite;
            patrolWaitTimer -= Time.deltaTime;
            if (patrolWaitTimer <= 0)
            {
                facingRight = !facingRight;
                _spriteRenderer.flipX = !_spriteRenderer.flipX;
            }
            return;
        }

        if (CanMoveForward())
        {
            if (facingRight)
            {
                _rigidbody2D.MovePosition(transform.position + (new Vector3(patrolSpeed, 0, 0) * Time.deltaTime));
                if (transform.position.x > startX + patrolRange)
                {
                    patrolWaitTimer = patrolWaitDuration;
                }
            }
            else
            {
                _rigidbody2D.MovePosition(transform.position - (new Vector3(patrolSpeed, 0, 0) * Time.deltaTime));
                if (transform.position.x < startX - patrolRange)
                {
                    patrolWaitTimer = patrolWaitDuration;
                }
            }
        }
        else
        {
            patrolWaitTimer = patrolWaitDuration;
        }

        _spriteRenderer.sprite = movingSprite;
    }

    IEnumerator ChargeAttack()
    {
        isChargingAttack = true;
        
        //face towards the player when the attack starts
        Vector2 target = PlayerManager.instance.player.transform.position;
        facingRight = transform.position.x < target.x;
        _spriteRenderer.flipX = facingRight;
        
        //start charging attack, wait for duration
        _spriteRenderer.sprite = chargingSprite;
        yield return new WaitForSeconds(attackChargeDuration);
        
        //spawn hitBox for attack based on the face direction that was determined above
        _spriteRenderer.sprite = attackingSprite;
        
        if (facingRight)
        {
            GameObject hitBox = Instantiate(vineHitBox, transform.position + new Vector3(1.5f, 0, 0), Quaternion.identity);
        }
        else
        {
            GameObject hitBox = Instantiate(vineHitBox, transform.position - new Vector3(1.5f, 0, 0), Quaternion.identity);
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
