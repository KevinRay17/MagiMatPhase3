using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{

    public GameObject wallToDestroy;

    public int materialNeeded;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("HurtBox"))
        {
            if (materialNeeded == 1)
            {
                if (PlayerManager.instance.material == Material.None)
                {
                    Destroy(wallToDestroy);
                }

            }

            if (materialNeeded == 2)
            {
                if (PlayerManager.instance.material == Material.Vine)
                {
                    Destroy(wallToDestroy);
                }
            }
        }
    }
}
