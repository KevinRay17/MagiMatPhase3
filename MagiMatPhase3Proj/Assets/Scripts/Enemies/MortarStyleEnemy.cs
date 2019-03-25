using System.Collections;
 using System.Collections.Generic;
 using UnityEngine;

// REQUIRES: 
// COLLIDER2D
// SPRITE RENDERER
public class MortarStyleEnemy : MonoBehaviour {
    // Public -----------------------------------------------------------------
    
    // What is instantiated and shot out from this GameObject
    // TODO: create projectile GameObject that is instantiated
    public GameObject projectileToInstantiate;
    
    // The bounce pad GameObject that is instantiated after this GameObject dies
    // TODO: create bounce GameObject that is instantiated
    public GameObject bouncePadToInstantiate;
    
    // Offset from the center of the sprite;
    // For example, if the the prefabToInstantiate is being fired from the upper left of the sprite,
    // then offsetFromCenter would look something like (1.0f, 2.0f, 0.0f)
    // DO NOT USE NEGATIVE NUMBERS ON THE X-AXIS, as it's already cast into either negative or positive through code
    public Vector3 offsetFromCenter;
    
    // ^
    public Vector3 bouncePadOffset;
    
    // Height of the arc the instantiatedPrefab travels along
    public float arcHeight = 2.5f;
    
    // The time in-between shots fired;
    // increase to lower fire-rate, decrease to increase fire-rate (counter-intuitive, I know)
    public float timeBetweenShots = 3.0f;
     
    public float detectionRadius = 10.0f;
     
    public float shotSpeed = 1.0f;

    public float secondsToDestroy = 3.0f;

    // Starting health value
    public int startingHealth = 3;

    public float deathAnimationDelay = 0.0f;
    
    // public float cooldownModifier;
    
    [Header("Do not edit these variables!")]
    // DO NOT EDIT IN THE INSPECTOR ---------------------------------------------
    // Controls whether or not the player is in front of or behind the object, changes whereToInstantiate
    // including spriteRenderer.flipX, this script assumes the sprite will be facing left by default
    public bool shouldWeBeFacingRight;
     
    // The instantiated prefabToInstantiate, exposed to track it's transform.position/tweak components
    public GameObject instantiatedPrefab;
     
    public GameObject thePlayer;
    
    public Vector3 playerPosition;
    
    public float ourDistanceFromThePlayer;
    
    // Private  -----------------------------------------------------------------
    private SpriteRenderer thisSpriteRenderer;
    
    private Vector3 whereToInstantiate;

    private Vector3 whereBouncePadIsInstantiated;
    
    private float cooldown;
    
    private Vector3 ShootDir;

    // Our current health
    private int health;
    
    // Position of the target used for calculation
    private Vector3 targetPosition;
    
      void Start()
      {
          thePlayer = GameObject.FindGameObjectWithTag("Player");
    
          thisSpriteRenderer = GetComponent<SpriteRenderer>();

          cooldown = timeBetweenShots;

          health = startingHealth;
      }
    
      public virtual void Update()
      {
          // Stores the player position for reference in the inspector, not used for calculations (so feel free to delete)
          playerPosition = thePlayer.transform.position;

          // Always ensures that detectionRadius is a positive number, to prevent errors
          detectionRadius = Mathf.Abs(detectionRadius);
          
          // Visualizes the attack radius, gives feedback when detectionRadius is edited in the inspector
          Debug.DrawLine(transform.position, new Vector3(detectionRadius, transform.position.y, 0), Color.red);
         
          if (thePlayer != null)
          {
              // This script doesn't use OnTriggerEnter/Stay, detection is done through transform.position instead,
              // so the detection radius can easily be tweaked for each instance
              ourDistanceFromThePlayer = Vector3.Distance(thePlayer.transform.position, this.gameObject.transform.position);
              
              // If thePlayer.transform.position is less than the AttackRadius, begin attacking.
              // Meaning the Player is inside the the detection radius
              if (ourDistanceFromThePlayer <= detectionRadius)
              {
                  BeginToFire();
              }
              else
              {
                  return;
                  // This would be where an Animator var would be set, to switch the sprite back into idle
              }
          }
          
          // Constantly checks our health, to account for any potential non-immediate damage dealt (DoTs, delayed damage, etc.)
          if (health <= 0)
          {
              Die();
          }
      }
    
     // Function that controls the start of the spit to the end of the spit
     public virtual void BeginToFire()
     {
         // Checks against the transform.position of thePlayer to see whether or not this GameObject should be facing left or right 
         if (transform.position.x > thePlayer.transform.position.x)
         {
             whereToInstantiate = transform.position + new Vector3(-offsetFromCenter.x, offsetFromCenter.y, offsetFromCenter.z);
         
             // Assumes the sprite is facing left by default
             shouldWeBeFacingRight = false;
             thisSpriteRenderer.flipX = false; 
         }  
         // ^
         if (transform.position.x < thePlayer.transform.position.x)
         {
             whereToInstantiate = transform.position + new Vector3(offsetFromCenter.x, offsetFromCenter.y, offsetFromCenter.z);
    
             shouldWeBeFacingRight = true;
             thisSpriteRenderer.flipX = true;
         }
    
         // Cooldown is constantly decreased each frame, however if you want cooldown to also be decreased by a specific modifier,
         // un-comment cooldownModifier above as well the code below.
         // Make sure to set cooldownModifier in the inspector!
         cooldown -= Time.deltaTime; //cooldown -= Time.deltaTime * cooldownModifier
         
         // Once the cooldown has expired, begin spitting
         if (cooldown <= 0)
         {
             // Draws a line to the player, for hit-detection checking and debugging purposes
             Debug.DrawLine(transform.position, thePlayer.transform.position, Color.cyan);
             
             targetPosition = thePlayer.transform.position - transform.position;
             
             // Math
             shotSpeed = Mathf.Sqrt(Mathf.Abs(targetPosition.x) / 0.0781678003f);
             
             // Creates the arc that the instantiatedPrefab travels along, edit variables in the inspector to change the shape of this arc
             if (shouldWeBeFacingRight)
             {
                 // More math
                 ShootDir = new Vector3(1.0f, arcHeight, 0f);
             }
             else
             {
                 // Even more math
                 ShootDir = new Vector3(-1.0f, arcHeight, 0f); 
             }
         
             // Any animator variables should be set here, to switch the sprite into an attack animation
             
             // The prefab is instantiated here, at the position whereToInstantiate, with the same transform.position
             // as this GameObject
             instantiatedPrefab = Instantiate(projectileToInstantiate, whereToInstantiate, transform.rotation);
             
             // Where force is added to the instantiatedPrefab, controlled by shotSpeed (assign its value in the inspector)
             instantiatedPrefab.GetComponent<Rigidbody2D>().velocity = ShootDir.normalized * shotSpeed;
             
             // Prefab is destroyed after a number of seconds, secondsToDestroy
             // Or if the prefab should be destroyed by making contact with the ground/walls/etc.
             // use a Destroy() statement inside an OnTriggerEnter function in the projectile script
             Destroy(instantiatedPrefab, secondsToDestroy);
             
             // Cooldown is reset to timeBetweenShots
             cooldown = timeBetweenShots;
             
             // Animator variable(s) to switch the sprite back into idle before firing another shot
         }
     }

    // This script assumes the GameObject we're attached to has a Collider2D, and the GameObject used to harm it
    // (the Player's method of attacking) has both a Collider2D and a Rigidbody2D.
    // If that isn't the case, attach a Rigidbody2D to the GameObject so that this OnTriggerEnter2D function can work as intended
    void OnTriggerEnter2D(Collider2D otherCollider2D)
    {
        // This code makes assumptions about the naming/tag/classification system used in the game.
        // Assumes that any GameObject that deals damage is tagged as 'Hitbox'
        if (otherCollider2D.tag == "HurtBox")
        {
            // Assumes that any GameObject tagged 'Hitbox' has a DealDamage script attached to it, which controls how much damage,
            // what type of damage/etc. the GameObject does, and that this script contains an int 'damage' that is the amount of damage done.
            // 5 is a temporary number so the script can compile
            TakeDamage(/*(int) otherCollider2D.GetComponent<DealDamage>().damage*/ 5);
        }
        else
        {
            return;
        }
    }

    public virtual void TakeDamage(int damageDone)
    {
        // Redundant int cast is done just to be 100% sure we're receiving an int.
        // Health is reduced by the 
        health -= (int) damageDone;
    }

   public virtual void Die()
    {
        // Like whereToInstantiate, see line 130
        whereBouncePadIsInstantiated = transform.position + new Vector3(bouncePadOffset.x, bouncePadOffset.y, bouncePadOffset.z);
        
        Instantiate(bouncePadToInstantiate, whereBouncePadIsInstantiated, transform.rotation);
        
        // Any animator variables would go here.
        // If there is a death animation, make sure to un-comment deathAnimationDelay and set it to the amount of seconds
        // the death animation lasts
        
        // Destroy ourselves once the bounce pad is instantiated and the death animation has played
        Destroy(gameObject, deathAnimationDelay);
    }
}
