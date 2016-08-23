using UnityEngine;
using System.Collections;

public class WeaponRanged : MonoBehaviour {

	private PlayerController playerController;
	private Transform shotSpawn;							// Transform of empty object where the shot is initialized

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

		if (col.gameObject.tag == "Enemy") {
			col.gameObject.GetComponent<EnemyController> ().Die ();
			playerController.MeleeHit ();
		}
	}
}
