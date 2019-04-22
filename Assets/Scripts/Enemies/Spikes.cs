using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerManager.instance.playerHealth.TakeDamage(1);
            other.gameObject.transform.position = PlayerManager.instance.playerMovement.teleLastPos;
            PlayerManager.instance.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(0,0,0);
        }
        else
        {
            Destroy(other.gameObject);
            
        }
    }
}
