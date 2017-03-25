using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorAffordance : MonoBehaviour {


	// SJ - Cursor art hookups
	[SerializeField] Texture2D walkCursor = null;
	[SerializeField] Texture2D attackCursor = null;
	[SerializeField] Texture2D errorCursor = null;

	// where the coordinates are looking
	[SerializeField] Vector2 cursorHotspot = new Vector2(96,96);


	// get access to the raycaster
	private CameraRaycaster cameraRaycaster;

	// Use this for initialization
	void Start () {
		cameraRaycaster = GetComponent<CameraRaycaster>();
	}
	
	// Update is called once per frame
	// SJ - Changed to LateUpdate
	void LateUpdate ()
	{

		//Debug.Log("Layer hit: " + cameraRaycaster.layerHit);

		// SJ - update the cursor based on the what we're mousing over
		switch (cameraRaycaster.layerHit) {

			case Layer.Walkable:
				Cursor.SetCursor (walkCursor, cursorHotspot, CursorMode.Auto);
				break;
			case Layer.Enemy:
				Cursor.SetCursor (attackCursor, cursorHotspot, CursorMode.Auto);
				break;
			case Layer.RaycastEndStop:
				Cursor.SetCursor (errorCursor, cursorHotspot, CursorMode.Auto);
				break;

			
			default:
				Debug.Log("Unknown Layer; No Cursor Handling");
				return;

		}			


	}
}
