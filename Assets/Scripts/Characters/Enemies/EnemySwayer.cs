using UnityEngine;
using System.Collections;

// Enemy that just sways back and forth

public class EnemySwayer : SwayingObject
{
	new void Update ()
	{
		base.Update ();

		if (startMoving) {
			// If going right ...
			if (!isGoingLeft) 
				// ... and local scale is positive (default is left)
				if (transform.localScale.x >= 0)
				Flip ();
			
			if (isGoingLeft)
				if (transform.localScale.x < 0)
					Flip ();
		}
	}

	void OnCollisionEnter2D(Collision2D col){
		if (col.transform.tag == "Player")
			col.gameObject.GetComponent<PlayerHealth> ().TakeDamage (transform);
		
	}

	private void Flip ()
	{
		// Multiply the x component of localScale by -1.
		Vector3 enemyScale = transform.localScale;
		enemyScale.x *= -1;
		transform.localScale = enemyScale;
	}
}
