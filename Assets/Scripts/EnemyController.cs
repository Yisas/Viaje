using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

	public float moveSpeed = 2f;		// The speed the enemy moves at.

	private bool dead = false;			// Whether or not the enemy is dead.
	private Animator anim;				// Reference to the enemy's animator component.
	private Transform frontCheck;		// Reference to the position of the gameobject used for checking if something is in front.

	void Awake() {

		// Setting up references.
		anim = GetComponentInChildren<Animator>();
		frontCheck = transform.Find("frontCheck").transform;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate() {

		// Create an array of all the colliders in front of the enemy.
		Collider2D[] frontHits = Physics2D.OverlapPointAll(frontCheck.position, 1);

		// Check each of the colliders.
		foreach(Collider2D c in frontHits)
		{
			// If any of the colliders is an Obstacle...
			if(c.tag == "Obstacle")
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
}
