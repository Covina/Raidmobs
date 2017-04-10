using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {


	// Get player position
	// Calculate offset from camera
	// Move camera on LateUpdate into position

	// Get the player object
	private GameObject playerPos;


	// Use this for initialization
	void Start () 
	{

		// Get Player
		playerPos = GameObject.FindGameObjectWithTag("Player");
		//Debug.Log("Player: " + playerPos);

	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}


	// Moev the Camera
	void LateUpdate ()
	{

		// move the camera arm based on the offset
		transform.position = playerPos.transform.position;

	}
}
