using UnityEngine;
using System.Collections;

public class RudessOnFoot : MonoBehaviour {

	public int health;						
	public float attackStartDelay;						// Wait time until starting to attack
	public float attackInterval;						// Wait time between attacks
	public int attacksPerRound;							// Amount of attacks until round is over
	public GameObject bolt;							// Prefab to fire

	private GameObject bulletSpawnPoint;
	private Animator anim;
	private float attackStartDelayTimer;
	private bool isAtacking = false;
	private float attackDelayTimer;
	private int currentRoundAttacks;

	void Awake(){
		attackStartDelayTimer = attackStartDelay;
		attackDelayTimer = attackInterval;
		anim = GetComponentInChildren<Animator> ();
		bulletSpawnPoint = transform.FindChild ("bulletSpawn").gameObject;
		currentRoundAttacks = attacksPerRound;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

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


}
