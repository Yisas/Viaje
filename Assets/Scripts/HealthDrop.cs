using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HealthDrop : MonoBehaviour {

	public AudioClip[] consumeClips;			// List of clips to play when health drop is consumed by player.

	private Animator anim;

	void Awake(){
		//Set up references
		anim = GetComponent<Animator>();
	}

	// Use this for initialization
	void Start () {
		/*
		// Don't collide with enemies
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

		foreach(GameObject enemy in enemies)
			Physics2D.IgnoreCollision(enemy.GetComponent<Collider2D>(), GetComponent<Collider2D>());
		*/
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D col){

		if (col.gameObject.tag == "Enemy") {
			Physics2D.IgnoreCollision(col.collider, GetComponent<Collider2D>());
		}

		// On collision with ground deactivate disposable landing gear. Layer 8 should be Ground
		if (col.gameObject.layer == 8) {
			anim.SetTrigger("landed");
		}

		if (col.gameObject.tag == "Player") {
			// Heal player
			col.gameObject.GetComponent<PlayerHealth> ().Heal ();

			// Play random consume audioclip
			int i = Random.Range(0, consumeClips.Length);
			AudioSource.PlayClipAtPoint(consumeClips[i], transform.position);
			Destroy (this.gameObject);
		}
	}
}
			
