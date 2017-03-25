﻿using UnityEngine;

public class CameraRaycaster : MonoBehaviour
{
    public Layer[] layerPriorities = {
        Layer.Enemy,
        Layer.Walkable
    };

    float distanceToBackground = 100f;
    Camera viewCamera;

    RaycastHit raycastHit;
    public RaycastHit hit
    {
        get { return raycastHit; }
    }

    Layer layerHit;
    public Layer currentLayerHit
    {
        get { return layerHit; }
    }


    // SJ - Declare the delegate 
    public delegate void OnLayerChangeObservers(Layer newLayer); 	
    // SJ - Declare the Observer
	public event OnLayerChangeObservers layerChangeObservers;	// SJ - instantiate an observer set


    void Start() 
    {
        viewCamera = Camera.main;

    }

    void Update ()
	{
		// Look for and return priority layer hit
		foreach (Layer layer in layerPriorities) {
			var hit = RaycastForLayer (layer);
			if (hit.HasValue) {

                raycastHit = hit.Value;

				// SJ - Layer changed so call all of the observers
				if (layerHit != layer) {

					layerHit = layer;
					layerChangeObservers(layer);

				}



                return;
            }
        }

        // Otherwise return background hit
        raycastHit.distance = distanceToBackground;
        layerHit = Layer.RaycastEndStop;
    }

    RaycastHit? RaycastForLayer(Layer layer)
    {
        int layerMask = 1 << (int)layer; // See Unity docs for mask formation
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit; // used as an out parameter
        bool hasHit = Physics.Raycast(ray, out hit, distanceToBackground, layerMask);
        if (hasHit)
        {
            return hit;
        }
        return null;
    }
}
