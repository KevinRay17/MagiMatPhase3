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
        if (CheckGrounded())
        {
            //Debug.Log("hi");
            Destroy(this.gameObject);
        }
    }

    protected virtual bool CheckGrounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, raycastDistance, groundLayers);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (PlayerManager.instance.playerActions.groundPounding)
        {
            if (collision.gameObject.CompareTag("Breakable"))
            {
                Destroy(collision.gameObject);
            }
            if (collision.gameObject.layer == 8)
            {
                RockMaterial cs = FindObjectOfType<RockMaterial>();
                cs.a = false;
                Destroy(gameObject);
            }
        }
        
        /*
        if (collision.gameObject.CompareTag("ground"))
        {
            Destroy(gameObject);
            RockMaterial cs = FindObjectOfType<RockMaterial>();
            cs.a = false;
        }
        */
    }
}
