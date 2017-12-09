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
	public float moveForce;					// Amount of force added to move the player left and right.
	public float maxSpeed;				    // As fast as player can go
	public float headbobSpeedMultiplier;	// To be passed to animator.
	public float jumpForce;					// Amount of force added when the player jumps.
	public float rangedAttackSpeedMultiplier;	// For the animator.
	public float shotForce;					// Bullet force amount
	public int bulletsInInventory;			// Remaining bullets
	public int killsForBulletRestock;		// Number of enemies the player hs to kill before he gets an bullet restock
	public AudioClip[] jumpSounds;			// Array of jump sounds to be called randomly
	public AudioClip[] meleeAttackSounds;	// Array of attack sounds to be called randomly
	public AudioClip[] rangeAttackSounds;	// Array of attack sounds to be called randomly
	public GameObject bullet;				// Bullet for ranged attack
    public bool rangedParticleSystem =false;// Whether the thrown bullet is a particle system
	public bool isInvulnerable=false;
	public float deathTime;
    public GameObject mainMenu;

	// Underwater specific values
	public float underWaterGravityScale;
	public float underWaterHorizontalMoveForce;
	public float underWaterHorizontalMaxSpeed;
	public float underWaterVerticalMoveForce;
	public float underWaterVerticalMaxSpeed;

	[HideInInspector]
	public bool isGrounded = false;         // Bool for checking if player is grounded, uses 
	private Transform groundCheck;			// A position marking where to check if the player is grounded.
	protected Transform shotSpawn;			// A position marking where to shoot from.
	protected Animator anim;				// Reference to the player's animator component.
    protected Rigidbody2D rb;
	private PlayerSpeechBubble speechBubble;
	protected AudioSource audioSource;		// Audiosource on object.
	private bool isDead=false;				// Turn on so death animations/protocols don't happen more than once.
	protected Canvas healthBarCanvas;		// Canvas object containing lifebar UI elements.
	private int killCount;		
	private int startingBulletsInInventory;
	private bool canDoubleJump = false;
	private bool doubleJump = false;
	private float deathTimer = 0;
    [HideInInspector]
	public bool hasControl = true;
	private float defaultGravityScale;
	private bool isUnderWater = false;

	// Use this for initialization
	void Start () {
		Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());
	}

	void Awake()
	{
		// Setting up references.
		groundCheck = transform.Find("groundCheck");
		shotSpawn = transform.Find ("shotSpawn");
		anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
		anim.SetFloat ("headbobSpeedMultiplier", headbobSpeedMultiplier);
		speechBubble = this.GetComponentsInChildren<PlayerSpeechBubble> ()[0];
		audioSource = GetComponent<AudioSource> ();
		healthBarCanvas=GameObject.FindGameObjectWithTag("LifeBar").GetComponent<Canvas>();
		startingBulletsInInventory = bulletsInInventory;
		defaultGravityScale = GetComponent<Rigidbody2D> ().gravityScale;
	}

	// Update is called once per frame
	void Update () {

		// The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
		isGrounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));  

		if (hasControl && !isUnderWater) {
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

            if (Input.GetButtonDown("Menu"))
            {
                mainMenu.GetComponent<Menu>().Open();
            }
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
		if (hasControl) {
			if (!isUnderWater) {
				MovementOnFoot ();

				// Jump if conditions are met, checks happen in update
				if (jump || (doubleJump && canDoubleJump))
					Jump ();
			} else
				MovementUnderWater ();
		}
	}

	void OnTriggerEnter2D (Collider2D col){
		if (col.transform.tag == ("DeadZone"))
			Die ();

		if (col.gameObject.layer == LayerMask.NameToLayer ("SwimmableWater")) {
			GoUnderwater ();
		}
	}

	void OnTriggerExit2D(Collider2D col){

		if (col.gameObject.layer == LayerMask.NameToLayer ("SwimmableWater")) {
			if (Mathf.Sign (GetComponent<Rigidbody2D> ().velocity.y) > 0)
				GetComponent<Rigidbody2D> ().velocity = new Vector2 (GetComponent<Rigidbody2D> ().velocity.x, 0f);
		
			GetComponent<Rigidbody2D> ().gravityScale = defaultGravityScale;
		}
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

		Vector3 priorVelocity = GetComponent<Rigidbody2D> ().velocity;

		// Cancel vertical velocity before jumping
		GetComponent<Rigidbody2D>().velocity = new Vector3(priorVelocity.x, 0 ,priorVelocity.z);

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

	// Method uses animator to call next step of shooting
 void RangedAttack(){
		if (bulletsInInventory > 0) {
			// Play random shooting clip.
			int i = Random.Range (0, rangeAttackSounds.Length);
			audioSource.clip = rangeAttackSounds [i];
			if (!audioSource.isPlaying)
				audioSource.Play ();
            
			anim.SetFloat ("rangedAttackSpeedMultiplier", rangedAttackSpeedMultiplier);
			anim.SetTrigger ("rangedAttack");
		}
	}


	public void ShootBullet(){
		// Don't collide player with bullet.
		GameObject tempBullet = Instantiate (bullet, shotSpawn.transform.position, bullet.transform.rotation) as GameObject;

        // If the shot is not a particle system...
        if (!rangedParticleSystem)
        {

            if (!isFacingLeft)
                tempBullet.transform.Rotate(0, 0, 180);

            Physics2D.IgnoreCollision(tempBullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            // ... directed shot uset rigid body of the bullet
            Rigidbody2D tempRigidBody = tempBullet.GetComponent<Rigidbody2D>();

            float directedShotForce;

            if (isFacingLeft)
                directedShotForce = -shotForce;
            else
                directedShotForce = shotForce;

            tempRigidBody.velocity = new Vector2(directedShotForce, 0);
        }
        else
        {
            // ... else instantiate and play particle system

            if (!isFacingLeft)
            {
                tempBullet.transform.localScale = new Vector3(-tempBullet.transform.localScale.x, tempBullet.transform.localScale.y, tempBullet.transform.localScale.z);
                tempBullet.transform.Rotate(Vector3.up, 180);
                //tempBullet.transform.rotation = new Quaternion(tempBullet.transform.rotation.x, -tempBullet.transform.rotation.y, tempBullet.transform.rotation.z, tempBullet.transform.rotation.w);
            }

            tempBullet.GetComponent<ParticleSystem>().Play();
        }

		bulletsInInventory--;

		healthBarCanvas.GetComponent<LifeBar> ().UpdatePowerBar (bulletsInInventory);

		healthBarCanvas.GetComponent<Animator> ().SetTrigger ("rangedAttack");
	}

	public void Die(){

		if (!isDead) {
			// Find all of the colliders on the gameobject and set them all to be triggers.
			Collider2D[] cols = GetComponents<Collider2D> ();
			foreach (Collider2D c in cols) {
				c.isTrigger = true;
			}

			// Disable control
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

	private void GoUnderwater(){
        // Alert flags
        rb.velocity = new Vector2(0, 0);
        anim.SetFloat("Speed", 0);

		isUnderWater = true;
		anim.SetBool ("underWater", true);

		// Invert gravity so player swims up
		GetComponent<Rigidbody2D>().gravityScale = underWaterGravityScale;

        if (!hasControl)
            hasControl = !hasControl;
    }

	public void ExitWater(){
		anim.SetBool ("underWater", false);
		isUnderWater = false;
		GetComponent<Rigidbody2D> ().gravityScale = defaultGravityScale;
	}

	private void MovementOnFoot(){
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

		// Check if character needs to flip
		CheckDirection (h);
	}

	private void MovementUnderWater(){
		// Cache the horizontal input.
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis ("Vertical");

		// The Speed animator parameter is set to the absolute value of the horizontal input.
		anim.SetFloat("Speed", Mathf.Abs(h) + Mathf.Abs(v));

		// If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
		if(h * GetComponent<Rigidbody2D>().velocity.x < underWaterHorizontalMaxSpeed)
			// ... add a force to the player.
			GetComponent<Rigidbody2D>().AddForce(Vector2.right * h * underWaterHorizontalMoveForce);

		// If the player's horizontal velocity is greater than the maxSpeed...
		if(Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > underWaterHorizontalMaxSpeed)
			// ... set the player's velocity to the maxSpeed in the x axis.
			GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x) * underWaterHorizontalMaxSpeed, GetComponent<Rigidbody2D>().velocity.y);

		// If the player is changing direction (v has a different sign to velocity.y) or hasn't reached maxSpeed yet...
		if(v * GetComponent<Rigidbody2D>().velocity.y < underWaterVerticalMaxSpeed)
			// ... add a force to the player.
			GetComponent<Rigidbody2D>().AddForce(Vector2.up * v * underWaterVerticalMoveForce);

		// If the player's vertical velocity is greater than the maxSpeed...
		if(Mathf.Abs(GetComponent<Rigidbody2D>().velocity.y) > underWaterVerticalMaxSpeed)
			// ... set the player's velocity to the maxSpeed in the y axis.
			GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x , Mathf.Sign(GetComponent<Rigidbody2D>().velocity.y) * underWaterVerticalMaxSpeed);

		// Check if character needs to flip
		CheckDirection (h);
	}

	public void CheckDirection(float vel){
		// If the input is moving the player right and the player is facing left...
		if(vel > 0 && isFacingLeft)
			// ... flip the player.
			FlipCharacter();
		// Otherwise if the input is moving the player left and the player is facing right...
		else if(vel < 0 && !isFacingLeft)
			// ... flip the player.
			FlipCharacter();
	}

	// Used by animation events
	public void HideCharacter(){
		SpriteRenderer[] sprts = GetComponentsInChildren<SpriteRenderer> ();

		foreach (SpriteRenderer spr in sprts)
			spr.enabled = false;

		GetComponentInChildren<Animator> ().enabled = false;
	}

    public void SetHealthbarCanvas(Canvas newHealthbar)
    {
        healthBarCanvas = newHealthbar;
    }
}
