using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable {

	
	[SerializeField] float maxHealthPoints = 100f;
	private float currentHealthPoints = 100f;

	private const int walkableLayerNumber = 8;
	private const int enemyLayerNumber = 9;

	GameObject currentEnemyTarget;
	CameraRaycaster cameraRaycaster;

	public float playerDamagePerHit;
	public float minTimeBetweenHits = 0.5f;
	public float lastHitTime = 0f;
	public float maxAttackRange = 2f;

	// Use this for initialization
	void Start () {
		cameraRaycaster = GameObject.FindObjectOfType<CameraRaycaster>();

		// register the observering function
		cameraRaycaster.notifyMouseClickObservers += OnMouseClick;

		currentHealthPoints = maxHealthPoints;

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// SJ - Getter for Health as percentage
	public float healthAsPercentage {
		get {
			return currentHealthPoints / maxHealthPoints;
		}
	}


	public void TakeDamage (float damage)
	{
		// calculate damage
		currentHealthPoints = Mathf.Clamp (currentHealthPoints - damage, 0f, maxHealthPoints);

		if (currentHealthPoints <= 0) {
			//Destroy(gameObject);
		}

	}



	// Observer Function to find enemy and damage it.
	void OnMouseClick (RaycastHit raycastHit, int layerHit)
	{

		// did we hit an enemy layer object?
		if (layerHit == enemyLayerNumber) {

			// get which enemy object it was
			var enemy = raycastHit.collider.gameObject;
			Debug.Log ("Mouse clicked on Enemy [" + enemy.name + "]");

			// Is the object even within attack Range?
			if ( (enemy.transform.position - transform.position).magnitude > maxAttackRange) {
				// No, so break out of function.
				return;
			}


			// assign the enemy
			currentEnemyTarget = enemy;

			// enforce that its an actual Enemy object
			var enemyComponent = currentEnemyTarget.GetComponent<Enemy> ();


			// Control the hit speed
			if (Time.time - lastHitTime > minTimeBetweenHits) {
				// damage the enemy
				enemyComponent.TakeDamage (playerDamagePerHit);
				lastHitTime = Time.time;
			}


			//currentEnemyTarget.GetComponent<IDamageable>().TakeDamage(playerDamagePerHit);

		}



	}



}
