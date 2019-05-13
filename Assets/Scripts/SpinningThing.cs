using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningThing : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.Rotate(0,0, 200 * Time.deltaTime);
	}
}
