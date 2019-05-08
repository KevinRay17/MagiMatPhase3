using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//handles enemy flicker when taking damage
//done by enabling a solid colored sprite with a spriteMask that is shaped to the parent sprite
public class EnemySpriteMask : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private SpriteMask _spriteMask;
    
    private float onHitFlickerTimer;
    public float onHitFlickerDuration;
    public Color flickerColor;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteMask = GetComponent<SpriteMask>();
        _spriteRenderer.color = flickerColor;
        _spriteRenderer.enabled = false;
    }

    void Update()
    {
        //if onHitFlickerTimer is greater than 0, the spriteRenderer should be enabled
        if (onHitFlickerTimer > 0)
        {
            _spriteMask.sprite = transform.parent.GetComponent<SpriteRenderer>().sprite;
            //the shape of the flicker is based on the spriteMask, but there is no flipX bool, so we change localScale instead
            if (transform.parent.GetComponent<SpriteRenderer>().flipX)
            {
                transform.localScale = new Vector3(-1,1,1);
            }
            else
            {
                transform.localScale = new Vector3(1,1,1);
            }
            _spriteRenderer.enabled = true;
            onHitFlickerTimer -= Time.deltaTime;
            if (onHitFlickerTimer <= 0)
            {
                //when the timer reaches zero again, re-disable the spriteRenderer
                _spriteRenderer.enabled = false;
            }
        }
    }

    public void Flicker()
    {
        //call this method when enemy is hit
        //sets timer to onHitFlickerDuration
        onHitFlickerTimer = onHitFlickerDuration;
    }
}
