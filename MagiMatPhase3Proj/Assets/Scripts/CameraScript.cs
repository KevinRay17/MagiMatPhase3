using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform target;
    public float lerpSpeed;

    void FixedUpdate()
    {
        Vector2 currentPosition = transform.position;
        Vector2 targetPosition = target.position;
        Vector3 newPosition = Vector2.Lerp(currentPosition, targetPosition, lerpSpeed * Time.fixedDeltaTime);
        newPosition.z = -10;

        transform.position = newPosition;
    }
}
