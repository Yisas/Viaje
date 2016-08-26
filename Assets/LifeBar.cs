using UnityEngine;
using System.Collections;

public class LifeBar : MonoBehaviour {

	private PlayerController playerController;
	private Vector3 healthScale;				// The local scale of the health bar initially (with full health).
	private Vector3 powerScale;
	private SpriteRenderer healthBarSprite;		// Health bar sprite.
	private SpriteRenderer healthBarDecor;		// Extra healthbar sprite that needs color changing.
	private SpriteRenderer powerBarSprite;

	void Awake(){
		playerController = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();

		// Getting the intial scale of the healthbar (whilst the player has full health).
		healthBarSprite = transform.FindChild ("lifeBar").GetComponent<SpriteRenderer>();
		healthScale = healthBarSprite.transform.localScale;
		healthBarDecor = transform.FindChild ("head_space_decor").GetComponent<SpriteRenderer>();

		powerBarSprite = transform.FindChild ("powerBar").GetComponent<SpriteRenderer>();
		powerScale = powerBarSprite.transform.localScale;

		// Set lifebar colors to healthy
		healthBarSprite.material.color= Color.green;
		healthBarDecor.material.color = Color.green;
		powerBarSprite.material.color = Color.blue;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void UpdateHealthBar (float health)
	{
		// Set the health bar's colour to proportion of the way between green and red based on the player's health.
		healthBarSprite.material.color = Color.Lerp(Color.green, Color.red, 1 - health * 0.01f);
		healthBarDecor.material.color = Color.Lerp(Color.green, Color.red, 1 - health * 0.01f);

		// Set the scale of the health bar to be proportional to the player's health.
		healthBarSprite.transform.localScale = new Vector3(healthScale.x * health * 0.01f, healthScale.y, healthScale.z);

		GetComponent<Animator> ().SetTrigger ("headBob");
	}

	public void UpdatePowerBar(float bullets){

		SpriteRenderer powerBarSprite = transform.FindChild ("powerBar").GetComponent<SpriteRenderer>();

		// Set the scale of the health bar to be proportional to the player's health.
		powerBarSprite.transform.localScale = new Vector3(powerScale.x * bullets * 0.1f, powerScale.y, powerScale.z);
	}
}
