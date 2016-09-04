using UnityEngine;
using System.Collections;

public class RudessOnFoot : MonoBehaviour {

	public int health;						
	public int defaultDamageAmount;
	public float attackStartDelay;						// Wait time until starting to attack
	public float attackInterval;						// Wait time between attacks
	public int attacksPerRound;							// Amount of attacks until round is over
	public GameObject bolt;								// Prefab to fire
	public GameObject rudessHead;
	public float transformInterval;
	public float portalScale;

	private GameObject bulletSpawnPoint;
	private Animator anim;
	private float attackStartDelayTimer;
	private bool isAtacking = false;
	private float attackDelayTimer;
	private int currentRoundAttacks;
	private float transformTimer;
	[HideInInspector]
	public bool transformed = false;
	private bool isDead = false;

	void Awake(){
		attackStartDelayTimer = attackStartDelay;
		attackDelayTimer = attackInterval;
		anim = GetComponentInChildren<Animator> ();
		bulletSpawnPoint = transform.FindChild ("bulletSpawn").gameObject;
		currentRoundAttacks = attacksPerRound;
		transformTimer = transformInterval;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (isDead)
			return;

		transformTimer -= Time.deltaTime;
		if(transformTimer <= 0 && !transformed) {
			transformTimer = transformInterval;
			DespawnToHead ();
		}


		if(!isAtacking)
			attackStartDelayTimer -= Time.deltaTime;

		if (attackStartDelayTimer <= 0 && !isAtacking) {
			anim.SetTrigger ("attack");
			attackStartDelayTimer = attackStartDelay;
		}

		if (isAtacking) {
			attackDelayTimer -= Time.deltaTime;
			if (attackDelayTimer <= 0 && currentRoundAttacks > 0) {
				RudessAttack ();
				attackDelayTimer = attackInterval;
				currentRoundAttacks--;
			} 

			if (currentRoundAttacks <= 0) {
				anim.SetTrigger ("stopAttack");
				anim.ResetTrigger ("attack");
			}
		}
	}

	public void RudessAnimatorAttackWarning(){
		isAtacking = true;
	}

	public void RudessAnimatorDoneAttacking(){

	}

	void RudessAttack(){
		Instantiate (bolt, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
	}

	public void TakeDamage(){
		health -= defaultDamageAmount;
		if (health <= 0)
			Die();
		else
			DespawnToHead ();
	}

	void DespawnToHead(){
		transformed = true;
		rudessHead.GetComponent<RudessHead> ().transformed = false;
		rudessHead.SetActive(true);
		rudessHead.GetComponent<RudessHead> ().InstantiatePortals (true);
		//gameObject.SetActive(false);
	}

	public void Die ()
	{
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
			anim.SetTrigger ("die");

			// Reopten "gates"
			GameObject[] gates = GameObject.FindGameObjectsWithTag("SpeakersGate");
			foreach (GameObject go in gates) 
				go.GetComponent<Animator> ().SetTrigger ("shrink");

			// Start despawning timer in update.
			isDead = true;
		}
	}


	public void Deactivate(){
		gameObject.SetActive(false);
	}
}
