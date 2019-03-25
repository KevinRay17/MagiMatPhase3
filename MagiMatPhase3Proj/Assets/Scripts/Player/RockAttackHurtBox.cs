using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockAttackHurtBox : MovingHurtBox
{
    public float raycastDistance;
    public LayerMask groundLayers;
    
    protected override void Update()
    {
        base.Update();
        if (!CheckGrounded())
        {
            Destroy(this.gameObject);
        }
    }

    protected virtual bool CheckGrounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, raycastDistance, groundLayers);
    }
}
