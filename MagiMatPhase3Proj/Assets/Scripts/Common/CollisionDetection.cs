using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("EnemyProjectile"))
        {
            PlayerManager.instance.playerHealth.TakeDamage(1);
        }
        else if (other.CompareTag("StoneProjectile"))
        {
            PlayerManager.instance.playerHealth.TakeDamage(2);
        }
        else if (other.CompareTag("Enemy"))
        {
            PlayerManager.instance.playerHealth.TakeDamage(1);
        }
        
    }
}
