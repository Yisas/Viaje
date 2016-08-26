using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
	public float maxNumberOfEnemies;	
	public float spawnTime = 5f;		// The amount of time between each spawn.
	public float spawnDelay = 3f;		// The amount of time before spawning starts.
	public GameObject[] enemies;		// Array of enemy prefabs.

	private GameController gameController;
	private bool isActive=false;		// Start spawning on true.
	private Transform dropRangeLeft;			// Smallest value of x in world coordinates the delivery can happen at.
	private Transform dropRangeRight;			// Largest value of x in world coordinates the delivery can happen at.

	void Awake(){
		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController>();
		dropRangeLeft = transform.FindChild ("dropRangeLeft");
		dropRangeRight = transform.FindChild ("dropRangeRight");
	}

	void Start ()
	{
		// Start calling the Spawn function repeatedly after a delay .
		InvokeRepeating("Spawn", spawnDelay, spawnTime);
	}


	void Spawn ()
	{
		if (isActive) {
			if (gameController.numberOfEnemies <= gameController.maxNumberOfEnemies) {
				// Instantiate a random enemy.
				int enemyIndex = Random.Range (0, enemies.Length);

				// Create a random x coordinate for the delivery in the drop range.
				float dropPosX = Random.Range (dropRangeLeft.position.x, dropRangeRight.position.x);

				// Create a position with the random x coordinate.
				Vector3 dropPos = new Vector3 (dropPosX, dropRangeLeft.position.y, transform.position.z);

				Instantiate (enemies [enemyIndex], dropPos, transform.rotation);
			}

			/*
		// Play the spawning effect from all of the particle systems.
		foreach(ParticleSystem p in GetComponentsInChildren<ParticleSystem>())
		{
			p.Play();
		}
		*/
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.transform.tag == "Player")
			isActive = true;
	}
}
