using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected Rigidbody2D _rigidbody2D;
    protected SpriteRenderer _spriteRenderer;

    public Material material;
    public int health;
    public EnemySpriteMask flickerMask;
    public float distanceToGround;
    public float groundCheckAheadDistance;

    [Header("Movement")] 
    public bool stationary;
    protected float startX; //set in Start() to get patrol bounds
    protected bool facingRight; //face direction
    public float patrolSpeed;
    public float patrolRange; //how far from startX this enemy will patrol
    protected float patrolWaitTimer; //timer for patrolWaitDuration
    public float patrolWaitDuration; //how long the enemy waits when reaching either end of its patrol bounds
    public float chaseSpeed;
    
    [Header("Detection")] 
    public bool isAggroed;
    public bool detectionRequiresLOS; //does the enemy need to see the player to get aggro
    public float detectionRange; //range of enemy vision
    public float dropAggroRange; //how far away the player must be to drop aggro
 
    
    public ParticleSystem deathAnim;
    

    protected virtual void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void Start()
    {
        //get startingX position for patrol bounds
        startX = transform.position.x;
    }
    
    protected virtual void Update()
    {
        //check if the enemy is still aggroed
        isAggroed = DetectPlayer();
        
        //if aggroed, do enemy-specific behaviour
        if (isAggroed)
        {
            AggroedBehaviour();
        }
        
        //if not aggroed and not stationary, patrol
        if (!isAggroed && !stationary)
        {
            Patrol();
        }
    }
    
    //handles enemy aggro
    protected virtual bool DetectPlayer()
    {
        float distanceToPlayer = Vector3.Distance(PlayerManager.instance.player.transform.position,transform.position);

        //if not aggroed
        if (!isAggroed)
        {
            //check if the player is within the detectionRange
            if (distanceToPlayer < detectionRange)
            {
                //if requiresLOS, use a raycast to check if the enemy can see the player
                if (detectionRequiresLOS)
                {
                    Vector2 directionToPlayer = PlayerManager.instance.player.transform.position - transform.position;
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer);
                    return hit.transform == PlayerManager.instance.player.transform;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }
        //if aggroed
        else
        {
            //check if the player is within the dropAggroRange
            //if not, drop aggro
            if (distanceToPlayer > dropAggroRange)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    protected virtual void Patrol()
    {
        //check if the enemy is currently waiting at the end of its patrol bounds
        //if it is, reduce the timer til it reaches 0
        //at 0, flip the sprite's direction
        Debug.Log(patrolWaitTimer);
        if (patrolWaitTimer > 0)
        {
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
            Debug.Log("cant move forward");
            patrolWaitTimer = patrolWaitDuration;
        }
    }

    protected virtual bool CanMoveForward()
    {
        Vector2 aheadDistance;
        if (facingRight)
        {
            aheadDistance = Vector2.right * groundCheckAheadDistance;
        }
        else
        {
            aheadDistance = Vector2.left * groundCheckAheadDistance;
        }
        Debug.DrawLine(transform.position, (Vector2) transform.position + (Vector2.down * distanceToGround) + aheadDistance);
        return Physics2D.OverlapPoint((Vector2) transform.position + (Vector2.down * distanceToGround) + aheadDistance);
    }
    
    //called in update when aggroed
    protected abstract void AggroedBehaviour();
    
    //the special attack of this enemy, should be called in AggroedBehaviour
    protected abstract void Attack();

    //call this method to damage enemy
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (flickerMask != null)
        {
            flickerMask.Flicker();
        }
        if (health <= 0)
        {
            if (material == Material.Rock)
            {
                Instantiate(deathAnim, transform.position, Quaternion.identity);
            }
            Death();
        }
    }

    //called in TakeDamage when enemy reaches 0 health
    protected virtual void Death()
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("HurtBox"))
        {
            TakeDamage(1);
        }
    }
}