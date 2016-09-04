using UnityEngine;
using System.Collections;

public class AnimatorActivator : MonoBehaviour {

	public string trigger;

	private GameObject animatableGameobject;
	public bool deactivateAfterAnimations = false;

	private Animator anim;
	private Animator[] childAnimators;
	private BoxCollider2D triggerCol;

	void Awake(){
		anim = GetComponent<Animator> ();
		triggerCol = GetComponent<BoxCollider2D> ();
		childAnimators = GetComponentsInChildren<Animator> ();
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.transform.tag == "Player") {
			ActivateAnimation ();
		}
	}

	public void ActivateAnimation(){
		CommonActivation (anim);

		foreach (Animator tempAnim in childAnimators)
			CommonActivation (tempAnim);

		if (deactivateAfterAnimations)
			Destroy (this);
	}

	public void ActivateReferencedAnimation( ){

		animatableGameobject.GetComponent<Animator>().enabled = true;
		animatableGameobject.GetComponent<Animator> ().SetTrigger ("start");

		Animator[] tempChildAnimators = animatableGameobject.GetComponent<Animator>().GetComponentsInChildren<Animator> ();

		foreach (Animator tempAnim in tempChildAnimators)
			tempAnim.enabled = true;
	}


	private void CommonActivation(Animator anmtr){
		if (anmtr != null) {

			anmtr.enabled = true;

			if (trigger != null)
				anmtr.SetTrigger (trigger);
			else
				anmtr.SetTrigger ("start");
		}

	}
}
