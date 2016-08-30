using UnityEngine;
using System.Collections;

public class AuxAnimationComunicator : MonoBehaviour {

	private PlayerController playerController;
	private EnemyRanged enemyRangedController;

	void Awake(){
		playerController = GetComponentInParent<PlayerController> ();
		enemyRangedController = GetComponentInParent<EnemyRanged> ();
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
}
