using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public enum PlayerType {Tato=1, Joel=2, Nell=3, Nelson=4, Jesus=5};

	//Variables

	[HideInInspector]
	public bool facingRight = false;		// For determining which way the player is currently facing.
	[HideInInspector]
	public bool jump = false;				// Condition for whether the player should jump.

	public PlayerType playerType;
	public float maxSpeed = 5f;			    // As fast as player can go
	public float moveForce = 365f;			// Amount of force added to move the player left and right.
	public float jumpForce = 1000f;			// Amount of force added when the player jumps.
	public AudioClip[] jumpSounds;			// Array of jump sounds to be called randomly
	public AudioClip[] meleeAttackSounds;	// Array of attack sounds to be called randomly

	private bool isGrounded = false;        // Bool for checking if player is grounded, uses 
	private Transform groundCheck;			// A position marking where to check if the player is grounded.
	private Animator anim;					// Reference to the player's animator component.
	private PlayerSpeechBubble speechBubble;


	// Use this for initialization
	void Start () {
	
	}

	void Awake()
	{
		// Setting up references.
		groundCheck = transform.FindChild("groundCheck");
		anim = GetComponentsInChildren<Animator>()[0];
		speechBubble = this.GetComponentsInChildren<PlayerSpeechBubble> ()[0];
	}

	// Update is called once per frame
	void Update () {

		// The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
		isGrounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));  

		// If the jump button is pressed and the player is grounded then the player should jump.
		if(Input.GetButtonDown("Jump") && isGrounded)
			jump = true;

		if (Input.GetButtonDown ("MeleeAttack"))
			MeleeAttack ();
	
	}

	// Called each physics step
	void FixedUpdate(){
		
		// Cache the horizontal input.
		float h = Input.GetAxis("Horizontal");

		// The Speed animator parameter is set to the absolute value of the horizontal input.
		anim.SetFloat("Speed", Mathf.Abs(h));

		// If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
		if(h * GetComponent<Rigidbody2D>().velocity.x < maxSpeed)
			// ... add a force to the player.
			GetComponent<Rigidbody2D>().AddForce(Vector2.right * h * moveForce);

		// If the player's horizontal velocity is greater than the maxSpeed...
		if(Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > maxSpeed)
			// ... set the player's velocity to the maxSpeed in the x axis.
			GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x) * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);

		// If the input is moving the player right and the player is facing left...
		if(h > 0 && !facingRight)
			// ... flip the player.
			FlipCharacter();
		// Otherwise if the input is moving the player left and the player is facing right...
		else if(h < 0 && facingRight)
			// ... flip the player.
			FlipCharacter();

		// Jump if conditions are met, checks happen in update
		if (jump)
			Jump ();

	}

	void FlipCharacter() {

		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;

		// Multiply the player's x local scale by -1.
		Vector3 tempScale = transform.localScale;
		tempScale.x *= -1;
		transform.localScale = tempScale;

		// Fix text orientation in speech bubble.
		speechBubble.ReRotate ();
	}

	void Jump(){
		
		// Add a vertical force to the player.
		GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));

		// Make sure the player can't jump again until the jump conditions from Update are satisfied.
		jump = false;

		// Play a random jump audio clip.
		int i = Random.Range(0, jumpSounds.Length);
		AudioSource.PlayClipAtPoint(jumpSounds[i], transform.position);

	}

	void MeleeAttack(){
		anim.SetTrigger ("meleeAttack");
	}

	//Called from gun on succesful hit
	public void MeleeHit(){
		// Play a random jump audio clip.
		int i = Random.Range(0, meleeAttackSounds.Length);
		AudioSource.PlayClipAtPoint(meleeAttackSounds[i], transform.position);
	}
}
