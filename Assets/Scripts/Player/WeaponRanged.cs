using UnityEngine;
using System.Collections;

public class WeaponRanged : MonoBehaviour {

	public ParticleSystem particleEffect;					// Particle effect of the gun when it is fired.

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

	public void Shoot(){
		// Enable particle system and fire once.
		particleEffect.Play ();
	}
}
