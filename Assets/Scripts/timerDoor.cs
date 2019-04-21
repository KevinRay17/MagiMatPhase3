using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timerDoor : MonoBehaviour
{
    public GameObject button1;
    public GameObject button2;
    public GameObject button3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (button1.gameObject.GetComponent<timerButtons>().isActive == true && button3.gameObject.GetComponent<timerButtons>().isActive && button1.gameObject.GetComponent<timerButtons>().isActive)
        {
            Destroy(this.gameObject);
        }
        
    }
}
