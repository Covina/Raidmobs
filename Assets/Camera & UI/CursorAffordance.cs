using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CameraRaycaster))]	// SJ - mandatory!
public class CursorAffordance : MonoBehaviour {


	// SJ - Cursor art hookups
	[SerializeField] Texture2D walkCursor = null;
	[SerializeField] Texture2D attackCursor = null;
	[SerializeField] Texture2D errorCursor = null;

	// where the coordinates are looking
	[SerializeField] Vector2 cursorHotspot = new Vector2(0,0);


	// get access to the raycaster
	private CameraRaycaster cameraRaycaster;

	// Use this for initialization
	void Start () {
		cameraRaycaster = GetComponent<CameraRaycaster>();


		cameraRaycaster.layerChangeObservers += OnLayerChange;	// SJ - register the observer
	}

	// SJ - Observer function
	public void OnLayerChange ()
	{

		//print("OnLayerChange reporting for duty");
		
		// SJ - update the cursor based on the what we're mousing over
		switch (cameraRaycaster.currentLayerHit) {
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
				Debug.Log ("Unknown Layer; No Cursor Handling");
				return;
		}
	}
	

}
