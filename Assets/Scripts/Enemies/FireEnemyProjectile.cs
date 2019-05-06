using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEnemyProjectile : MonoBehaviour
{
    
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            PlayerManager.instance.playerHealth.TakeDamage(1);
            Destroy(gameObject);
        }
        else if (other.gameObject.layer == 8 || other.gameObject.CompareTag("Absorber"))
        {
            Destroy(gameObject);
        }
    }
}
