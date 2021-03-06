using UnityEngine;
using System.Collections;

public class CutsceneWaypoint : MonoBehaviour
{

	public float waypointTrayectoryTime;
	// Amount of time it takes for the player to move towards the trayectory.
	public GameObject consequentWaypoint;
	// If there is another waypoing to move to, attach similar gameobject
	public bool deactivateAfterCompletion;
	public bool animateMovement;

	private GameObject player;
	private Transform waypoint;
	[HideInInspector]
	public bool started = false;
	private float initialPositionX;
	private float initialPositionY;
	private float timer;
	Vector2 newPosition = new Vector2 (0f, 0f);

	void Awake ()
	{
		player = GameObject.FindGameObjectWithTag ("Player");
		waypoint = transform.Find ("waypoint");
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		if (!started)
		if (col.transform.tag == "Player") {
			player.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);
			player.GetComponentInChildren<Animator> ().SetFloat ("Speed", 0);
			player.GetComponent<PlayerController> ().enabled = false;
			player.GetComponent<BoxCollider2D> ().enabled = false;
			player.GetComponent<CircleCollider2D> ().enabled = false;
			StartMovement ();
		}
	}

	void Update ()
	{
		if (started) {
			timer += Time.deltaTime;

			if (timer <= waypointTrayectoryTime) {
				// Calculate next position
				newPosition.x = Mathf.Lerp (initialPositionX, waypoint.position.x, timer / waypointTrayectoryTime);
				newPosition.y = Mathf.Lerp (initialPositionY, waypoint.position.y, timer / waypointTrayectoryTime);

				if (animateMovement) {
					player.GetComponent<PlayerController> ().ExitWater ();
					player.GetComponentInChildren<Animator> ().SetFloat ("Speed", 1);
					player.GetComponent<PlayerController> ().CheckDirection ((waypoint.position.x - initialPositionX) / waypointTrayectoryTime);
				}

				// Apply to player
				player.transform.position = newPosition;
			} else if (timer > waypointTrayectoryTime) {
				player.GetComponent<PlayerController> ().enabled = true;
				player.GetComponent<PlayerController> ().ExitWater ();
				player.GetComponent<BoxCollider2D> ().enabled = true;
				player.GetComponent<CircleCollider2D> ().enabled = true;
				started = false;
				timer = 0;

				if (consequentWaypoint)
					consequentWaypoint.GetComponent<CutsceneWaypoint> ().StartMovement ();

				if(animateMovement)
					player.GetComponentInChildren<Animator> ().SetFloat ("Speed", 0);

				if (deactivateAfterCompletion)
					gameObject.SetActive(false);
			}
		}
	}

	public void StartMovement(){
		started = true;
		initialPositionX = transform.position.x;
		initialPositionY = transform.position.y;
		timer = 0;
	}
}

