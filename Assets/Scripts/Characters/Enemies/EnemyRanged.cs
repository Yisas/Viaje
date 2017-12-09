using UnityEngine;
using System.Collections;

public class EnemyRanged : EnemyController
{

	public GameObject bullet;
	public float throwForceHorizontal;
	public float throwForceVertical;
	public float rangedAttackTime;

	private float rangedAttackTimer = 0;

	new void Awake ()
	{
		base.Awake ();
		rangedAttackTimer = rangedAttackTime;
	}

	new void Update ()
	{
		base.Update ();

		if (!isDead) {
			rangedAttackTimer -= Time.deltaTime;
			if (rangedAttackTimer <= 0) {
				rangedAttackTimer = rangedAttackTime;

				RaycastHit2D raycastHit;
				raycastHit = Physics2D.Raycast (frontCheck.position, Vector2.right * FindPlayer ());
				if (raycastHit)
				if (raycastHit.collider.transform.tag == "Player")
					ThrowSetup ();
			}
		}
	}

	// For playing animations prior to throwing object
	void ThrowSetup ()
	{
		GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);
		anim.SetFloat ("Speed", 0);
		

		lockMovement = true;

		anim.SetTrigger ("rangedAttack");

	}

	public void UnlockMovement ()
	{
		lockMovement = false;
	}

	public void ThrowBullet ()
	{
		// Instantiate object
		Transform bulletSpawnPoint = transform.Find ("bulletSpawnPoint");
		GameObject bulletInstance = (GameObject)Instantiate (bullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

		int direction = FindPlayer ();

		// Add force
		bulletInstance.GetComponent<Rigidbody2D> ().AddForce (Vector2.right * throwForceHorizontal * direction);
		bulletInstance.GetComponent<Rigidbody2D> ().AddForce (Vector2.up * throwForceVertical);
	}
}
