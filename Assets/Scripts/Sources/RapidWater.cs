using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RapidWater : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerStay2D(Collider2D player)
    {
        Debug.Log("Your Drowning");
        player.GetComponent<Rigidbody2D>().AddForce(Vector2.left * 100);
    }
}
