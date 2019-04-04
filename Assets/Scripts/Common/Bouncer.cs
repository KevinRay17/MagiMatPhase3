using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncer : MonoBehaviour
{
    private Vector3 startPos;

    public float mod;

    public float spdMod;
    // Use this for initialization
    void Start ()
    {
        startPos = transform.position;
    }
	
    // Update is called once per frame
    void Update ()
    {
        transform.position = startPos + Vector3.up * Mathf.Sin(Time.time *spdMod)*mod;
    }
}