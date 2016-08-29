using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{

	public float moveSpeed = 2f;
	// The speed the enemy moves at.
	public float deathDespawnInterval;
	// Time the character is on the scene after dying.
	public float headbobSpeedMultiplier;
	// To be passed to animator.

	private float deathTimer = 0f;
	// When timer reaches despawn time, destroy object.
	private bool isDead = false;
	// Check for whether enemy is dead, despawn after interval.
	private Animator anim;
	// Reference to the enemy's animator component.
	private Transform frontCheck;
	// Reference to the position of the gameobject used for checking if something is in front.
	private GameController gameController;
	private PlayerController playerController;
	private bool isFacingLeft = true;
	private bool isGrounded = false;

	[HideInInspector]
	public EnemySpawner enemySpawner;

	void Awake ()
	{
		// Setting up references.
		anim = GetComponentInChildren<Animator> ();
		anim.SetFloat ("headbobSpeedMultiplier", headbobSpeedMultiplier);
		frontCheck = transform.Find ("frontCheck").transform;
		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		gameController.numberOfEnemies++;
		playerController = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();
	}

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		// Destroy object when it should despawn.
		if (isDead) {
			deathTimer += Time.deltaTime;
			if (deathTimer >= deathDespawnInterval)
				Destroy (this.gameObject);
		}
	}

	void FixedUpdate ()
	{
		int directionSign = FindPlayer ();

		// Set the enemy's velocity to moveSpeed in the x direction.
		GetComponent<Rigidbody2D> ().velocity = new Vector2 (moveSpeed * directionSign, GetComponent<Rigidbody2D> ().velocity.y);

		Reorient (directionSign);

		// The Speed animator parameter is set.
		anim.SetFloat ("Speed", Mathf.Abs (GetComponent<Rigidbody2D> ().velocity.magnitude));

	}

	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.transform.tag == "DeadZone")
			Die (3);
	}

	void OnCollisionEnter2D (Collision2D col)
	{

		if (col.collider.gameObject.layer == LayerMask.NameToLayer ("Ground"))
			isGrounded = true;

		// Don't collide with enemies until landig
		if (col.collider.transform.tag == "Enemy" && !isGrounded)
			Physics2D.IgnoreCollision (col.collider, GetComponent<Collider2D> ());
	}


	void OnCollisionExit2D (Collision2D col)
	{
		if (col.gameObject.layer == 8
		    && isGrounded) {
			isGrounded = false;
		}
	}

	public void Flip ()
	{
		// Multiply the x component of localScale by -1.
		Vector3 enemyScale = transform.localScale;
		enemyScale.x *= -1;
		transform.localScale = enemyScale;
		isFacingLeft = !isFacingLeft;
	}

	// killType is 0 when melee, 1 when ranged, 3 when suicide
	public void Die (int killType)
	{
		if (!isDead) {

			// Find all of the colliders on the gameobject and set them all to be triggers.
			Collider2D[] cols = GetComponents<Collider2D> ();
			foreach (Collider2D c in cols) {
				c.isTrigger = true;
			}


			// Move all sprite parts of the player to the front
			SpriteRenderer[] spr = GetComponentsInChildren<SpriteRenderer> ();
			foreach (SpriteRenderer s in spr) {
				s.sortingLayerName = "UI";
			}

			// ... Trigger the 'Die' animation state
			anim.SetTrigger ("Dead");

			// Start despawning timer in update.
			isDead = true;

			gameController.numberOfEnemies--;
			if (enemySpawner != null)
				enemySpawner.numberOfEnemies--;

			if (killType != 3)
				playerController.EnemyKill (killType);
		}
	}

	private int FindPlayer ()
	{
		int direction = 0;

		Vector3 tempFloat = transform.position - playerController.transform.position;

		if (tempFloat.x <= 0)
			direction = 1;
		else
			direction = -1;

		return direction;
	}

	private void Reorient (int direction)
	{
		if (direction == -1 && !isFacingLeft)
			Flip ();
		else if (direction == 1 && isFacingLeft)
			Flip ();
	}
}
