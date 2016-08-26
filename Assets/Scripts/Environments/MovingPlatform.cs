using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour {

	private Transform leftmostPoint;					// Limits of platform trayectory.
	private Transform rightmostPoint;					// Limits of platform trayectory.
	private Vector3 leftmostPointPosition;				// Fixed vectors for target positions
	private Vector3 rightmostPointPosition;				// Fixed vectors for target positions
	private float journeyLength;
	private bool isGoingLeft = true;
	private bool startMoving = false;

	public float speed;
	public float startTime;

	void Awake(){
		// Setup reference
		leftmostPoint = transform.Find("leftmostPoint");
		rightmostPoint = transform.Find("rightmostPoint");
		leftmostPointPosition = leftmostPoint.position;
		rightmostPointPosition = rightmostPoint.position;
	}


	// Use this for initialization
	void Start () {
			journeyLength = Vector3.Distance (leftmostPoint.position, rightmostPoint.position);	
	}
	
	// Update is called once per frame
	void Update () {

		if (Time.time >= startTime && !startMoving) {
			startMoving = true;
			startTime = Time.time;
		}

		if (startMoving) {
			float distCovered = (Time.time - startTime) * speed;

			if (distCovered >= journeyLength) {
				startTime = Time.time;
				distCovered = 0f;
				isGoingLeft = !isGoingLeft;
			}

			float fracJourney = distCovered / journeyLength;
	
			if (isGoingLeft)
				transform.position = Vector3.Lerp (leftmostPointPosition, rightmostPointPosition, fracJourney);
			else
				transform.position = Vector3.Lerp (rightmostPointPosition, leftmostPointPosition, fracJourney);
		}
	}
}
