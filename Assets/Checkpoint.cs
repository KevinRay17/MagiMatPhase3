﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// REQUIRES:
// BOX COLLIDER2D set to isTrigger
// Checkpoint script to update the Player's spawnPosition, making the level easier to traverse as well as making exploring
// later sections of the level do-able
public class Checkpoint : MonoBehaviour
{
    private DontDestroy cp;
    public ParticleSystem ding;
    private void Start()
    {
        GameObject particle = GameObject.Find("CPParticle");
        ding = particle.GetComponent<ParticleSystem>();
        cp= GameObject.FindGameObjectWithTag("spawn").GetComponent<DontDestroy>();
    }

    void Awake()
    {
        // TODO: Add this code in the main Player script Awake function.
        // spawnPos is a Vector3 that needs to be declared given coordinates in the Player script.
        // Ideally, the first set of coords is where the player first spawns in,
        // and that set is replaced by the position of the last checkpoint the Player has activated
        // spawnPosition = startingSpawnPosition;
    }

    void OnTriggerEnter2D(Collider2D thingInsideMe)
    {
        // If what just entered our trigger is tagged as the Player, set its spawnPosition is changed to be our current position
        if (thingInsideMe.CompareTag("Player"))
        {
            // spawnPosition is set to our transform.position
            cp.gameObject.transform.position = new Vector3(transform.position.x,transform.position.y +.1f,transform.position.z);
            PlayerManager.instance.playerHealth.health= 5f;
            ding.Play();
        }
    }
}
