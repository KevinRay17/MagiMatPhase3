using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ProjectileBehaviour : MonoBehaviour
{
    [HideInInspector] public static Transform _Player;
    [HideInInspector] public Vector3 _TargetPos;
    private float FireballSpeed = 10;
    // Start is called before the first frame update
    void Start()
    {
        _Player = GameObject.Find("Player").transform;
        _TargetPos = _Player.position;
       
    }

    // Update is called once per frame 
    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _TargetPos, FireballSpeed*Time.deltaTime);
        if (transform.position == _TargetPos)
        {         
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            PlayerHealth.PhStatic.health -= 1;
            Destroy(gameObject);
        }
        else if (other.gameObject.layer == 8)
        {
            Destroy(gameObject);
        }
    }

}
