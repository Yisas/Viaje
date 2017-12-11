using UnityEngine;
using System.Collections;

public class RudessHead : MonoBehaviour {

	public float moveForce;
	public float maxSpeed;
	public Object bolt;
	public float shootTime;
	public float shotSprayInterval;
	public float transformInterval;
	public GameObject rudessOnFoot;
    public RudessOnFoot rudessOnFootScript;
	public GameObject teleportEffect;
	public float teleportDelay;

	private Vector3 leftWaypoint;
	private Vector3 rightWaypoint;
	private Transform leftKeyboardSpawnPoint;
	private Transform rightKeyboardSpawnPoint;
	private PlayerController playerController;
	private int playerDirection;									// Relative direction towards the players. -1 for left.
	private float shootTimer;
	private float shotSprayTimer;
	private bool isShooting = false;
	private GameObject[] spawnPoints;
	private float transformTimer;
	[HideInInspector]
	public bool transformed = false;
	private float teleportTimer;
	[HideInInspector]
	public bool isTeleporting = false;
	
	public bool isDead = false;
	private int randomSpawnPoint = 0;

	void Awake(){
		playerController = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController>();
		leftWaypoint = transform.Find ("leftWaypoint").position;
		rightWaypoint = transform.Find ("rightWaypoint").position;
		leftKeyboardSpawnPoint = transform.Find ("leftKeyboardSpawnPoint").transform;
		rightKeyboardSpawnPoint = transform.Find ("rightKeyboardSpawnPoint").transform;
		shootTimer = shootTime;
		shotSprayTimer = shotSprayInterval;
		spawnPoints = GameObject.FindGameObjectsWithTag ("RudessSpawner");
		transformTimer = transformInterval;
		teleportTimer = teleportDelay;
		teleportEffect.GetComponent<ParticleSystem>().startLifetime = teleportDelay;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (isDead || rudessOnFootScript.isDead)
        {
            Destroy(gameObject);
            return;
        }

		if(!isTeleporting && !transformed)
			transformTimer -= Time.deltaTime;

		if(transformTimer <= 0 && !transformed) {
			transformTimer = transformInterval;
			isTeleporting = true;
			ChooseSpawnPosition ();
			InstantiatePortals (false);
		}

		if (isTeleporting) {
			teleportTimer -= Time.deltaTime;
			if (teleportTimer <= 0) {
				teleportTimer = teleportDelay;
				SpawnRudessOnFoot ();
			}
		}

		playerDirection = FindPlayer ();

		if (isShooting) {
			shotSprayTimer -= Time.deltaTime;
			if (shotSprayTimer <= 0) {
				shotSprayTimer = shotSprayInterval;
				ShootBolt ();
				isShooting = false;
			}
		} else {
			shootTime -= Time.deltaTime;
			if (shootTime <= 0) {
				isShooting = true;
				shootTime = shootTimer;
				ShootBolt ();
			}
		}
	}

	void FixedUpdate(){
		if (playerDirection != 0 && !isTeleporting) {
			GetComponent<Rigidbody2D>().AddForce(Vector2.right * playerDirection * moveForce);

			// If the player's horizontal velocity is greater than the maxSpeed...
			if(Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > maxSpeed)
				// ... set the player's velocity to the maxSpeed in the x axis.
				GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x) * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);

			BorderCheck ();
		}
	}

	// Uses horizontal position diference to return the sign of the offset to the player.
	int FindPlayer ()
	{
		int direction = 0;

		Vector3 tempFloat = transform.position - playerController.transform.position;

		if (tempFloat.x <= 0)
			direction = 1;
		else
			direction = -1;

		return direction;
	}

	// Cancel velocity if position if out of bounds.
	void BorderCheck(){
		if (transform.position.x <= leftWaypoint.x || transform.position.x >= rightWaypoint.x) {
			Rigidbody2D rigidbody2D = GetComponent<Rigidbody2D> ();
			Vector2 tempVector = new Vector2(0, rigidbody2D.velocity.y);
			GetComponent<Rigidbody2D> ().velocity = tempVector;
		}
	}

	void ShootBolt(){
		Vector3 spawnPoint = new Vector3(Random.Range(leftKeyboardSpawnPoint.position.x,rightKeyboardSpawnPoint.position.x),leftKeyboardSpawnPoint.position.y,leftKeyboardSpawnPoint.position.z);
		Instantiate(bolt,spawnPoint, new Quaternion(0,0,0,1));
	}

	void SpawnRudessOnFoot(){
		rudessOnFoot.SetActive (true);
		rudessOnFoot.transform.position = spawnPoints [randomSpawnPoint].transform.position;
		rudessOnFoot.transform.localScale = spawnPoints [randomSpawnPoint].transform.localScale;
		transformed = true;
		rudessOnFoot.GetComponent<RudessOnFoot>().transformed = false;
		isTeleporting = false;
		gameObject.SetActive (false);
	}

	void ChooseSpawnPosition(){
		randomSpawnPoint = Random.Range (0, spawnPoints.Length);
		/*
		int randomSpawnPoint;

		if (tempFloat <= 0.5f)
			randomSpawnPoint = 0;
		else if (tempFloat > 0.5f && tempFloat <= 1f)
			randomSpawnPoint = 1;
		else
			randomSpawnPoint = (int)tempFloat;
			*/
		
	}

	public void InstantiatePortals(bool reappear){
		// Cancel horizontal velocity
		GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);

		GameObject headEffect = (GameObject)Instantiate (teleportEffect, transform.position, transform.rotation);
		headEffect.GetComponent<ParticleSystem> ().Play ();
		GameObject onFootEfect = (GameObject)Instantiate (teleportEffect, spawnPoints [randomSpawnPoint].transform.position, spawnPoints [randomSpawnPoint].transform.rotation);
		onFootEfect.GetComponent<ParticleSystem> ().startSize = rudessOnFoot.GetComponent<RudessOnFoot> ().portalScale;
		onFootEfect.GetComponent<ParticleSystem> ().Play ();

		if (!reappear) {
			GetComponentInChildren<Animator> ().SetTrigger ("teleport");
			rudessOnFoot.GetComponentInChildren<Animator> ().SetTrigger ("reappear");
		} else {
			GetComponentInChildren<Animator> ().SetTrigger ("reappear");
			rudessOnFoot.GetComponentInChildren<Animator> ().SetTrigger ("teleport");
		}
			
	}

	public void RescaleSprites(){
		transform.Find ("sprites").localScale = new Vector3 (1f, 1f, 1f);
	}
}
