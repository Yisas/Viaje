using UnityEngine;
using System.Collections;

// Enemy that just sways back and forth

public class EnemySwayer : SwayingObject
{
	new void Update ()
	{
		base.Update ();

		Debug.Log (transform.localScale.x);
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

	private void Flip ()
	{
		// Multiply the x component of localScale by -1.
		Vector3 enemyScale = transform.localScale;
		enemyScale.x *= -1;
		transform.localScale = enemyScale;
	}
}
