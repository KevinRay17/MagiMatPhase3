using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public class EventTriggers : MonoBehaviour
{
    public Camera camera;

    private float _targetScale = 8f;


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
        //When moving through a camera trigger change cmaera size
        if (other.gameObject.CompareTag("camTrigger"))
        {
            _targetScale = 15f;
        } else if (other.gameObject.CompareTag("camResetScale"))
        {
            _targetScale = 6.4f;
        }

        //When moving through water and you are fire, put yourself out
        if (other.gameObject.CompareTag("Water"))
        {
            GameObject splash = GameObject.FindGameObjectWithTag("splash");
            splash.transform.position = new Vector3(transform.position.x,transform.position.y - 1f, transform.position.z);
            splash.GetComponent<ParticleSystem>().Play();
        }
        if (other.gameObject.CompareTag("Water") && PlayerManager.instance.material == Material.Fire)
        {
            ResourceController.currentMana = 0;
            PlayerManager.instance.material = Material.None;
            PlayerManager.instance.ChangeMaterial(Material.None);
            
        }

    } 
    
}
