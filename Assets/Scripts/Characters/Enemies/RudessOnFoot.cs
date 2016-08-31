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
		Debug.Log (health);
		DespawnToHead ();
	}

	void DespawnToHead(){
		transformed = true;
		rudessHead.GetComponent<RudessHead> ().transformed = false;
		rudessHead.SetActive(true);
		rudessHead.GetComponent<RudessHead> ().InstantiatePortals (true);
		//gameObject.SetActive(false);
	}

	public void Deactivate(){
		gameObject.SetActive(false);
	}
}
