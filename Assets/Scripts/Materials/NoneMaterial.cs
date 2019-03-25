using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoneMaterial : MaterialClass
{   
    [Header("Attack")] 
    public bool attackUseFaceDirection;
    
    public float attackDamage;
    public float attackOffset; //distance between the center of the hurt box and the player
    public bool attackHitMultipleTargets;
    public Vector2 attackSize; //size of hurt box assuming the attack is directed left or right
    public GameObject hurtBoxPrefab;
    
    public override void Attack(GameObject player)
    {
        Debug.Log("Basic Attack");

        int attackDirection; //direction that the attack is directed, same as faceDirection where 1 = UP, 2 = RIGHT, 3 = DOWN, 4 = LEFT
        Vector2 hurtBoxSize = attackSize;
        
        if (attackUseFaceDirection)
        {
            //if true, use the player's faceDirection (basically inputs) to determine attack direction
            attackDirection = PlayerManager.instance.playerMovement.faceDirection;
        }
        else
        {
            //convert mouse direction to an int for face direction
            //get mouse direction, convert it to an angle in degrees, then divide it by 90 and round to int
            Vector2 direction = PlayerManager.instance.playerActions.mouseDirection;
            float directionAngle = GlobalFunctions.Vector2DirectionToAngle(direction);
            attackDirection = Mathf.RoundToInt(directionAngle / 90);
            attackDirection += 1;
            //since the boundaries for the directions are not in the cardinal directions, but instead on the diagonals
            //the last 45 degrees need to be associated with the upward direction
            attackDirection = attackDirection == 5 ? 1 : attackDirection;
        }

        //since the hitBoxSize is for horizontal attacks, we swap the width and height for upward or downward attacks
        if (attackDirection == 1 || attackDirection == 3)
        {
            hurtBoxSize = new Vector2(hurtBoxSize.y, hurtBoxSize.x);
        }

        //instantiate the hurt box, give it its appropriate size and position, change its script's values accordingly
        GameObject hurtBox = Instantiate(hurtBoxPrefab, player.transform);
        HurtBox hbScript = hurtBox.GetComponent<HurtBox>();

        hbScript._spriteRenderer.size = hurtBoxSize;
        hbScript._boxCollider.size = hurtBoxSize;
        hurtBox.transform.localPosition = GlobalFunctions.FaceDirectionToVector2(attackDirection) * attackOffset;

        hbScript.damage = attackDamage;
        hbScript.hitMultipleTargets = attackHitMultipleTargets;
    }

    public override void Special(GameObject player)
    {
        Debug.Log("No Basic Special");
    }
}

