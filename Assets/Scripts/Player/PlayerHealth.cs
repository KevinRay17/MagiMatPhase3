using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//handles player health, death?, collisions?

public class PlayerHealth : MonoBehaviour
{

    public static PlayerHealth PhStatic;
    
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody2D;
    private Collider2D _collider;
    public bool isPlayerDead = false;

    public float health; //TODO: maybe change to int?

    void Awake()
    {
        //assign components
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
    }

    void Update()
    {
        //check if player still has health remaining
        //if not, run death method
        if (health <= 0)
        {
            PlayerManager.instance.Death();
            StartCoroutine(DeathCoroutine());
            isPlayerDead = true;
        }
        else
        {
            isPlayerDead = false;
        }
    }
    
    //call this method to deal damage to player
    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
    }

    IEnumerator DeathCoroutine()
    {
        //TODO: Death animation/Menu/Overlay?
        yield break;
    }
}
