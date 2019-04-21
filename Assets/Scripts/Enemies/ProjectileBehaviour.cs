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
        
        Vector3 relativePos = _TargetPos - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        transform.rotation = new Quaternion(0,0,rotation.z, rotation.w);
        


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
            PlayerManager.instance.playerHealth.TakeDamage(1);
            Destroy(gameObject);
        }
        else if (other.gameObject.layer == 8 || other.gameObject.CompareTag("Absorber"))
        {
            Destroy(gameObject);
        }
    }

}
