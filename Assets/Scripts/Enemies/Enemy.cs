using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected Rigidbody2D _rigidbody2D;
    protected SpriteRenderer _spriteRenderer;

    public Material material;

    [Header("Detection")] 
    public bool isAggroed;
    public bool detectionRequiresLOS; //does the enemy need to see the player to get aggro
    public float detectionRange; //range of enemy vision
    public float dropAggroRange; //how far away the player must be to drop aggro
    

    protected virtual void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void Update()
    {
        //check if the enemy is still aggroed
        isAggroed = DetectPlayer();
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

    protected abstract void Behaviour();
    
    protected abstract void Attack();

}