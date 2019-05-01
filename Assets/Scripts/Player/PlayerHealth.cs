using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

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
    
    public float hitPauseTimeScale = 0.0f;
    public float hitPauseDuration = 0.5f;

    //damage overlay
    public Image dmgOverlayImage;
    private Color dmgOverlayColor;

    void Awake()
    {
        //assign components
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();

        startingColor = _spriteRenderer.color;

        //set dmg overlay image to transparent at load in
        dmgOverlayColor = dmgOverlayImage.color;
        dmgOverlayColor.a = 0f;
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

        //make image appear while alpha value is not transparent
        //increase transparency over time (set actual color to be the same as temporary color)
        //deactivate image when transparent
        if (dmgOverlayColor.a > 0f)
        {
            dmgOverlayImage.enabled = true;
            dmgOverlayColor.a -= Time.deltaTime * 2f; //setting temporary color
            dmgOverlayImage.color = dmgOverlayColor; //setting actual color to temporary color
        }
        else
            dmgOverlayImage.enabled = false; //here because i can't get it to start out transparent :/
        dmgOverlayColor.a = Mathf.Clamp(dmgOverlayColor.a, -0.1f, 1f); //clamping so that the value never goes to infinity and crashes everything if you're doing a no dmg run
    }

    //call this method to deal damage to player
    public void TakeDamage(float damageAmount)
    {
        if (!invincible)
        {
            //make dmg overlay image pop up when damaged (alpha value from 0-1)
            dmgOverlayColor.a = 1f;
            
            
            var PlayerHurt = Resources.Load<AudioClip>("Sounds/PlayerHurt");
            AudioManager.instance.PlaySound(PlayerHurt);

            health -= damageAmount;
            _invincibilityTimer = invincibilityDuration;
            StartCoroutine(HitPause());
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
        if (collision.gameObject.layer == 14)
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

    IEnumerator HitPause()
    {
        //Set timeScale to the pause timescale that we want
        //Also set fixedDeltaTime (for FixedUpdate and Physics) to the matching value (usually 0.02f)
        Time.timeScale = hitPauseTimeScale;
        Time.fixedDeltaTime = hitPauseTimeScale * 0.02f;
        //Wait for the pause duration -- using WaitForSecondsRealtime so it isn't affected by the timeScale which may be 0
        yield return new WaitForSecondsRealtime(hitPauseDuration);
        //Reset timeScale and fixedDeltaTime to default values
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = 0.02f;
    }
}
