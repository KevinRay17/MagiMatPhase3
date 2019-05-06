using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YouDiedScript : MonoBehaviour
{

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
