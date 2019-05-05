using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RapidWater : MonoBehaviour
{
    public bool rapidleft;

    public bool rapidRight;

    public bool rapidUp;

    public bool rapidDown;
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
        if (rapidleft)
            player.GetComponent<Rigidbody2D>().AddForce(Vector2.left * 100);
        else if (rapidRight)
            player.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 100);
        else if (rapidUp)
            player.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 100);
        else if (rapidDown)
            player.GetComponent<Rigidbody2D>().AddForce(Vector2.down * 75);
    }
}
