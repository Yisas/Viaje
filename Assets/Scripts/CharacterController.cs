using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour {

	//Variables

	[HideInInspector]
	public bool facingRight = false;		// For determining which way the player is currently facing.
	[HideInInspector]
	public bool jump = false;				// Condition for whether the player should jump.

	public float speed;						// Player speed
	public float moveForce = 365f;			// Amount of force added to move the player left and right.

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// Called each physics step
	void FixedUpdate(){
		
		// Cache the horizontal input.
		float h = Input.GetAxis("Horizontal");

		// Move
		if(h * GetComponent<Rigidbody2D>().velocity.x < 0)
			// ... add a force to the player.
			GetComponent<Rigidbody2D>().AddForce(Vector2.right * h * moveForce);


		// If the input is moving the player right and the player is facing left...
		if(h > 0 && !facingRight)
			// ... flip the player.
			FlipCharacter();
		// Otherwise if the input is moving the player left and the player is facing right...
		else if(h < 0 && facingRight)
			// ... flip the player.
			FlipCharacter();
	}

	void FlipCharacter() {

		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;

		// Multiply the player's x local scale by -1.
		Vector3 tempScale = transform.localScale;
		tempScale.x *= -1;
		transform.localScale = tempScale;
	}
}
