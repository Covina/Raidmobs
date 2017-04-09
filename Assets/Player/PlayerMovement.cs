using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.AI;

[RequireComponent(typeof (NavMeshAgent))]
[RequireComponent(typeof (AICharacterControl))]
[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{
	ThirdPersonCharacter thirdPersonCharacter = null;   // A reference to the ThirdPersonCharacter on the object
	CameraRaycaster cameraRaycaster = null;
    AICharacterControl aiCharacterControl = null;
    GameObject walkTarget = null;


    private Vector3 currentDestination, clickPoint;

	private const int walkableLayerNumber = 8;
	private const int enemyLayerNumber = 9;


    private bool isInDirectMode = false;	

	[SerializeField] float walkMoveStopRadius = 0.20f;
	[SerializeField] float attackMoveStopRadius = 2.0f;



        
    private void Start()
    {
        cameraRaycaster 		= Camera.main.GetComponent<CameraRaycaster>();
        thirdPersonCharacter 	= GetComponent<ThirdPersonCharacter>();
        aiCharacterControl 		= GetComponent<AICharacterControl>();
        walkTarget				= new GameObject("walkTarget");

        currentDestination = transform.position;

        // Register the observer
		cameraRaycaster.notifyMouseClickObservers += ProcessMouseClick;
    }


	void ProcessMouseClick (RaycastHit raycastHit, int layerHit)
	{

		currentDestination = raycastHit.point; 

		switch (layerHit) {
			case enemyLayerNumber:	
				// Navigate to Enemy
				GameObject enemy = raycastHit.collider.gameObject;
				aiCharacterControl.SetTarget(enemy.transform);
				break;
			case walkableLayerNumber:	// Walkable
				walkTarget.transform.position = raycastHit.point;
				aiCharacterControl.SetTarget(walkTarget.transform);
				break;

			default:
				Debug.Log ("Default reached in ProcessMouseMovement() Switch");
				return;
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


//    // SJ - Refactored movement to be called within the FixedUpdate
//	private void ProcessMouseMovement (RaycastHit firstArg, int layerHit)
//	{
//
//		Debug.Log("ProcessMouseMovement().  Point: [" + firstArg.transform.position + "], Second Arg: [" + layerHit + "]");
//
//
//
//		switch (layerHit) {
//			case 8:	// Walkable
//				currentDestination = ShortDestination(clickPoint, walkMoveStopRadius);
//				break;
//			case 9:	// Enemy
//				currentDestination = ShortDestination(clickPoint, attackMoveStopRadius);
//				break;
//			default:
//				Debug.Log ("Default reached in ProcessMouseMovement() Switch");
//				return;
//		}
//
//		WalkToDestination ();
//
//	}
//
//	// SJ - Extracted method
//	// move the character
//	private void WalkToDestination ()
//	{
//		// SJ - get the distance of the move between current position and click position
//		var playerToClickPoint = currentDestination - transform.position;
//		// SJ - Move until we get within the radius then stop
//		if (playerToClickPoint.magnitude >= walkMoveStopRadius) {
//			thirdPersonCharacter.Move (playerToClickPoint, false, false);
//		}
//		else {
//			// SJ - inside the radius
//			thirdPersonCharacter.Move (Vector3.zero, false, false);
//		}
//	}
//
//
//	private Vector3 ShortDestination (Vector3 destination, float shortening)
//	{
//			// calculate the subtraction vector
//			Vector3 reductionVector = (destination - transform.position).normalized * shortening;	
//
//			// return the shorter vector.
//			return (destination - reductionVector);
//
//	}
//
//
	void OnDrawGizmos()
	{

		//print("Gizmos Draw");

		Gizmos.color = Color.black;
		Gizmos.DrawLine(transform.position, currentDestination);

		Gizmos.DrawSphere(currentDestination, 0.1f);	// draw sphere at end of line
		Gizmos.DrawSphere(clickPoint, 0.2f);

		// Draw Attack Sphere
		Gizmos.color = new Color(255f, 0f, 0f, 0.5f);
		Gizmos.DrawWireSphere(transform.position, attackMoveStopRadius);


	}





}

