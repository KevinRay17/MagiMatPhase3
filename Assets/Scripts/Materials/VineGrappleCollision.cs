using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineGrappleCollision : MonoBehaviour
{
    private VineMaterial _vineScript = (VineMaterial) MaterialsManager.GetMaterialScript(Material.Vine);
    void Update()
    {   
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DetachVineGrapple();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        PlayerManager.instance.playerMovement.isClimbing = true;
        _vineScript.onWall = true;

    }

    void DetachVineGrapple()
    {
        _vineScript.onGrapple = false;
    }
}