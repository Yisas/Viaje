using UnityEngine;
using System.Collections;

public class WeaponMelee : MonoBehaviour {

	private PlayerController playerController;

	void Awake(){
		//Set up references
		playerController= GetComponentInParent<PlayerController>();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D (Collision2D col){
		if (col.gameObject.tag == "Enemy") 
			col.gameObject.GetComponent<EnemyController> ().Die (0);

		if (col.gameObject.tag == "EnemyUntracked") {
			// Switch sprite to dead 
			col.gameObject.GetComponent<SpriteSwitch> ().Switch ();
			col.gameObject.GetComponent<Animator> ().SetTrigger ("die");
		}

		if (col.gameObject.tag == "RudessOnFoot")
			col.gameObject.GetComponent<RudessOnFoot> ().TakeDamage ();
	}
}
