using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{

	public float health = 100f;
	// The player's health.
	public float repeatDamagePeriod = 0.5f;
	// How frequently the player can be damaged.
	public float hurtForceGrounded;
	// The force with which the player is pushed when hurt.
	public float hurtForceAirborne;
	// The force with which the player is pushed when hurt.
	public float damageAmount = 10f;
	// The amount of damage to take when enemies touch the player
	public float lifePowerupDefault;
	// Default amount of life the player is healed when picking up health.

	private float lastHitTime;
	// The time at which the player was last hit.
	private PlayerController playerControl;
	// Reference to the PlayerControl script.
	private Animator anim;
	// Reference to the Animator on the player
	private LifeBar lifeBar;
	private float startingHealth;
	private bool isHurt = false;					// Whether the player should be displaying the hurt face

	void Awake ()
	{
		// Setting up references.
		playerControl = GetComponent<PlayerController> ();
		anim = GetComponentInChildren<Animator> ();
		lifeBar = GameObject.FindGameObjectWithTag ("LifeBar").GetComponent<LifeBar> ();
		startingHealth = health;
	}

	void OnCollisionEnter2D (Collision2D col)
	{
		// If the colliding gameobject is an Enemy...
		if (col.gameObject.tag == "Enemy") {
			// ... and if the time exceeds the time of the last hit plus the time between hits...
			if (Time.time > lastHitTime + repeatDamagePeriod) {
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
    
	void OnCollisionStay2D (Collision2D col)
	{
		if (col.gameObject.tag == "Enemy" || col.gameObject.tag == "EnemyUntracked") {
			if (Time.time - lastHitTime >= repeatDamagePeriod)
					// ... and if the player still has health...
				if (health > 0f) {
				// ... take damage and reset the lastHitTime.
				TakeDamage (col.transform); 
			}
				// If the player doesn't have health, die.
				else
				playerControl.Die ();
		}
	}

	public void TakeDamage (Transform enemy)
	{

		AuxFunctions.HitDirection hitDirection = AuxFunctions.ReturnDirection (enemy, transform);

		if (!playerControl.isInvulnerable) {

			anim.SetTrigger ("angryHeadbob");

			// Make sure the player can't jump.
			playerControl.jump = false;

			// Create a vector that's from the enemy to the player with an upwards boost.
			Vector3 hurtVectorVertical = transform.position - enemy.position + Vector3.up * 5f;

			Vector3 hurtVectorHorizontal;
			if (hitDirection == AuxFunctions.HitDirection.Left)
				hurtVectorHorizontal = transform.position - enemy.position + Vector3.right * 5f;
			else
				hurtVectorHorizontal = transform.position - enemy.position + Vector3.left * 5f;
			
			// Cancel prior velocities
			GetComponent<Rigidbody2D> ().velocity = new Vector3 (0, 0, 0);

			if (playerControl.isGrounded == true) {				// Add a force to the player in the direction of the vector and multiply by the hurtForce.
				GetComponent<Rigidbody2D> ().AddForce (hurtVectorVertical * hurtForceGrounded);
				GetComponent<Rigidbody2D> ().AddForce (hurtVectorHorizontal * hurtForceGrounded);
			} else {
				GetComponent<Rigidbody2D> ().AddForce (hurtVectorVertical * hurtForceAirborne);
				GetComponent<Rigidbody2D> ().AddForce (hurtVectorHorizontal * hurtForceAirborne);
			}

			// Reduce the player's health by 10.
			health -= damageAmount;

			if (health <= startingHealth / 2 && !isHurt) {
				isHurt = true;
                foreach(SpriteSwitch ss in GetComponents<SpriteSwitch>())
				    ss.Switch ();
			}

			lifeBar.UpdateHealthBar (health, true, isHurt);
		}
	}

	public void Heal ()
	{
		health += lifePowerupDefault;
		if (health > startingHealth)
			health = startingHealth;

		if (health > startingHealth/2) {
			isHurt = false;
            foreach (SpriteSwitch ss in GetComponents<SpriteSwitch>())
                ss.SwitchBack();
        }
		
		lifeBar.UpdateHealthBar (health, false, isHurt);
	}
}
