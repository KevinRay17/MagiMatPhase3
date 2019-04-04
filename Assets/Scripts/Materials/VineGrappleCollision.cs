using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineGrappleCollision : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DetachVineGrapple();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        DetachVineGrapple();
    }

    void DetachVineGrapple()
    {
        VineMaterial vineScript = (VineMaterial) MaterialsManager.GetMaterialScript(Material.Vine);
        vineScript.onGrapple = false;
    }
}