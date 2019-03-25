using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineMaterial : MaterialClass
{
    [Header("Attack")]
    public bool attackUseFaceDirection;
    
    public float attackDamage;
    public float attackOffset;
    public bool attackHitMultipleTargets;
    public Vector2 attackSize;
    public GameObject hurtBoxPrefab;

    [Header("Special")] 
    [HideInInspector] public bool onGrapple; //used for exitting the grapple, referenced by VineGrappleCollision script that is put on player
    public float grappleShootSpeed; //speed that grapple line is shot out
    public float grappleSpeed; //speed that player is pulled by the grapple
    public float maxGrappleDistance;
    public float maxGrappleTime; //max time that the player can be on the grapple until it automatically breaks
    public LayerMask grappleLayer; //layers that the grapple can hit
    public GameObject grapplePrefab;
    
    public override void Attack(GameObject player)
    {
        Debug.Log("Vine Attack");
        
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
        Debug.Log("Vine Special");

        Vector2 direction = PlayerManager.instance.playerActions.mouseDirection;
        RaycastHit2D hit = Physics2D.Raycast(player.transform.position, direction, maxGrappleDistance, grappleLayer);

        Vector2 grapplePosition; //the end of the grapple
        if (hit.collider != null)
        {
            //grapple hit
            grapplePosition = hit.point;
            StartCoroutine(ShootGrapple(player, grapplePosition, true));
        }
        else
        {
            //grapple missed
            Vector2 playerPos = player.transform.position;
            grapplePosition = playerPos + (direction * maxGrappleDistance);
            StartCoroutine(ShootGrapple(player, grapplePosition, false));
        }
    }

    IEnumerator ShootGrapple(GameObject player, Vector2 grapplePosition, bool hit)
    {
        //visualisation of grapple shooting from player
        
        GameObject grapple = Instantiate(grapplePrefab, player.transform);
        LineRenderer grappleLR = grapple.GetComponent<LineRenderer>();
        VineGrappleLine grappleScript = grapple.GetComponent<VineGrappleLine>();

        grappleScript.lineSource = player;

        bool shootingGrapple = true;
        float timer = 0;
        
        Vector2 grappleEndPosition = player.transform.position;
        
        while (shootingGrapple)
        {
            //set start of line to player position
            grappleLR.SetPosition(0, player.transform.position);
            
            //move end point towards the grapple position
            grappleEndPosition = Vector2.MoveTowards(grappleEndPosition, grapplePosition, grappleShootSpeed * Time.deltaTime);
            grappleLR.SetPosition(1, grappleEndPosition);

            //if the end position reaches the final grapple position, flip bool to exit loop
            if (grappleEndPosition == grapplePosition)
            {
                shootingGrapple = false;
            }
            
            //timer to avoid getting stuck in loop
            timer += Time.deltaTime;
            if (timer > maxGrappleTime)
            {
                shootingGrapple = false;
            }       
            
            yield return new WaitForEndOfFrame();
        }

        //hit stores if the grapple actually hit something we can grapple to
        if (hit)
        {
            //if hit, enable the grapple script to constantly update its line start position to the player position
            grappleScript.enabled = true;
            //start coroutine for pulling the player to the grapple position
            StartCoroutine(GrappleToPoint(player, grapplePosition, grapple));
        }
        else
        {
            //if missed, destroy the grapple visual
            Destroy(grapple);
        }
    }
    
    IEnumerator GrappleToPoint(GameObject player, Vector2 grapplePosition, GameObject grapple)
    {
        //after line is made between player and the target point, pull the player towards the grapple position
        
        Rigidbody2D playerRB = player.GetComponent<Rigidbody2D>();
        
        //remove existing velocity so there isn't a strange jump/drop at the end of the dash
        playerRB.velocity = Vector2.zero;

        //temporarily remove gravity, so there isn't jitter when flying in the air
        playerRB.gravityScale = 0;

        //disable player movement
        PlayerManager.instance.playerMovement.canMove = false;
        
        onGrapple = true;
        float timer = 0;
        
        //add component to player to check for collisions for exitting the grapple
        VineGrappleCollision playerVineScript = player.AddComponent<VineGrappleCollision>();

        while (onGrapple)
        {
            //move player towards the grapple point
            playerRB.MovePosition(Vector2.MoveTowards(player.transform.position, grapplePosition, grappleSpeed * Time.deltaTime));

            //if timer reaches the maxGrappleTime, stop grappling just to avoid getting stuck
            timer += Time.deltaTime;
            if (timer > maxGrappleTime)
            {
                onGrapple = false;
            }        
            yield return new WaitForEndOfFrame();
        }
        
        //reset gravity, collisions, and movement
        playerRB.gravityScale = PlayerManager.instance.playerMovement.gravityScale;
        Physics2D.IgnoreLayerCollision(player.layer, LayerMask.NameToLayer("Enemies"), false);
        PlayerManager.instance.playerMovement.canMove = true;
        
        //remove the added component from the player
        Destroy(playerVineScript);
        //destroy the grapple visual
        Destroy(grapple);
    }
}
