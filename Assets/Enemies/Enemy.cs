using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.AI;	// SJ - required to get access to Nav Mesh Agent component


public class Enemy : MonoBehaviour, IDamageable {

	public float maxHealthPoints;
	public float chaseRadius;
	public float attackRadius;
	public float damagePerShot = 7f;
	public float secondsBetweenShots = 0.5f;
	private float timeLastFired;


	// Projectile Info
	[SerializeField] GameObject projectileToUse;
	[SerializeField] GameObject projectileSocket;
	[SerializeField] Vector3 aimOffset = new Vector3(0,1,0);



	// SJ - Get all the component access for the enemy detecting player bit
	private ThirdPersonCharacter thirdPersonCharacter = null;
	private AICharacterControl aiCharacterControl;
	private NavMeshAgent navMeshAgent;
	private Player player;

	// SJ - Calculate how far away the player is
	private float distanceFromPlayer;

	private float currentHealthPoints;
	private bool isAttacking = false;

	// Use this for initialization
	void Start () {

		// SJ - Gain access to all the components on the Enemy
		thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
		aiCharacterControl = GetComponent<AICharacterControl>();
		navMeshAgent = GetComponent<NavMeshAgent>();
		player = GameObject.FindObjectOfType<Player>();

		// set the HP so the UI bar works if HP is scaled up.
		currentHealthPoints = maxHealthPoints;
	}
	
	// Update is called once per frame
	void Update ()
	{

		TargetAcquisition ();

	}

	// SJ - if the player enters the detection radius, move to the player
	private void TargetAcquisition ()
	{
		// SJ - Calculate distance as float between player and enemy; If it's less than detectionRadius, set Player as as Target
		distanceFromPlayer = Vector3.Distance (player.transform.position, transform.position);
		//Debug.Log ("Distance from player: " + distanceFromPlayer);

		// SJ - chase player
		if (distanceFromPlayer < chaseRadius) {
			//Debug.Log ("Player within range of enemy!");
			// acquire Target!
			aiCharacterControl.SetTarget (player.transform);
		} else {
			// outside range, remove target
			aiCharacterControl.SetTarget (null);
		}

		// SJ - Attack Player
		if (distanceFromPlayer <= attackRadius && !isAttacking) {

			// Look at the player
			transform.LookAt (player.transform);



			// SJ - stop the double fire when going in/out at the attackRadius boundary
			//... if its been more secons than the time between shots, fire away!
			if ((Time.time - timeLastFired) > secondsBetweenShots) {

				isAttacking = true;

				//Debug.Log("Time.time [" + Time.time + "]; timeLastFired [" + timeLastFired + "]; secondsBetweenShots [" + secondsBetweenShots + "]");
				InvokeRepeating ("FireProjectile", 0f, secondsBetweenShots);	// TODO - Change to Coroutine.

			}

		} 

		// if player is outside the attack radius, stop firing
		if (distanceFromPlayer >= attackRadius) {

			// stop the attacking
			isAttacking = false;
			CancelInvoke();

		}


	}

	void FireProjectile ()
	{
		// Look at the player
		transform.LookAt(player.transform);

		// instantiate projectile
		GameObject firedProjectile = Instantiate (projectileToUse, projectileSocket.transform.position, Quaternion.identity) as GameObject;

		// Get the projectile component since we're using it multiple times
		Projectile projectileComponent = firedProjectile.GetComponent<Projectile> ();

		// set the projectile damage
		projectileComponent.SetDamage(damagePerShot);

		// aim the projectile and the attacker
		Vector3 unitVectorToPlayer = (player.transform.position + aimOffset - projectileSocket.transform.position).normalized;

		// Fire it!
		firedProjectile.GetComponent<Rigidbody> ().velocity = unitVectorToPlayer * projectileComponent.projectileSpeed;
		timeLastFired = Time.time;

	}


	public float healthAsPercentage {
		get {
			return currentHealthPoints / maxHealthPoints;
		}
	}

	// SJ - From IDamageable Interface
	public void TakeDamage (float damage)
	{
		// Apply Damage
		currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);

		// Kill enemy (remove from field)
		if (currentHealthPoints <= 0) {
			Destroy(gameObject);
		}
	}


	void OnDrawGizmos()
	{

		// Draw Detection Radius
		Gizmos.color = new Color(0f, 0f, 255f, 0.5f);
		Gizmos.DrawWireSphere(transform.position, chaseRadius);

		// Draw Attack Radius
		Gizmos.color = new Color(255f, 0f, 0f, 0.5f);
		Gizmos.DrawWireSphere(transform.position, attackRadius);


	}

}
