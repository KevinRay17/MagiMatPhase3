using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEnemy : Enemy
{
    [Header("Attack")] 
    protected bool isChargingAttack;
    public float attackChargeDuration;
    public float attackCooldown;
    public float fireballSpeed;
    public GameObject fireballPrefab;

    [Header("Dash")] 
    public float dashRange;
    public float dashTime;
    
    protected override void AggroedBehaviour()
    {
        if (!isChargingAttack)
        {
            isChargingAttack = true;
            StartCoroutine(Dash());
        }
    }

    IEnumerator ChargeAttack()
    {
        Vector3 target = PlayerManager.instance.player.transform.position;
        yield return new WaitForSeconds(attackChargeDuration);
        facingRight = transform.position.x < target.x;
        _spriteRenderer.flipX = facingRight;
        
        GameObject fireball = Instantiate(fireballPrefab, transform.position, Quaternion.identity);
        Rigidbody2D fireballRigidbody = fireball.GetComponent<Rigidbody2D>();
        
        Vector2 directionToPlayer = (target - transform.position).normalized;
        fireballRigidbody.velocity = directionToPlayer * fireballSpeed;
        fireball.transform.right = directionToPlayer * -1;

        yield return new WaitForSeconds(attackCooldown);
        isChargingAttack = false;
    }

    IEnumerator Dash()
    {
        Vector2 playerPos = PlayerManager.instance.player.transform.position;
        Vector2 dashLocation = transform.position;
        if (playerPos.x > transform.position.x)
        {
            dashLocation = transform.position + new Vector3(dashRange, 0, 0);
            facingRight = true;
        }
        else
        {
            dashLocation = transform.position - new Vector3(dashRange, 0, 0);
            facingRight = false;
        }
        _spriteRenderer.flipX = facingRight;
        float timer = 0;
        while (timer < dashTime)
        {
            _rigidbody2D.MovePosition(Vector2.Lerp(transform.position, dashLocation, timer/dashTime));
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        StartCoroutine(ChargeAttack());
    }
    
    protected override void Attack()
    {
        GameObject boulder = Instantiate(fireballPrefab, transform.position, Quaternion.identity);
        Vector2 directionToPlayer = (PlayerManager.instance.player.transform.position - transform.position).normalized;
        boulder.GetComponent<Rigidbody2D>().velocity = directionToPlayer * fireballSpeed;
    }
}
