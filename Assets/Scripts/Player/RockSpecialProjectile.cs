using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSpecialProjectile : Projectile
{
    private Rigidbody2D _rigidbody2D;
    
    public float maxDistance;
    private float _distanceTravelled;
    
    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }
    
    public override void Update()
    {
        _distanceTravelled += _rigidbody2D.velocity.magnitude * Time.deltaTime;
        if (_distanceTravelled >= maxDistance)
        {
            Destroy(this.gameObject);
        }
    }
    
    public override void ApplyEffect()
    {
        //do damage
    }
}
