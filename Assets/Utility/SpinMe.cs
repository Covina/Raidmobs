using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinMe : MonoBehaviour {

	[SerializeField] float xRotationsPerMinute = 1f;
	[SerializeField] float yRotationsPerMinute = 1f;
	[SerializeField] float zRotationsPerMinute = 1f;
	
	void Update () {


		// 360 degrees * rotationsPerMinute = total degrees per minute
		// TotalDegree / 60 = degrees per second
		// DPS * Time.Deltatime = degree movement per frame


        float xDegreesPerFrame = 0; // TODO COMPLETE ME
        transform.RotateAround (transform.position, transform.right, xDegreesPerFrame);

		float yDegreesPerFrame = 0; // TODO COMPLETE ME
        transform.RotateAround (transform.position, transform.up, yDegreesPerFrame);

		float zDegreesPerFrame = ((360 * zRotationsPerMinute) / 60) * Time.deltaTime; // TODO COMPLETE ME
        transform.RotateAround (transform.position, transform.forward, zDegreesPerFrame);
	}
}
