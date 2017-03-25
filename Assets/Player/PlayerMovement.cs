using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{
    ThirdPersonCharacter m_Character;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster;
    Vector3 currentClickTarget;

    private bool isInDirectMode = false;	// TODO consider making this a static later

    [SerializeField] float walkMoveStopRadius = 0.20f;

        
    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        m_Character = GetComponent<ThirdPersonCharacter>();
        currentClickTarget = transform.position;
    }

    // Fixed update is called in sync with physics
    private void FixedUpdate ()
	{

		// SJ - Check for swap between mouse movement and WASD keyboard movement
		if (Input.GetKeyDown (KeyCode.G)) {		// TODO potentially allow players to remap or add in menu
			isInDirectMode = !isInDirectMode;	// Toggle the movement mode
			Debug.Log("G Pressed, switching to " + isInDirectMode);
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
		Vector3 m_CamForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
		Vector3 m_Move = v*m_CamForward + h*Camera.main.transform.right;

		// SJ - Finally, move the character!
		// replaced "crouch" and "m_jump" with false
		m_Character.Move(m_Move, false, false);

	}


    // SJ - Refactored movement to be called within the FixedUpdate
	private void ProcessMouseMovement ()
	{
		if (Input.GetMouseButton (0)) {
			print ("Cursor raycast hit" + cameraRaycaster.hit.collider.gameObject.name.ToString ());
			// SJ - added to only register new location if target point is walkable
			// SJ - Changed from IF to SWITCH afterward
			switch (cameraRaycaster.layerHit) {
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
			m_Character.Move (playerToClickPoint, false, false);
		}
		else {
			// SJ - inside the radius
			m_Character.Move (Vector3.zero, false, false);
		}
	}
}

