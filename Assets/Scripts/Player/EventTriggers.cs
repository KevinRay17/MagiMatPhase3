using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;

public class EventTriggers : MonoBehaviour
{
    public Camera camera;

    private float _targetScale = 6.4f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (camera.orthographicSize != _targetScale)
        {
            camera.orthographicSize += (_targetScale - camera.orthographicSize) * .05f;
        }
       
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("camTrigger"))
        {
            _targetScale = 15f;
        } else if (other.gameObject.CompareTag("camResetScale"))
        {
            _targetScale = 6.4f;
        }
    } 
    
}
