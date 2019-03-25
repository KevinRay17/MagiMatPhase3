using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingHurtBox : HurtBox
{
    public float speed;
    public Vector2 direction;

    protected virtual void Update()
    {
        Vector2 pos = transform.position;
        transform.position = pos + (direction * speed * Time.deltaTime);
    }
}
