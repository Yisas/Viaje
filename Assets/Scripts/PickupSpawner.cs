using UnityEngine;
using System.Collections;

public class PickupSpawner : MonoBehaviour {
	
	public float pickupDeliveryTime = 5f;		// Delay on delivery.
	public GameObject dropItem;					// Item that gets spawned.

	private Transform dropRangeLeft;			// Smallest value of x in world coordinates the delivery can happen at.
	private Transform dropRangeRight;			// Largest value of x in world coordinates the delivery can happen at.
	private bool depleted = false;				// Stop spawning when triggered.

	void Awake(){
		dropRangeLeft = transform.FindChild ("dropRangeLeft");
		dropRangeRight = transform.FindChild ("dropRangeRight");
	}

	void Start (){
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.transform.tag == "Player") {
			// Start the first delivery.
			StartCoroutine (DeliverPickup ());
			depleted = true;
		}
	}

	public IEnumerator DeliverPickup()
	{
		if (!depleted) {
			// Wait for the delivery delay.
			yield return new WaitForSeconds (pickupDeliveryTime);

			// Create a random x coordinate for the delivery in the drop range.
			float dropPosX = Random.Range (dropRangeLeft.position.x, dropRangeRight.position.x);

			// Create a position with the random x coordinate.
			Vector3 dropPos = new Vector3 (dropPosX, transform.position.y, transform.position.z);

			Instantiate (dropItem, dropPos, transform.rotation);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
