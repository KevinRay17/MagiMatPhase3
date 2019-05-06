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
        if (onHitFlickerTimer > 0)
        {
            _spriteMask.sprite = transform.parent.GetComponent<SpriteRenderer>().sprite;
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
                _spriteRenderer.enabled = false;
            }
        }
    }

    public void Flicker()
    {
        onHitFlickerTimer = onHitFlickerDuration;
    }
}
