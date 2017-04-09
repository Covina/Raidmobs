using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	private float damageCaused;
	public float projectileSpeed;

	public void SetDamage (float damage)
	{
		damageCaused = damage;

	}


	void OnCollisionEnter (Collision collision)
	{

		// SJ - Is the colliding object Damageable?  If it is, it will have inherited from IDamageable 
		// ... and be able to be identified as having a component of IDamageable
		Component damageableComponent = collision.gameObject.GetComponent (typeof(IDamageable));

		// SJ - if it has an IDamageable component on it, proceed.
		if (damageableComponent) {

			// SJ - Cast it has IDamageable to access and call its TakeDamage method
			// ... The individual actions from taking damage are defined within the Damageable objects (Player, Enemy, etc).
			(damageableComponent as IDamageable).TakeDamage(damageCaused);

			// If the projectile damaged its target, remove it from scene
			Destroy(gameObject);


		}

	}




}
