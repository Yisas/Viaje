using UnityEngine;
using System.Collections;

public class AuxAnimationComunicator : MonoBehaviour {

	private PlayerController playerController;
	private EnemyRanged enemyRangedController;
	private RudessOnFoot rudess;
	private RudessHead rudessHead;
	private AnimatorActivator animatorActivator;
	private PlayerSwitch playerSwitch;
	private HoleSpawner holeSpawner;

	void Awake() {
		playerController = GetComponentInParent<PlayerController> ();
		enemyRangedController = GetComponentInParent<EnemyRanged> ();
		rudess = GetComponentInParent<RudessOnFoot> ();
		rudessHead = GetComponentInParent<RudessHead> ();
		animatorActivator = GetComponent<AnimatorActivator> ();
		holeSpawner = GetComponentInParent<HoleSpawner> ();
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

	void RudessOnFootDeactivate(){
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

	void ActivateCollider(){
		Collider2D[] cols = GetComponents<Collider2D> ();

		foreach (Collider2D col in cols)
			col.enabled = true;
	}

	void HoleSpawn(){
		holeSpawner.Spawn ();
	}
		
}
