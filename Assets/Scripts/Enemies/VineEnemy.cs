using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineEnemy : Enemy
{
    [Header("Attack")] 
    protected bool isChargingAttack;
    public float attackRange;
    public float attackChargeDuration;
    public GameObject vineHitBox;

    public Sprite movingSprite;
    public Sprite waitingSprite;
    public Sprite chargingSprite;
    public Sprite attackingSprite;
    
    protected override void AggroedBehaviour()
    {
        if (!isChargingAttack)
        {
            Vector2 target = PlayerManager.instance.player.transform.position;
            if (Vector3.Distance(transform.position, target) < attackRange)
            {
                StartCoroutine(ChargeAttack());
            }
            else
            {
                _spriteRenderer.sprite = movingSprite;
                facingRight = transform.position.x < target.x;
                _spriteRenderer.flipX = facingRight;
                if (facingRight)
                {
                    _rigidbody2D.MovePosition(transform.position + (new Vector3(chaseSpeed, 0, 0) * Time.deltaTime));
                }
                else
                {
                    _rigidbody2D.MovePosition(transform.position - (new Vector3(chaseSpeed, 0, 0) * Time.deltaTime));
                }
            }
        }
    }

    protected override void Patrol()
    {
        //check if the enemy is currently waiting at the end of its patrol bounds
        //if it is, reduce the timer til it reaches 0
        //at 0, flip the sprite's direction
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
        
        //if the enemy is not waiting, move based on its current facing
        //also check if the enemy reaches its patrol bounds
        //if it does, set patrolWaitTimer
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

        _spriteRenderer.sprite = movingSprite;
    }

    IEnumerator ChargeAttack()
    {
        isChargingAttack = true;
        
        Vector2 target = PlayerManager.instance.player.transform.position;
        facingRight = transform.position.x < target.x;
        _spriteRenderer.flipX = facingRight;
        _spriteRenderer.sprite = chargingSprite;
        yield return new WaitForSeconds(attackChargeDuration);
        _spriteRenderer.sprite = attackingSprite;
        Attack();
        yield return new WaitForSeconds(0.3f);
        isChargingAttack = false;
    }

    protected override void Attack()
    {

        if (facingRight)
        {
            GameObject hitBox = Instantiate(vineHitBox, transform.position + new Vector3(1.5f, 0, 0), Quaternion.identity);
        }
        else
        {
            GameObject hitBox = Instantiate(vineHitBox, transform.position - new Vector3(1.5f, 0, 0), Quaternion.identity);
        }
    }
}
