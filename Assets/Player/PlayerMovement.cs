using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{
    ThirdPersonCharacter m_Character;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster;
    Vector3 currentClickTarget;

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
		if (Input.GetMouseButton (0)) {
			print ("Cursor raycast hit" + cameraRaycaster.hit.collider.gameObject.name.ToString ());

			// SJ - added to only register new location if target point is walkable
			// SJ - Changed from IF to SWITCH afterward
			switch (cameraRaycaster.layerHit) {

			case Layer.Walkable:
				currentClickTarget = cameraRaycaster.hit.point;  // So not set in default case
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
		} else {
			// SJ - inside the radius
			m_Character.Move (Vector3.zero, false, false);

		}
		
    }
}

