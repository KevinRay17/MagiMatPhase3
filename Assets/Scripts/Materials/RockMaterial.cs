using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockMaterial : MaterialClass
{
    [Header("Attack")] 
    public float attackDamage;
    public float attackMoveSpeed;
    public Vector2 attackOffset; //distance between the center of the hurt box and the player
    public bool attackHitMultipleTargets;
    public Vector2 attackSize; //size of hurt box assuming the attack is directed left or right
    public GameObject hurtBoxPrefab;

    [Header("Special")] 
    public float specialRadius;
    public int numberOfProjectiles;
    public float projectileSpeed;
    public GameObject projectilePrefab;

    public override void Attack(GameObject player)
    {
        Debug.Log("Rock Attack");
        int attackDirection = PlayerManager.instance.playerMovement.faceDirection;
        if (PlayerManager.instance.playerMovement.isGrounded)
        {
            if (attackDirection == 2 || attackDirection == 4)
            {
                Vector2 attackDirectionV2 = GlobalFunctions.FaceDirectionToVector2(attackDirection);

                Vector3 directionalOffset = Vector2.zero;
                directionalOffset.x = attackOffset.x * attackDirectionV2.x;
                directionalOffset.y = attackOffset.y;

                GameObject hurtBox = Instantiate(hurtBoxPrefab, player.transform.position + directionalOffset,
                    Quaternion.identity);
                RockAttackHurtBox hbScript = hurtBox.GetComponent<RockAttackHurtBox>();

                hbScript._spriteRenderer.size = attackSize;
                hbScript._boxCollider.size = attackSize;
                hbScript.damage = attackDamage;
                hbScript.hitMultipleTargets = attackHitMultipleTargets;

                hbScript.speed = attackMoveSpeed;
                hbScript.direction = attackDirectionV2;
            }
        }
    }

    public override void Special(GameObject player)
    {
        Debug.Log("Rock Special");
        float radiansBetweenOrbiters = (360 * Mathf.Deg2Rad) / numberOfProjectiles;

        for (int i = 0; i < numberOfProjectiles; i++)
        {
            float radians = i * radiansBetweenOrbiters;
            Vector2 playerPos = player.transform.position;
            Vector2 spawnOffset = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians)) * specialRadius;
            Vector2 spawnPosition = playerPos + spawnOffset;

            GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity, this.transform);
            Rigidbody2D projRB = projectile.GetComponent<Rigidbody2D>();
            projRB.velocity = spawnOffset.normalized * projectileSpeed;
        }
    }
}
