using UnityEngine;
using System.Collections;

public class SpawningEnemyBullet : EnemyBullet {

	public GameObject spawnEffect;
	public float spawnEffectInterval;
	public float preDamageInterval;
    public AudioSource audioSource;

	private float spawnEffectTimer;
	private float preDamageTimer;
	private bool hasSpawned = false;
	private bool startDamage = false;

	void Awake(){
		base.Awake ();
		spawnEffectTimer = spawnEffectInterval;
		preDamageTimer = preDamageInterval;
	}

	void Start(){
		GameObject portal = (GameObject) Instantiate (spawnEffect, transform.position, transform.rotation);
		portal.GetComponent<ParticleSystem> ().startLifetime = spawnEffectInterval;
		portal.GetComponent<ParticleSystem> ().Play ();
	}

	new void Update(){
		spawnEffectTimer -= Time.deltaTime;

		if (spawnEffectTimer <= 0 && !hasSpawned) {
            audioSource.Play();
			GetComponent<SpriteRenderer> ().enabled = true;
			hasSpawned = true;
		}

		if (hasSpawned)
			preDamageTimer -= Time.deltaTime;

		if (preDamageTimer <= 0 && !startDamage) {
			startDamage = true;
			GetComponent<BoxCollider2D> ().enabled = true;
		}

		if (startDamage)
			base.Update ();
	}
}
