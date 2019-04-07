using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//handles player health, death?, collisions?

public class PlayerHealth : MonoBehaviour
{   
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody2D;
    private Collider2D _collider;
    
    public bool isPlayerDead = false;
    private Color startingColor;

    public float health; //TODO: maybe change to int?

    public bool invincible;
    public float invincibilityDuration;
    public int invFlickersPerSec;
    private float _invincibilityTimer;

    void Awake()
    {
        //assign components
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();

        startingColor = _spriteRenderer.color;
    }

    void Update()
    {
    /* Commented out this code, old death script that was replaced/moved to the Death.cs script
        //check if player still has health remaining
        //if not, run death method
        if (health <= 0)
        {
            PlayerManager.instance.Death();
            isPlayerDead = true;
        }
        else
        {
            isPlayerDead = false;
        }
     */

        //check if player is still invincible
        if (_invincibilityTimer > 0)
        {
            invincible = true;
            _invincibilityTimer -= Time.deltaTime;
        }
        else
        {
            invincible = false;
        }
        
        UpdateColor();
    }
    
    //call this method to deal damage to player
    public void TakeDamage(float damageAmount)
    {
        if (!invincible)
        {
            health -= damageAmount;
            _invincibilityTimer = invincibilityDuration;
        }
    }

    void UpdateColor()
    {
        //check what color the spriteRenderer should be
        if (invincible && InvincibleFlicker())
        {
            _spriteRenderer.color = Color.clear;
        }
        else
        {
            _spriteRenderer.color = startingColor;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 13)
        {
            TakeDamage(1);
        }
    }

    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 13)
        {
            TakeDamage(1);
        }
    }
    */

    bool InvincibleFlicker()
    {
        float sinTemp = Mathf.Sign(Mathf.Sin(_invincibilityTimer * invFlickersPerSec * 4));
        if (sinTemp >= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
