using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockEnemy : Enemy
{
    [Header("Attack")] 
    protected bool isChargingAttack; //used to determine if a new attack can start
    public float attackRange; //how close the player has to be for enemy to start charging attack
    public float attackChargeDuration;
    public float boulderSpeed;
    public Vector2 boulderSpawnOffset; //offset from enemy that the boulder should be spawned
    public GameObject boulderPrefab;
    
    //called in update when isAggroed
    protected override void AggroedBehaviour()
    {
        //if not already charging an attack, check distance between enemy and player
        if (!isChargingAttack)
        {
            Vector2 target = PlayerManager.instance.player.transform.position;
            if (Vector3.Distance(transform.position, target) < attackRange)
            {
                //if the player is within the attackRange, start charging an attack
                Attack();
            }
            else
            {
                if (CanMoveForward())
                {
                    //if the player is outside the attackRange, but is still aggroed, move towards the player
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

    //rock attack is a boulder toss
    //the boulder is first spawned, then slowly grows to its maximum size
    //when the boulder is fully charged (max size), it is then thrown at the player
    //the boulder is affected by gravity
    IEnumerator ChargeAttack()
    {   
        isChargingAttack = true;
        //instantiate boulder
        GameObject boulder = Instantiate(boulderPrefab, transform.position + (Vector3)boulderSpawnOffset, Quaternion.identity);
        Collider2D boulderCollider = boulder.GetComponent<Collider2D>();
        Rigidbody2D boulderRigidbody = boulder.GetComponent<Rigidbody2D>();
        
        //remove collisions between boulder and enemy since neither are triggers
        Physics2D.IgnoreCollision(boulderCollider, this.GetComponent<Collider2D>(), true);
        
        //make boulder kinematic, so it isn't affected by gravity until it is thrown
        boulderRigidbody.isKinematic = true;

        //create timer for the attackChargeDuration
        float chargeTimer = 0;
        while (chargeTimer < attackChargeDuration)
        {
            if (boulder != null)
            {
                //set scale of the boulder based on how far into the attackChargeDuration we are
                boulder.transform.localScale = Vector3.Lerp(Vector3.zero, new Vector3(0.5f, 0.5f, 1),
                    chargeTimer / attackChargeDuration);
            }
            
            //always face the enemy towards the player when charging attack
            Vector3 target = PlayerManager.instance.player.transform.position;
            facingRight = transform.position.x < target.x;
            _spriteRenderer.flipX = facingRight;
            
            chargeTimer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        
        //charging finished
        if (boulder != null)
        {
            //throw boulder towards player
            Vector3 target = PlayerManager.instance.player.transform.position;
            Vector2 directionToPlayer = (target - boulder.transform.position).normalized;
            boulderRigidbody.isKinematic = false;
            boulderRigidbody.velocity = directionToPlayer * boulderSpeed;
        }

        isChargingAttack = false;
    }
    
    protected override void Attack()
    {
        StartCoroutine((ChargeAttack()));
    }
}
