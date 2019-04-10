using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {
	private bool isShaking = false;
	private float baseX, baseY, baseZ;
	private float intensity = 0.1f;
	private int shakes = 0;



	// Use this for initialization
	void Start () {
		baseX = transform.position.x;
		baseY = transform.position.y;
		baseZ = transform.position.z;

	}
	
	// Update is called once per frame
	void FixedUpdate () {


		if (isShaking) {
			float randomShakeX = Random.Range (-intensity, intensity);
			float randomShakeY = Random.Range (-intensity, intensity);

			transform.position = new Vector3 (baseX + randomShakeX, baseY + randomShakeY, baseZ);

			shakes--;

			if(shakes <= 0) {
				isShaking = false;
				transform.position = new Vector3 (baseX, baseY, baseZ);
			}
		}
	}

	//Different Shakes
	public void MinorShake(float in_intensity) {
		isShaking = true;
		shakes = 10;
		intensity = in_intensity;
	}

	public void LongShake(float in_intensity) {
		isShaking = true;
		shakes = 100;
		intensity = in_intensity;
	}

	public void PlayerShake(float in_intensity) {
		isShaking = true;
		shakes = 5;
		intensity = in_intensity;
	}
}
