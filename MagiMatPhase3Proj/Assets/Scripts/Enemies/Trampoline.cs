using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// REQUIRES:
// Collider2D set to IsTrigger
// Script for the Jump Pad instantiated by the Spitter enemy.
// Have this script attached to the Jump Pad prefab, or one of its children
public class Trampoline : MonoBehaviour
{
    // Float that controls how much force is added to the player when entering the trigger
    public float bounceForce;

    // Set inside OnTriggerEnter2D, instead of repeated calls to GetComponent<> and slowing down the game,
    // once the player's been inside the collider once, we store its RigidBody for later use
    private Rigidbody2D playerRigidbody;
    
    // Where we check for collisions, and if the Player happened to collide with us, apply the bounceForce variable
    private void OnTriggerEnter2D(Collider2D thingICollidedWith)
    {
        // Check if what just collided with us was the Player, and if it was, continue with the script
        if (thingICollidedWith.gameObject.CompareTag("Player"))
        {
            Debug.Log("Your Mom");
            // Get and store the Player's rigidbody for later use
            playerRigidbody = thingICollidedWith.gameObject.GetComponent<Rigidbody2D>();
            
            // Apply bounceForce
            playerRigidbody.AddForce(transform.up * bounceForce);
        }
    }
}
