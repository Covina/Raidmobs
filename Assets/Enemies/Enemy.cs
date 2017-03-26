﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.AI;	// SJ - required to get access to Nav Mesh Agent component


public class Enemy : MonoBehaviour, IDamageable {

	[SerializeField] private float maxHealthPoints = 100f;
	[SerializeField] private float playerDetectionRadius = 20f;
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

		FindIfPlayerInRange ();



	}

	private void FindIfPlayerInRange ()
	{
		// SJ - Calculate distance as float between player and enemy; If it's less than detectionRadius, set Player as as Target
		distanceFromPlayer = Vector3.Distance (player.transform.position, transform.position);
		//Debug.Log ("Distance from player: " + distanceFromPlayer);
		// SJ - Are we within range?
		if (distanceFromPlayer < playerDetectionRadius) {
			//Debug.Log ("Player within range of enemy!");
			// acquire Target!
			aiCharacterControl.SetTarget (player.transform);
		}
		else {
			// outside range, remove target
			aiCharacterControl.SetTarget (null);
		}
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

}
