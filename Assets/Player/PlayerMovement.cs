using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{
    ThirdPersonCharacter thirdPersonCharacter;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster;

    private Vector3 currentDestination;
    private Vector3 clickPoint;


    private bool isInDirectMode = false;	

	[SerializeField] float walkMoveStopRadius = 0.20f;
	[SerializeField] float attackMoveStopRadius = 2.0f;



        
    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
        currentDestination = transform.position;
    }

    // Fixed update is called in sync with physics
    private void FixedUpdate ()
	{

		// SJ - Check for swap between mouse movement and WASD keyboard movement
		if (Input.GetKeyDown (KeyCode.G)) {		
			isInDirectMode = !isInDirectMode;	// Toggle the movement mode
			Debug.Log("G Pressed, switching to " + isInDirectMode);

			// SJ - default out the currentClickTarget so the player doesnt return to last known click point
			currentDestination = transform.position;
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

			clickPoint = cameraRaycaster.hit.point;		// store the mouse click point

			switch (cameraRaycaster.currentLayerHit) {
				case Layer.Walkable:
					currentDestination = ShortDestination(clickPoint, walkMoveStopRadius);
					break;
				case Layer.Enemy:
					currentDestination = ShortDestination(clickPoint, attackMoveStopRadius);
					break;
				default:
					Debug.Log ("Default reached in Switch");
					return;
			}
		}

		WalkToDestination ();

	}

	// SJ - Extracted method
	// move the character
	private void WalkToDestination ()
	{
		// SJ - get the distance of the move between current position and click position
		var playerToClickPoint = currentDestination - transform.position;
		// SJ - Move until we get within the radius then stop
		if (playerToClickPoint.magnitude >= walkMoveStopRadius) {
			thirdPersonCharacter.Move (playerToClickPoint, false, false);
		}
		else {
			// SJ - inside the radius
			thirdPersonCharacter.Move (Vector3.zero, false, false);
		}
	}


	private Vector3 ShortDestination (Vector3 destination, float shortening)
	{
			// calculate the subtraction vector
			Vector3 reductionVector = (destination - transform.position).normalized * shortening;	

			// return the shorter vector.
			return (destination - reductionVector);

	}


	void OnDrawGizmos()
	{

		print("Gizmos Draw");

		Gizmos.color = Color.black;
		Gizmos.DrawLine(transform.position, currentDestination);

		Gizmos.DrawSphere(currentDestination, 0.1f);	// draw sphere at end of line
		Gizmos.DrawSphere(clickPoint, 0.2f);

		// Draw Attack Sphere
		Gizmos.color = new Color(255f, 0f, 0f, 0.5f);
		Gizmos.DrawWireSphere(transform.position, attackMoveStopRadius);


	}





}

