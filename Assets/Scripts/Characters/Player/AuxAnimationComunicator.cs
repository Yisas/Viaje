﻿using UnityEngine;
using System.Collections;

public class AuxAnimationComunicator : MonoBehaviour {

	private PlayerController playerController;
	private EnemyRanged enemyRangedController;
	private RudessOnFoot rudess;
	private RudessHead rudessHead;
	private AnimatorActivator animatorActivator;
	private PlayerSwitch playerSwitch;

	void Awake() {
		playerController = GetComponentInParent<PlayerController> ();
		enemyRangedController = GetComponentInParent<EnemyRanged> ();
		rudess = GetComponentInParent<RudessOnFoot> ();
		rudessHead = GetComponentInParent<RudessHead> ();
		animatorActivator = GetComponent<AnimatorActivator> ();
	}

	void FinishAttacking(){
		playerController.FinishAttacking ();
	}

	void UnlockMovement(){
		enemyRangedController.UnlockMovement ();
	}

	void ThrowBullet(){
		enemyRangedController.ThrowBullet ();
	}

	void ShootBullet(){
		playerController.ShootBullet ();
	}

	void RudessAttack(){
		rudess.RudessAnimatorAttackWarning ();
	}

	void RudessDoneAttacking(){
		rudess.RudessAnimatorDoneAttacking ();
	}

	void RudessDeactivate(){
		rudess.Deactivate ();
	}

	void RudessHeadRescaleSprites(){
		rudessHead.RescaleSprites ();
	}

	void DestroyPlayer(){
		Destroy(GameObject.FindGameObjectWithTag ("Player"));
	}

	void ActivateReferencedAnimation(){
		animatorActivator.ActivateReferencedAnimation ();
	}
	/*
	void SwitchPlayers(){
		playerSwitch.SwitchPlayers ();
	}
	*/

	public void Deleteme(bool hey){

	}
}
