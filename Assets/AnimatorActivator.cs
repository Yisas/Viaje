using UnityEngine;
using System.Collections;

public class AnimatorActivator : MonoBehaviour {

	public GameObject animatableGameobject;

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
		anim.enabled = true;
		anim.SetTrigger ("start");

		foreach (Animator tempAnim in childAnimators)
			tempAnim.enabled = true;
	}

	public void ActivateReferencedAnimation( ){

		animatableGameobject.GetComponent<Animator>().enabled = true;
		animatableGameobject.GetComponent<Animator> ().SetTrigger ("start");

		Animator[] tempChildAnimators = animatableGameobject.GetComponent<Animator>().GetComponentsInChildren<Animator> ();

		foreach (Animator tempAnim in tempChildAnimators)
			tempAnim.enabled = true;
	}
}
