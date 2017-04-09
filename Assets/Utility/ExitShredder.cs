using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitShredder : MonoBehaviour {


	void OnTriggerExit(Collider collider) {

		//Debug.Log("Destroying " + collider);
		Destroy(collider.gameObject);

	}


}
