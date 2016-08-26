using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HealthDrop : MonoBehaviour {

	private Animator anim;

	void Awake(){
		//Set up references
		anim = GetComponent<Animator>();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D col){
		// On collision with ground deactivate disposable landing gear. Layer 8 should be Ground
		if (col.gameObject.layer == 8) {
			//landingGear.ForEach (DeactivateSprite);
			anim.SetTrigger("landed");
		}
	}
}
			
