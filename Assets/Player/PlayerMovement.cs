using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{
    ThirdPersonCharacter thirdPersonCharacter;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster;
    Vector3 currentClickTarget;

    private bool isInDirectMode = false;	

    [SerializeField] float walkMoveStopRadius = 0.20f;

        
    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
        currentClickTarget = transform.position;
    }

    // Fixed update is called in sync with physics
    private void FixedUpdate ()
	{

		// SJ - Check for swap between mouse movement and WASD keyboard movement
		if (Input.GetKeyDown (KeyCode.G)) {		
			isInDirectMode = !isInDirectMode;	// Toggle the movement mode
			Debug.Log("G Pressed, switching to " + isInDirectMode);

			// SJ - default out the currentClickTarget so the player doesnt return to last known click point
			currentClickTarget = transform.position;
		}

		if (isInDirectMode) {

			ProcessDirectMovement ();

		} else {

			ProcessMouseMovement ();

		}



		
    }

    // SJ - Keybaord Movement
    private void ProcessDirectMovement ()
	{

		// SJ - get the values
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");

		// SJ - calculate movement, pulled from ThirdPersonUserControl
		// Updated to access camera using the main static
		Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
		Vector3 movement = v*cameraForward + h*Camera.main.transform.right;

		// SJ - Finally, move the character!
		// replaced "crouch" and "m_jump" with false
		thirdPersonCharacter.Move(movement, false, false);

	}


    // SJ - Refactored movement to be called within the FixedUpdate
	private void ProcessMouseMovement ()
	{
		if (Input.GetMouseButton (0)) {
			print ("Cursor raycast hit" + cameraRaycaster.hit.collider.gameObject.name.ToString ());
			// SJ - added to only register new location if target point is walkable
			// SJ - Changed from IF to SWITCH afterward
			switch (cameraRaycaster.currentLayerHit) {
				case Layer.Walkable:
					currentClickTarget = cameraRaycaster.hit.point;
					// So not set in default case
					break;
				case Layer.Enemy:
					Debug.Log ("Clicked on Enemy");
					break;
				default:
					Debug.Log ("Default reached in Switch");
					return;
			}
		}
		// SJ - get the distance of the move between current position and click position
		var playerToClickPoint = currentClickTarget - transform.position;
		// SJ - Move until we get within the radius then stop
		if (playerToClickPoint.magnitude >= walkMoveStopRadius) {
			thirdPersonCharacter.Move (playerToClickPoint, false, false);
		}
		else {
			// SJ - inside the radius
			thirdPersonCharacter.Move (Vector3.zero, false, false);
		}
	}
}

