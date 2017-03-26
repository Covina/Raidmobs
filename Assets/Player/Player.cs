using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable {

	
	[SerializeField] private float maxHealthPoints = 100f;
	private float currentHealthPoints = 100f;

	// Use this for initialization
	void Start () {
		
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


	public void TakeDamage(float damage)
	{
		// calculate damage
		currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);

	}

}
