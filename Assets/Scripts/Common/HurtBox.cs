using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtBox : MonoBehaviour
{
    [HideInInspector] public SpriteRenderer _spriteRenderer;
    [HideInInspector] public BoxCollider2D _boxCollider;

    public float damage;
    public bool hitMultipleTargets;
    
    public float lifetime;
    public ParticleSystem bloodParticleSystem;
    
    protected virtual void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    protected virtual void Start()
    {
        Destroy(this.gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 13 || other.gameObject.layer == 14)
        {
            bloodParticleSystem.transform.position = other.gameObject.transform.position;
            bloodParticleSystem.Emit(20);
        }
        
        if (PlayerManager.instance.material == Material.Fire && other.gameObject.CompareTag("Torch"))
        {
            Debug.Log("BURN");
        }

        if (PlayerManager.instance.material == Material.Fire && other.gameObject.CompareTag("VineWall"))
        {
            Destroy(other.gameObject);
        }
        
    }
}
