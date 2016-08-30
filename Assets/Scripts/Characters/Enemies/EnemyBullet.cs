using UnityEngine;
using System.Collections;

public class EnemyBullet : Bullet {

	private PlayerHealth playerHealth;

	void Awake(){
		playerHealth = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerHealth> ();
	}

	void Update(){
		base.Update ();
	}

	new void OnCollisionEnter2D (Collision2D col){
		if (col.transform.tag == "Player") {
			playerHealth.TakeDamage (transform);
			Destroy (this.gameObject);
		}
	}
}
