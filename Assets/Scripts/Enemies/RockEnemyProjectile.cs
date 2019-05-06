using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockEnemyProjectile : MonoBehaviour
{
    public float lifetime;
    void Start()
    {
        Destroy(this.gameObject, lifetime);
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (GetComponent<Rigidbody2D>().velocity.magnitude > 2f)
            {
                PlayerManager.instance.playerHealth.TakeDamage(1);
            }
            Destroy(gameObject);
        }
    }
}
