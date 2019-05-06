using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineEnemyHitBox : MonoBehaviour
{
    void Start()
    {
        Destroy(this.gameObject, 0.1f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            PlayerManager.instance.playerHealth.TakeDamage(1);
            Destroy(gameObject);
        }
    }
}
