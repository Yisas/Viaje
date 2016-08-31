using UnityEngine;
using System.Collections;

public class AuxAnimationComunicator : MonoBehaviour {

	private PlayerController playerController;
	private EnemyRanged enemyRangedController;
	private RudessOnFoot rudess;

	void Awake() {
		playerController = GetComponentInParent<PlayerController> ();
		enemyRangedController = GetComponentInParent<EnemyRanged> ();
		rudess = GetComponentInParent<RudessOnFoot> ();
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
}
