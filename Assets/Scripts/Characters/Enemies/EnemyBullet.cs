using UnityEngine;
using System.Collections;

public class EnemyBullet : Bullet {

	private PlayerHealth playerHealth;

	protected void Awake(){
		playerHealth = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerHealth> ();
	}

	protected void Update(){
		base.Update ();
	}

	new void OnCollisionEnter2D (Collision2D col){
		if (col.transform.tag == "Player") {
			playerHealth.TakeDamage (transform);
			Destroy (this.gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.transform.tag == "Player") {
			playerHealth.TakeDamage (transform);
			Destroy (this.gameObject);
		}
	}
}
