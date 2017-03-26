using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.AI;	// SJ - required to get access to Nav Mesh Agent component


public class Enemy : MonoBehaviour, IDamageable {

	[SerializeField] float maxHealthPoints = 100f;
	[SerializeField] float detectionRadius = 20f;
	[SerializeField] float attackRadius = 10f;

	// Projectile Info
	[SerializeField] float damagePerShot = 7f;
	[SerializeField] GameObject projectileToUse;
	[SerializeField] GameObject projectileSocket;

	private float currentHealthPoints = 100f;

	// SJ - Get all the component access for the enemy detecting player bit
	private ThirdPersonCharacter thirdPersonCharacter = null;
	private AICharacterControl aiCharacterControl;
	private NavMeshAgent navMeshAgent;
	private Player player;

	// SJ - Calculate how far away the player is
	private float distanceFromPlayer;



	// Use this for initialization
	void Start () {

		// SJ - Gain access to all the components on the Enemy
		thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
		aiCharacterControl = GetComponent<AICharacterControl>();
		navMeshAgent = GetComponent<NavMeshAgent>();
		player = GameObject.FindObjectOfType<Player>();
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
		if (distanceFromPlayer < detectionRadius) {
			//Debug.Log ("Player within range of enemy!");
			// acquire Target!
			aiCharacterControl.SetTarget (player.transform);
		} else {
			// outside range, remove target
			aiCharacterControl.SetTarget (null);
		}

		// SJ - Attack Player
		if (distanceFromPlayer <= attackRadius) {

			FireProjectile ();

		}


	}

	void FireProjectile ()
	{
		// instantiate projectile
		GameObject projectile = Instantiate (projectileToUse, projectileSocket.transform.position, Quaternion.identity, projectileSocket.transform) as GameObject;

		// Get the projectile component since we're using it multiple times
		Projectile projectileComponent = projectile.GetComponent<Projectile> ();

		// set the projectile damage
		projectileComponent.damageCaused = damagePerShot;

		// aim the projectile
		Vector3 unitVectorToPlayer = (player.transform.position - projectileSocket.transform.position).normalized;

		// Fire it!
		projectile.GetComponent<Rigidbody> ().velocity = unitVectorToPlayer * projectileComponent.projectileSpeed;

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

	}


	void OnDrawGizmos()
	{

		// Draw Detection Radius
		Gizmos.color = new Color(0f, 0f, 255f, 0.5f);
		Gizmos.DrawWireSphere(transform.position, detectionRadius);

		// Draw Attack Radius
		Gizmos.color = new Color(255f, 0f, 0f, 0.5f);
		Gizmos.DrawWireSphere(transform.position, attackRadius);


	}

}
