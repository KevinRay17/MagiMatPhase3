using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Rename this script to something else clearer, preferrably: 'MakeImageBounce' or 'UIBobEffect'
public class Bouncer : MonoBehaviour
{
    // Starting position of the gameObject we're attached to
    private Vector3 startPosition;

    public float modifier;

    public float speedModifier;
    
    // Use this for initialization
    void Start ()
    {
        startPosition = transform.position;
    }
	
    // Update is called once per frame
    void Update ()
    {
        transform.position = startPosition + Vector3.up * Mathf.Sin(Time.time * speedModifier) * modifier;
    }
}
