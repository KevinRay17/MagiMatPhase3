using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireMaterial : MaterialClass
{
    [Header("Attack")]
    public bool attackUseFaceDirection;
    
    public float attackDamage;
    public float attackOffset;
    public bool attackHitMultipleTargets;
    public Vector2 attackSize;
    public GameObject hurtBoxPrefab;
    
    [Header("Special")]
    public float specialDashDistance;
    public float specialDashTime; //how long the dash takes
    
    public override void Attack(GameObject player)
    {
        Debug.Log("Fire Attack");

        //See NoneMaterial Attack() for comments
        
        int attackDirection;
        Vector2 hitBoxSize = attackSize;
        
        if (attackUseFaceDirection)
        {
            attackDirection = PlayerManager.instance.playerMovement.faceDirection;
        }
        else
        {
            Vector2 direction = PlayerManager.instance.playerActions.mouseDirection;
            float directionAngle = GlobalFunctions.Vector2DirectionToAngle(direction);
            attackDirection = Mathf.RoundToInt(directionAngle / 90);
            attackDirection += 1;
            attackDirection = attackDirection == 5 ? 1 : attackDirection;
        }

        if (attackDirection == 1 || attackDirection == 3)
        {
            hitBoxSize = new Vector2(hitBoxSize.y, hitBoxSize.x);
        }

        GameObject hurtBox = Instantiate(hurtBoxPrefab, player.transform);
        HurtBox hbScript = hurtBox.GetComponent<HurtBox>();

        hbScript._spriteRenderer.size = hitBoxSize;
        hbScript._boxCollider.size = hitBoxSize;
        hurtBox.transform.localPosition = GlobalFunctions.FaceDirectionToVector2(attackDirection) * attackOffset;

        hbScript.damage = attackDamage;
        hbScript.hitMultipleTargets = attackHitMultipleTargets;
    }
    
    public override void Special(GameObject player)
    {
        Debug.Log("Fire Special");

        Vector2 direction = PlayerManager.instance.playerActions.mouseDirection;

        StartCoroutine(FireSpecial(player, direction));
    }

    IEnumerator FireSpecial(GameObject player, Vector3 direction)
    {
        Rigidbody2D playerRB = player.GetComponent<Rigidbody2D>();
        //make the player ignore enemy collisions
        Physics2D.IgnoreLayerCollision(player.layer, LayerMask.NameToLayer("Enemies"), true);
        
        //remove existing velocity so there isn't a strange jump/drop at the end of the dash
        playerRB.velocity = Vector2.zero;

        //temporarily remove gravity, so there isn't jitter when flying in the air
        playerRB.gravityScale = 0;

        //disable player movement
        PlayerManager.instance.playerMovement.canMove = false;

        //using MovePosition so the momentum of the dash isn't carried over
        //split over a few increments to make the dash seem smoother and not so sudden
        int upper = Mathf.RoundToInt(specialDashTime / 0.01f);
        for (int i = 0; i < upper; i++)
        {
            playerRB.MovePosition(player.transform.position + (direction * specialDashDistance/upper));
            yield return new WaitForSeconds(0.01f);
        }
        
        //add slight impulse force at the end for a little bit of momentum
        playerRB.AddForce(direction * specialDashDistance, ForceMode2D.Impulse);

        //reset gravity, collisions, and movement
        playerRB.gravityScale = PlayerManager.instance.playerMovement.gravityScale;
        Physics2D.IgnoreLayerCollision(player.layer, LayerMask.NameToLayer("Enemies"), false);
        PlayerManager.instance.playerMovement.canMove = true;

    }
}
