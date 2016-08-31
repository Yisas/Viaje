using UnityEngine;
using System.Collections;

public class RudessHead : MonoBehaviour {

	public float moveForce;
	public float maxSpeed;
	public Object bolt;
	public float shootTime;
	public float shotSprayInterval;

	private Vector3 leftWaypoint;
	private Vector3 rightWaypoint;
	private Transform leftKeyboardSpawnPoint;
	private Transform rightKeyboardSpawnPoint;
	private PlayerController playerController;
	private int playerDirection;									// Relative direction towards the players. -1 for left.
	private float shootTimer;
	private float shotSprayTimer;
	private bool isShooting = false;

	void Awake(){
		playerController = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController>();
		leftWaypoint = transform.FindChild ("leftWaypoint").position;
		rightWaypoint = transform.FindChild ("rightWaypoint").position;
		leftKeyboardSpawnPoint = transform.FindChild ("leftKeyboardSpawnPoint").transform;
		rightKeyboardSpawnPoint = transform.FindChild ("rightKeyboardSpawnPoint").transform;
		shootTimer = shootTime;
		shotSprayTimer = shotSprayInterval;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
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
		if (playerDirection != 0) {
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
}
