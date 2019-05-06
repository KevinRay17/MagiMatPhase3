using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockEnemy : Enemy
{
    [Header("Attack")] 
    protected bool isChargingAttack;
    public float attackRange;
    public float attackChargeDuration;
    public float boulderSpeed;
    public Vector2 boulderSpawnOffset;
    public GameObject boulderPrefab;
    
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

    IEnumerator ChargeAttack()
    {   
        isChargingAttack = true;
        GameObject boulder = Instantiate(boulderPrefab, transform.position + (Vector3)boulderSpawnOffset, Quaternion.identity);
        Collider2D boulderCollider = boulder.GetComponent<Collider2D>();
        Rigidbody2D boulderRigidbody = boulder.GetComponent<Rigidbody2D>();
        
        Physics2D.IgnoreCollision(boulderCollider, this.GetComponent<Collider2D>(), true);
        boulderRigidbody.isKinematic = true;

        float chargeTimer = 0;
        while (chargeTimer < attackChargeDuration)
        {
            if (boulder != null)
            {
                boulder.transform.localScale = Vector3.Lerp(Vector3.zero, new Vector3(0.5f, 0.5f, 1),
                    chargeTimer / attackChargeDuration);
            }
            Vector3 target = PlayerManager.instance.player.transform.position;
            facingRight = transform.position.x < target.x;
            _spriteRenderer.flipX = facingRight;
            chargeTimer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        if (boulder != null)
        {
            Vector3 target = PlayerManager.instance.player.transform.position;
            Vector2 directionToPlayer = (target - boulder.transform.position).normalized;
            boulderRigidbody.isKinematic = false;
            boulderRigidbody.velocity = directionToPlayer * boulderSpeed;
        }

        isChargingAttack = false;
    }
    
    protected override void Attack()
    {
        GameObject boulder = Instantiate(boulderPrefab, transform.position + (Vector3)boulderSpawnOffset, Quaternion.identity);
        Vector2 directionToPlayer = (PlayerManager.instance.player.transform.position - transform.position).normalized;
        boulder.GetComponent<Rigidbody2D>().velocity = directionToPlayer * boulderSpeed;
    }
}
