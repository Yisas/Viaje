using UnityEngine;
using System.Collections;

public class AuxPlayerAnimation : MonoBehaviour {

	private PlayerController playerController;

	void Awake(){
		playerController = GetComponentInParent<PlayerController> ();
	}

	void FinishAttacking(){
		playerController.FinishAttacking ();
	}
}
