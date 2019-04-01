using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSpecialProjectile : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    
    public float maxDistance;
    private float _distanceTravelled;
    
    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }
    
    public void Update()
    {
        _distanceTravelled += _rigidbody2D.velocity.magnitude * Time.deltaTime;
        if (_distanceTravelled >= maxDistance)
        {
            Destroy(this.gameObject);
        }
    }
    
    public void ApplyEffect()
    {
        //do damage
    }
}
