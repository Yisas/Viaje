using UnityEngine;
using System.Collections;

public class EnemyRanged : EnemyController {

	public GameObject bullet;
	public float throwForceHorizontal;
	public float throwForceVertical;

	private bool deleteme = true;

	void Update() {
		if (deleteme) {
			lockMovement = true;
			Debug.Log ("hey");
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);
			anim.SetFloat ("Speed",0);
			ThrowSetup ();
			deleteme = false;
		}
	}

	// For playing animations prior to throwing object
	void ThrowSetup(){
		anim.SetTrigger ("rangedAttack");

	}

	public void UnlockMovement(){
		lockMovement = false;
	}

	public void ThrowBullet(){
		// Instantiate object
		Transform bulletSpawnPoint = transform.FindChild("bulletSpawnPoint");
		GameObject bulletInstance = (GameObject) Instantiate (bullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

		// Add force
		bulletInstance.GetComponent<Rigidbody2D>().AddForce(Vector2.left * throwForceHorizontal);
		bulletInstance.GetComponent<Rigidbody2D>().AddForce(Vector2.up * throwForceVertical);
	}
}
