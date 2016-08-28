using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

	public enum PlayerType {Tato=1, Joel=2, Nell=3, Nelson=4, Jesus=5};

	//Variables

	[HideInInspector]
	public bool isFacingLeft = true;		// For determining which way the player is currently facing.
	[HideInInspector]
	public bool jump = false;				// Condition for whether the player should jump.

	public PlayerType playerType;
	public float maxSpeed = 5f;			    // As fast as player can go
	public float moveForce = 365f;			// Amount of force added to move the player left and right.
	public float headbobSpeedMultiplier;	// To be passed to animator.
	public float jumpForce = 1000f;			// Amount of force added when the player jumps.
	public float shotForce;					// Bullet force amount
	public int bulletsInInventory;			// Remaining bullets
	public int killsForBulletRestock;		// Number of enemies the player hs to kill before he gets an bullet restock
	public AudioClip[] jumpSounds;			// Array of jump sounds to be called randomly
	public AudioClip[] meleeAttackSounds;	// Array of attack sounds to be called randomly
	public AudioClip[] rangeAttackSounds;	// Array of attack sounds to be called randomly
	public GameObject bullet;				// Bullet for ranged attack
	public bool isInvulnerable=false;
	public float deathTime;

	[HideInInspector]
	public bool isGrounded = false;         // Bool for checking if player is grounded, uses 
	private Transform groundCheck;			// A position marking where to check if the player is grounded.
	private Transform shotSpawn;			// A position marking where to shoot from.
	private Animator anim;					// Reference to the player's animator component.
	private PlayerSpeechBubble speechBubble;
	private WeaponRanged weaponRanged;		// Reference to ranged weapon script.
	private AudioSource audioSource;		// Audiosource on object.
	private bool isDead=false;				// Turn on so death animations/protocols don't happen more than once.
	private Canvas healthBarCanvas;			// Canvas object containing lifebar UI elements.
	private int killCount;		
	private int startingBulletsInInventory;
	private bool canDoubleJump = false;
	private bool doubleJump = false;
	private float deathTimer = 0;
	private bool hasControl = true;

	// Use this for initialization
	void Start () {
		Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());
	}

	void Awake()
	{
		// Setting up references.
		groundCheck = transform.FindChild("groundCheck");
		shotSpawn = transform.FindChild ("shotSpawn");
		anim = GetComponentInChildren<Animator>();
		anim.SetFloat ("headbobSpeedMultiplier", headbobSpeedMultiplier);
		speechBubble = this.GetComponentsInChildren<PlayerSpeechBubble> ()[0];
		weaponRanged = GetComponentInChildren<WeaponRanged> ();
		audioSource = GetComponent<AudioSource> ();
		healthBarCanvas=GameObject.FindGameObjectWithTag("LifeBar").GetComponent<Canvas>();
		startingBulletsInInventory = bulletsInInventory;
	}

	// Update is called once per frame
	void Update () {

		// The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
		isGrounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));  

		if (hasControl) {
			// If the jump button is pressed and the player is grounded then the player should jump.
			if (Input.GetButtonDown ("Jump"))
			if (!jump && isGrounded)
				jump = true;
			else
				doubleJump = true;

			if (Input.GetButtonDown ("MeleeAttack"))
				MeleeAttack ();

			if (Input.GetButtonDown ("RangedAttack"))
				RangedAttack ();
		}

		if (isDead) {
			deathTimer -= Time.deltaTime;
			if (deathTimer <= 0) {
				Scene scene = SceneManager.GetActiveScene ();
				SceneManager.LoadScene (scene.name);
			}
		}
	
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
		if(h > 0 && isFacingLeft)
			// ... flip the player.
			FlipCharacter();
		// Otherwise if the input is moving the player left and the player is facing right...
		else if(h < 0 && !isFacingLeft)
			// ... flip the player.
			FlipCharacter();

		// Jump if conditions are met, checks happen in update
		if (jump || (doubleJump && canDoubleJump))
			Jump ();

	}

	void OnTriggerEnter2D (Collider2D col){
		if (col.transform.tag == ("DeadZone"))
			Die ();
			
	}

	void FlipCharacter() {

		// Switch the way the player is labelled as facing.
		isFacingLeft = !isFacingLeft;

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
		if (jump)
			canDoubleJump = true;
		else
			canDoubleJump = false;
		
		jump = false;
		doubleJump = false;

		// Play a random jump audio clip.
		int i = Random.Range(0, jumpSounds.Length);
		AudioSource.PlayClipAtPoint(jumpSounds[i], transform.position);

		anim.SetTrigger ("jump");

	}

	void MeleeAttack(){
		isInvulnerable = true;
		anim.SetTrigger ("meleeAttack");
	}

	public void FinishAttacking(){
		isInvulnerable = false;
	}

	void RangedAttack(){
		if (bulletsInInventory > 0) {
			anim.SetTrigger ("rangedAttack");
			weaponRanged.Shoot ();

			// Don't collide player with bullet.
			GameObject tempBullet = Instantiate (bullet, shotSpawn.transform.position, bullet.transform.rotation) as GameObject;
			Physics2D.IgnoreCollision (tempBullet.GetComponent<Collider2D> (), GetComponent<Collider2D> ());

			if (!isFacingLeft)
				tempBullet.transform.Rotate (0, 0, 180);

			Rigidbody2D tempRigidBody = tempBullet.GetComponent<Rigidbody2D> ();

			float directedShotForce;

			if (isFacingLeft)
				directedShotForce = -shotForce;
			else
				directedShotForce = shotForce;

			tempRigidBody.velocity = new Vector2 (directedShotForce, 0);

			// Play random shooting clip.
			int i = Random.Range (0, rangeAttackSounds.Length);
			audioSource.clip = rangeAttackSounds [i];
			if (!audioSource.isPlaying)
				audioSource.Play ();

			bulletsInInventory--;

			healthBarCanvas.GetComponent<LifeBar> ().UpdatePowerBar (bulletsInInventory);
		}
	}

	public void Die(){

		if (!isDead) {
			// Find all of the colliders on the gameobject and set them all to be triggers.
			Collider2D[] cols = GetComponents<Collider2D> ();
			foreach (Collider2D c in cols) {
				c.isTrigger = true;
			}
			/*
			// Move all sprite parts of the player to the front
			SpriteRenderer[] spr = GetComponentsInChildren<SpriteRenderer>();
			foreach(SpriteRenderer s in spr)
			{
				s.sortingLayerName = "UI";
			}
			*/
			// ... disable user Player Control script
			//GetComponent<PlayerController> ().enabled = false;
			hasControl=false;

			// ... Trigger the 'Die' animation state
			anim.SetTrigger ("Dead");

			// trigger die in lifebar
			healthBarCanvas.GetComponent<Animator> ().SetTrigger ("die");

			isDead = true;

			deathTimer = deathTime;
		}
	}

	// Kill type is 0 when melee, 1 when ranged.
	public void EnemyKill(int killType){
		if (killType == 0) {
			// Play a random jump audio clip.
			int i = Random.Range (0, meleeAttackSounds.Length);
			AudioSource.PlayClipAtPoint (meleeAttackSounds [i], transform.position);
		}

		// Restock a single bullet when killCount is a multiple of killsForBulletRestock
		killCount++;
		if (killCount % killsForBulletRestock == 0) {
			bulletsInInventory++;
			if (!(bulletsInInventory > startingBulletsInInventory))
				healthBarCanvas.GetComponent<LifeBar> ().UpdatePowerBar (bulletsInInventory);
			else
				bulletsInInventory = startingBulletsInInventory;
		}
	}
}
