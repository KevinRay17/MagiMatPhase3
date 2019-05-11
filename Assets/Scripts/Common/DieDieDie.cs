using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieDieDie : MonoBehaviour
{

    void Update()
    {
        if (Input.GetKey(KeyCode.F))
        {
            PlayerManager.instance.playerHealth.TakeDamage(5);
        }
    }
}
