using UnityEngine;
using System.Collections;

public class HoleSpawner : MonoBehaviour {

	public GameObject enemy;

	void OnTriggerEnter2D(Collider2D col){
		if (col.transform.tag == "Player")
			GetComponentInChildren<Animator> ().SetTrigger ("start");
	}

	public void Spawn(){
		GameObject tempEnemy = (GameObject)Instantiate (enemy, transform);
		//tempEnemy.GetComponent<Animator> ().SetTrigger ("start");
		tempEnemy.GetComponent<Animator>().enabled=true;
	}
}
