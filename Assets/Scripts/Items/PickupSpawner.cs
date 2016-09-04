using UnityEngine;
using System.Collections;

public class PickupSpawner : MonoBehaviour
{
	
	public float pickupDeliveryTime = 5f;
	// Delay on delivery.
	public GameObject dropItem;
	// Item that gets spawned.
	public int maxPickups = 1;

	private Transform dropRangeLeft;
	// Smallest value of x in world coordinates the delivery can happen at.
	private Transform dropRangeRight;
	// Largest value of x in world coordinates the delivery can happen at.
	private bool depleted = false;
	// Stop spawning when triggered.
	private int remainingPickups;
	private bool started = false;
	private bool waiting = false;

	void Awake ()
	{
		dropRangeLeft = transform.FindChild ("dropRangeLeft");
		dropRangeRight = transform.FindChild ("dropRangeRight");
		remainingPickups = maxPickups;
	}


	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.transform.tag == "Player") {
			started = true;
		}
	}


	// Update is called once per frame
	void Update ()
	{
		if (started && !depleted && !waiting) {
			StartCoroutine (DeliverPickup ());
		}
	}

	public IEnumerator DeliverPickup ()
	{
		waiting = true;
			
		// Wait for the delivery delay.
		yield return new WaitForSeconds (pickupDeliveryTime);

		waiting = false;

		// Create a random x coordinate for the delivery in the drop range.
		float dropPosX = Random.Range (dropRangeLeft.position.x, dropRangeRight.position.x);

		// Create a position with the random x coordinate.
		Vector3 dropPos = new Vector3 (dropPosX, transform.position.y, transform.position.z);

		Instantiate (dropItem, dropPos, transform.rotation);
			
		if (--remainingPickups <= 0)
			depleted = true;
	}
}
