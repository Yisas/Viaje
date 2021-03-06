﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HealthDrop : MonoBehaviour {

	public AudioClip[] consumeClips;			// List of clips to play when health drop is consumed by player.
	public AudioClip landClip;					// Sound that plays when item lands on the ground.
    public AudioSource audioSource;

	private Animator anim;
	private bool hasLanded = false;
    private bool startDestroy = false;

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

	void OnCollisionEnter2D(Collision2D col){

		if (col.gameObject.tag == "Enemy") {
			Physics2D.IgnoreCollision(col.collider, GetComponent<Collider2D>());
		}

		// On collision with ground deactivate disposable landing gear. Layer 8 should be Ground
		if (col.gameObject.layer == 8) {
			if (!hasLanded) {
				// Eliminate disposable landing gear
				foreach (Transform child in transform)
					if (child.tag == "HealthDropDispose")
						child.GetComponent<SpriteRenderer> ().enabled = false;
				// Deactivate animator
				anim.enabled=false;

                audioSource.clip = landClip;
                audioSource.Play();
				hasLanded = true;
			}
		}

		if (col.gameObject.tag == "Player") {
			// Heal player
			col.gameObject.GetComponent<PlayerHealth> ().Heal ();

			// Play random consume audioclip
			int i = Random.Range(0, consumeClips.Length);

            AudioSource source = col.gameObject.GetComponent<AudioSource>();

            source.clip = consumeClips[i];
            source.Play();
			Destroy (this.gameObject);
		}
	}
}
			
