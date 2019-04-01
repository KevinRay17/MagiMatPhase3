using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterAnim : MonoBehaviour
{
    public void DestroyThis()
    {
        Destroy(gameObject);
    }
}
