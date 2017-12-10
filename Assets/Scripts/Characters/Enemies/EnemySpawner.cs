using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
	public int maxNumberOfEnemies;	
	public float spawnTime;				// The amount of time between each spawn.
	public float spawnDelay;			// The amount of time before spawning starts.
	public GameObject[] enemies;		// Array of enemy prefabs.
	public Object spawnEffect;

	private GameController gameController;
	private bool isActive=false;		// Start spawning on true.
	private Transform dropRangeLeft;			// Smallest value of x in world coordinates the delivery can happen at.
	private Transform dropRangeRight;			// Largest value of x in world coordinates the delivery can happen at.

	[HideInInspector]
	public int numberOfEnemies=0;
	private float spawnTimer;
	private Vector3 nextSpawnPosition;
	private int nextEnemyIndex;
	private bool spawning = false;
	private GameObject currentParticleEffect;

	void Awake(){
        gameController = GameController.GetInstance();
		dropRangeLeft = transform.Find ("dropRangeLeft");
		dropRangeRight = transform.Find ("dropRangeRight");
		spawnTimer = spawnTime;
	}

	void Update(){
		// Waiting for delay
		if (spawnDelay > 0)
			spawnDelay -= Time.deltaTime;
		// Else start spawning.
		else {
			if (isActive && (gameController.numberOfEnemies < gameController.maxNumberOfEnemies && numberOfEnemies < maxNumberOfEnemies)) {
				// Spawn aura
				if (spawnTimer > 0 && !spawning) {
					nextEnemyIndex = Random.Range (0, enemies.Length);

					nextSpawnPosition = FindSpawnTarget ();

					spawning = true;

					currentParticleEffect = (GameObject) Instantiate (spawnEffect,nextSpawnPosition,new Quaternion(0,0,0,1));

					currentParticleEffect.GetComponent<ParticleSystem> ().startLifetime = spawnTime;
					currentParticleEffect.GetComponent<ParticleSystem> ().Play ();
				} else if(spawnTimer<=0 && spawning){
					// Final instantiation
					GameObject enemy =(GameObject) Instantiate (enemies [nextEnemyIndex], nextSpawnPosition, transform.rotation);
					enemy.GetComponent<EnemyController> ().enemySpawner = GetComponent<EnemySpawner>();
					numberOfEnemies++;
					// Reset timer and flags
					spawning = false;
					spawnTimer = spawnTime;
				}
				spawnTimer -= Time.deltaTime;
		}
	}

	}

	void Spawn ()
	{
		if (isActive) {
			if (gameController.numberOfEnemies < gameController.maxNumberOfEnemies && numberOfEnemies < maxNumberOfEnemies) {

				int enemyIndex = Random.Range (0, enemies.Length);

				Vector3 dropPos = FindSpawnTarget ();

				GameObject enemy =(GameObject) Instantiate (enemies [enemyIndex], dropPos, transform.rotation);
				enemy.GetComponent<EnemyController> ().enemySpawner = GetComponent<EnemySpawner>();
				numberOfEnemies++;
			}
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

	private Vector3 FindSpawnTarget(){
		RaycastHit2D hit= new RaycastHit2D();
		Vector3 dropPos;
		List<int> usedValues = new List<int>();

		GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");

		foreach(GameObject go in enemies){
			for(int p= (int)go.transform.position.x - 2; p < (int)go.transform.position.x + 2; p++)
			usedValues.Add (p);
				}


		int i = 0;

		do {

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

		return dropPos;
	}
}
