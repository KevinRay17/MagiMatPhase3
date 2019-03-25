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
        if (PlayerManager.instance.material == Material.Fire && other.gameObject.CompareTag("Torch"))
        {
            Debug.Log("BURN");
        }
    }
}
