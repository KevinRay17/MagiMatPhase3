using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialAbsorberProjectile : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;

    public float maxDistance;
    private float _distanceTravelled;

    public GameObject particlePrefab;
    public int particleAmount;

    public static bool FireUnlocked = false;
    
    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        _distanceTravelled += _rigidbody2D.velocity.magnitude * Time.deltaTime;
        if (_distanceTravelled >= maxDistance)
        {
            DestroySelf();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            return;
        }
        
        if (other.CompareTag("Enemy"))
        {
            Enemy enemyScript = other.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                if (enemyScript.material != Material.None)
                {
                    PlayerManager.instance.ChangeMaterial(enemyScript.material);
                    SpawnParticles();
                }
            }
        } 
        else if (other.CompareTag("MaterialSource"))
        {
            MaterialSource materialSourceScript = other.GetComponent<MaterialSource>();
            if (materialSourceScript != null)
            {
                if (materialSourceScript.material != Material.None && FireUnlocked)
                {
                    ResourceController.currentMana = 100f;
                    PlayerManager.instance.ChangeMaterial(materialSourceScript.material);
                    SpawnParticles();
                }
                else if (materialSourceScript.material == Material.Vine || materialSourceScript.material == Material.Rock)
                {
                    ResourceController.currentMana = 100f;
                    PlayerManager.instance.ChangeMaterial(materialSourceScript.material);
                    SpawnParticles();
                }
            }
        }  else if (other.CompareTag("MaterialUnlock"))
        {
            MaterialSource materialSourceScript = other.GetComponent<MaterialSource>();
            ResourceController.currentMana = 100f;
            PlayerManager.instance.ChangeMaterial(materialSourceScript.material);
            SpawnParticles();
            FireUnlocked = true;
        }

        DestroySelf();
    }

    private void SpawnParticles()
    {
        for (int i = 0; i < particleAmount; i++)
        {
            Vector2 pos = transform.position;
            GameObject particle = Instantiate(particlePrefab, pos, Quaternion.identity);
        }
    }
    
    private void DestroySelf()
    {
        PlayerManager.instance.playerActions.materialAbsorberOut = false;
        Destroy(this.gameObject);  
    }
}
