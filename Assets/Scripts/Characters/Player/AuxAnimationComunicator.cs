using UnityEngine;
using System.Collections;

public class AuxAnimationComunicator : MonoBehaviour {

	private PlayerController playerController;
	private EnemyRanged enemyRangedController;
	private RudessOnFoot rudess;
	private RudessHead rudessHead;

	void Awake() {
		playerController = GetComponentInParent<PlayerController> ();
		enemyRangedController = GetComponentInParent<EnemyRanged> ();
		rudess = GetComponentInParent<RudessOnFoot> ();
		rudessHead = GetComponentInParent<RudessHead> ();
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
}
