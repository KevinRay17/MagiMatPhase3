using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineGrappleCollision : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        VineMaterial vineScript = (VineMaterial)MaterialsManager.GetMaterialScript(Material.Vine);
        vineScript.onGrapple = false;
    }
}
