using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour {

	public float health = 100f;					// The player's health.
	public float repeatDamagePeriod = 2f;		// How frequently the player can be damaged.
	public float hurtForce = 10f;				// The force with which the player is pushed when hurt.
	public float damageAmount = 10f;			// The amount of damage to take when enemies touch the player
	public float lifePowerupDefault;			// Default amount of life the player is healed when picking up health.

	private float lastHitTime;					// The time at which the player was last hit.
	private PlayerController playerControl;		// Reference to the PlayerControl script.
	private Animator anim;						// Reference to the Animator on the player
	private Vector3 healthScale;				// The local scale of the health bar initially (with full health).
	private Canvas healthBarCanvas;				// Canvas object containing lifebar UI elements.
	private SpriteRenderer healthBarSprite;		// Health bar sprite.
	private SpriteRenderer healthBarDecor;		// Extra healthbar sprite that needs color changing.


	void Awake() {
		// Setting up references.
		playerControl = GetComponent<PlayerController>();
		anim = GetComponentInChildren<Animator>();

		// Getting the intial scale of the healthbar (whilst the player has full health).
		healthBarCanvas=GameObject.FindGameObjectWithTag("LifeBar").GetComponent<Canvas>();
		healthBarSprite = healthBarCanvas.transform.FindChild ("lifeBar").GetComponent<SpriteRenderer>();
		healthScale = healthBarSprite.transform.localScale;
		healthBarDecor = healthBarCanvas.transform.FindChild ("head_space_decor").GetComponent<SpriteRenderer>();

		// Set lifebar colors to healthy
		healthBarSprite.material.color= Color.green;
		healthBarDecor.material.color = Color.green;
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

		anim.SetTrigger ("angryHeadbob");

		// Make sure the player can't jump.
		playerControl.jump = false;

		// Create a vector that's from the enemy to the player with an upwards boost.
		Vector3 hurtVector = transform.position - enemy.position + Vector3.up * 5f;

		// Add a force to the player in the direction of the vector and multiply by the hurtForce.
		GetComponent<Rigidbody2D>().AddForce(hurtVector * hurtForce);

		// Reduce the player's health by 10.
		health -= damageAmount;

		UpdateHealthBar ();
	}

	public void Heal(){
		health += lifePowerupDefault;
		UpdateHealthBar ();
	}

	private void UpdateHealthBar ()
	{
		// Set the health bar's colour to proportion of the way between green and red based on the player's health.
		healthBarSprite.material.color = Color.Lerp(Color.green, Color.red, 1 - health * 0.01f);
		healthBarDecor.material.color = Color.Lerp(Color.green, Color.red, 1 - health * 0.01f);

		// Set the scale of the health bar to be proportional to the player's health.
		healthBarSprite.transform.localScale = new Vector3(healthScale.x * health * 0.01f, healthScale.y, healthScale.z);

		healthBarCanvas.GetComponent<Animator> ().SetTrigger ("headBob");
	}
}
