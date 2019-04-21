using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timerButtons : MonoBehaviour
{
    public float personalTimer = 1f;
    public bool isActive = false;
    public int materialNeeded;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        personalTimer += Time.deltaTime;


        if (personalTimer < 0.5f)
        {
            isActive = true;
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }
        else
        {
            isActive = false;
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;

        }
        
    }
    
    
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("HurtBox"))
        {

            if (materialNeeded == 3)
            {
                if (PlayerManager.instance.material == Material.Rock)
                {
                    personalTimer = 0;
                }
                
            }
        }
    }
}
