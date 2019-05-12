using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemProjectile : MonoBehaviour
{
    private float lifeTime = 2f;

    private float life = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        life+= 1 *Time.deltaTime;
        if (life > lifeTime)
        {
            var RockLand = Resources.Load<AudioClip>("Sounds/EnemyRockLand");
            AudioManager.instance.PlaySound(RockLand);
            Destroy(gameObject);
            
        }

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerManager.instance.playerHealth.TakeDamage(1);
            var RockLand = Resources.Load<AudioClip>("Sounds/EnemyRockLand");
            AudioManager.instance.PlaySound(RockLand);
            Destroy(gameObject);
        }
    }
}
