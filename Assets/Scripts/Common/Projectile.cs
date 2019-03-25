using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // The tag this projectile is intended to affect.
    // Leave blank if the projectile can affect every GameObject
    public String tagWeAffect;
    
    // All publicly-declared functions are virtual, designed to be overridden by any future classes that
    // inherit from this script (make sure to declare 'override' before overriding any function in inherited scripts)
    public virtual void Start()
    {
        
    }

    public virtual void Update()
    {
        
    }
    
    protected virtual void OnTriggerEnter2D(Collider2D whatWeCollidedWith)
    {
        // If there's something assigned to tagWeAffect in the inspector.
        // (If we DON'T affect every GameObject)
        if (tagWeAffect != null)
        {
            // If what we collided with has the same tag as the one we defined in the inspector
            if (whatWeCollidedWith.gameObject.CompareTag(tagWeAffect))
            {
                ApplyEffect();
            }
        }
        // If this projectile is supposed to affect every GameObject, regardless of tag.
        // (If we DO affect every GameObject)
        else
        {
            ApplyEffect();
        }
        
        Die();
    }

    // Does whatever effect the projectile is intended do to: deal damage/apply status/increase a count/etc.
    // Override this when the projectile does something besides die immediately after colliding with anything
    public virtual void ApplyEffect()
    {
        return;
    }
    
    public virtual void Die()
    {
        // When overriding, animator-related variables should in here, before the Destroy() call
        
        // Un-comment and copy/paste this code to set up a co-routine to wait until the animation is done before destroying.
        // This first part goes inside the Die() function
        // StartCoroutine("PlayAnimBeforeDeath");
        //
        // The IEnumator function goes OUTSIDE Die(), make sure to copy/paste outside of any other function too
        // IEnumator PlayAnimBeforeDeath()
        // {
        //     // The variable doesn't have to be 'secondsToWaitFor', just make sure it's initialized as a float and/or cast into one.
        //     // WaitForSeconds sometimes bugs out if you pass it a negative number.
        //     // To side-step this issue, only the absolute value of 'secondsToWaitFor' is passed to WaitForSeconds
        //     yield return new WaitForSeconds(Mathf.Abs(secondsToWaitFor));
        //
        //     Destroy(gameObject);
        // }
        
        Destroy(gameObject);
    }
}
