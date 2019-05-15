using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEnemy : Enemy
{
    [Header("Attack")] 
    protected bool isChargingAttack;
    public float attackChargeDuration; //after dashing, how long until the fireball is shot (should be extremely short or zero)
    public float attackCooldown; //how long the enemy must wait between attacks
    public float fireballSpeed;
    public GameObject fireballPrefab;

    [Header("Dash")] 
    public float dashRange; //how far the dash goes
    public float dashTime; //how long the dash takes
    
    protected override void AggroedBehaviour()
    {
        if (!isChargingAttack)
        {
            Attack();
        }
    }

    //dashes towards the player
    IEnumerator Dash()
    {
        //gets the x direction towards the player, then finds the end location of the dash based on dashRange
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
        
        //face the direction of the end location
        _spriteRenderer.flipX = facingRight;
        
        _animator.Play("dash");
        
        //move towards the end location based on the dashTime
        float timer = 0;
        while (timer < dashTime)
        {
            _rigidbody2D.MovePosition(Vector2.Lerp(transform.position, dashLocation, timer/dashTime));
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        
        var EnemyFire = Resources.Load<AudioClip>("Sounds/EnemyFire");
        AudioManager.instance.PlaySound(EnemyFire);

        //after the dash is finished, start coroutine for fireball attack
        StartCoroutine(ChargeAttack());
    }
    
    
    //shoots a fireball towards the player's current location, does not follow the player
    //not affected by gravity
    IEnumerator ChargeAttack()
    {
        _animator.Play("idle");
        
        //face the enemy towards the player
        Vector3 target = PlayerManager.instance.player.transform.position;
        facingRight = transform.position.x < target.x;
        _spriteRenderer.flipX = facingRight;
        
        yield return new WaitForSeconds(attackChargeDuration);
        
        //instantiate fireball and shoot it towards the player
        GameObject fireball = Instantiate(fireballPrefab, transform.position, Quaternion.identity);
        Rigidbody2D fireballRigidbody = fireball.GetComponent<Rigidbody2D>();
        
        Vector2 directionToPlayer = (target - transform.position).normalized;
        fireballRigidbody.velocity = directionToPlayer * fireballSpeed;
        
        //rotate the fireball so it is facing the direction that it is travelling
        fireball.transform.right = directionToPlayer * -1;

        //wait for delay between attacks (attackCooldown)
        yield return new WaitForSeconds(attackCooldown);
        isChargingAttack = false;
    }

    
    protected override void Attack()
    {
        isChargingAttack = true;
        StartCoroutine(Dash());
    }
}
