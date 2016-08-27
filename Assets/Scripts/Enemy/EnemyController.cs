using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

	public float moveSpeed = 2f;		// The speed the enemy moves at.
	public float deathDespawnInterval;	// Time the character is on the scene after dying.

	private float deathTimer = 0f;		// When timer reaches despawn time, destroy object.
	private bool isDead=false;			// Check for whether enemy is dead, despawn after interval.
	private Animator anim;				// Reference to the enemy's animator component.
	private Transform frontCheck;		// Reference to the position of the gameobject used for checking if something is in front.
	private GameController gameController;
	private PlayerController playerController;

	[HideInInspector]
	public EnemySpawner enemySpawner;

	void Awake() {
		// Setting up references.
		anim = GetComponentInChildren<Animator>();
		frontCheck = transform.Find("frontCheck").transform;
		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController>();
		gameController.numberOfEnemies++;
		playerController = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

		// Destroy object when it should despawn.
		if (isDead) {
			deathTimer += Time.deltaTime;
			if (deathTimer >= deathDespawnInterval)
				Destroy (this.gameObject);
		}
	}

	void FixedUpdate() {

		// Create an array of all the colliders in front of the enemy.
		Collider2D[] frontHits = Physics2D.OverlapPointAll(frontCheck.position, 1);

		// Check each of the colliders.
		foreach(Collider2D c in frontHits)
		{
			// If any of the colliders is an Obstacle...
			if(c.tag == "Obstacle" || c.tag=="Enemy")
			{
				// ... Flip the enemy and stop checking the other colliders.
				Flip ();
				break;
			}
		}

		// Set the enemy's velocity to moveSpeed in the x direction.
		GetComponent<Rigidbody2D>().velocity = new Vector2((-transform.localScale.x) * moveSpeed, GetComponent<Rigidbody2D>().velocity.y);	

		// The Speed animator parameter is set.
		anim.SetFloat("Speed", Mathf.Abs(GetComponent<Rigidbody2D>().velocity.magnitude));

	}

	public void Flip()
	{
		// Multiply the x component of localScale by -1.
		Vector3 enemyScale = transform.localScale;
		enemyScale.x *= -1;
		transform.localScale = enemyScale;
	}

	// killType is 0 when melee, 1 when ranged.
	public void Die(int killType){
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

			playerController.EnemyKill (killType);
		}
	}
}
