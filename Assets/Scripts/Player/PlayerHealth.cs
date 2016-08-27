using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour {

	public float health = 100f;					// The player's health.
	public float repeatDamagePeriod = 0.5f;		// How frequently the player can be damaged.
	public float hurtForceGrounded ;			// The force with which the player is pushed when hurt.
	public float hurtForceAirborne;				// The force with which the player is pushed when hurt.
	public float damageAmount = 10f;			// The amount of damage to take when enemies touch the player
	public float lifePowerupDefault;			// Default amount of life the player is healed when picking up health.

	private float lastHitTime;					// The time at which the player was last hit.
	private PlayerController playerControl;		// Reference to the PlayerControl script.
	private Animator anim;						// Reference to the Animator on the player
	private LifeBar lifeBar;
	private float startingHealth;

	void Awake() {
		// Setting up references.
		playerControl = GetComponent<PlayerController>();
		anim = GetComponentInChildren<Animator>();
		lifeBar = GameObject.FindGameObjectWithTag ("LifeBar").GetComponent<LifeBar> ();
		startingHealth = health;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnCollisionEnter2D (Collision2D col){
		// If the colliding gameobject is an Enemy...
		if(col.gameObject.tag == "Enemy")
		{
			// ... and if the time exceeds the time of the last hit plus the time between hits...
			if (Time.time > lastHitTime + repeatDamagePeriod) 
			{
				// ... and if the player still has health...
				if (health > 0f) {
					// ... take damage and reset the lastHitTime.
					TakeDamage (col.transform); 
					lastHitTime = Time.time; 
				}
				// If the player doesn't have health, die.
				else
					playerControl.Die ();
			}
		}
	}

	void TakeDamage (Transform enemy)
	{
		if (!playerControl.isInvulnerable) {

			anim.SetTrigger ("angryHeadbob");

			// Make sure the player can't jump.
			playerControl.jump = false;

			// Create a vector that's from the enemy to the player with an upwards boost.
			Vector3 hurtVector = transform.position - enemy.position + Vector3.up * 5f;

			if(playerControl.isGrounded==true)
				// Add a force to the player in the direction of the vector and multiply by the hurtForce.
				GetComponent<Rigidbody2D> ().AddForce (hurtVector * hurtForceGrounded);
			else
				GetComponent<Rigidbody2D> ().AddForce (hurtVector * hurtForceAirborne);

			// Reduce the player's health by 10.
			health -= damageAmount;

			lifeBar.UpdateHealthBar (health,true);
		}
	}

	public void Heal(){
		health += lifePowerupDefault;
		if(health>startingHealth)
			health=startingHealth;
		lifeBar.UpdateHealthBar (health,false);
	}
}
