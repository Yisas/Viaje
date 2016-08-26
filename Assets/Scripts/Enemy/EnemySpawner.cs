using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
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
		if(gameController.numberOfEnemies <= gameController.maxNumberOfEnemies)
		// Start calling the Spawn function repeatedly after a delay .
		InvokeRepeating("Spawn", spawnDelay, spawnTime);
	}


	void Spawn ()
	{
		int i = 0;

		if (isActive) {
			if (gameController.numberOfEnemies <= gameController.maxNumberOfEnemies) {

				RaycastHit2D hit= new RaycastHit2D();

				int enemyIndex;
				Vector3 dropPos;
				List<int> usedValues = new List<int>();

				do {
					// Instantiate a random enemy.
					enemyIndex = Random.Range (0, enemies.Length);

					// Create a random x coordinate for the delivery in the drop range.
					int dropPosX = UniqueRandomInt((int)dropRangeLeft.position.x,(int)dropRangeRight.position.x, usedValues);
					//float dropPosX = Random.Range(dropRangeLeft.position.x,dropRangeRight.position.x);
					usedValues.Add(dropPosX);

					// Create a position with the random x coordinate.
					dropPos = new Vector3 (dropPosX, dropRangeLeft.position.y, transform.position.z);

					hit= new RaycastHit2D();
					// Layer nine should be characters, make sure you are not on an character
					hit = Physics2D.Raycast (dropPos, -Vector2.up,10f,9);

					i++;

					if(i>=100){
						i=0;
						//Debug.Log("Spawner error");
						hit= new RaycastHit2D();

					}

				} while (hit.collider!=null);

				hit= new RaycastHit2D();

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


	public int UniqueRandomInt(int min, int max, List<int> usedValues)
	{
		int i = 0;
		int val = Random.Range(min, max);
		while(usedValues.Contains(val) && i<=1000)
		{
			val = Random.Range(min, max);
			i++;
		}
		return val;
	}
}
