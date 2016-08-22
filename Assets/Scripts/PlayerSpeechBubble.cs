using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerSpeechBubble : MonoBehaviour {

	public float changeTextInterval;	 	// Countdown interval amount for next bubble.	
	public float displayInterval;			// Time bubble is on screen.

	private bool isVisible = false;			// Is canvas currently visible on scren.
	private Canvas canvas;
	private Text text;						// Text inside speech bubble.
	private float timer = 0f;				// Timer for displaying the bubble.

	void Awake (){

		// Set up references
		text = this.GetComponentInChildren<Text>();
		canvas = this.GetComponentInChildren<Canvas>();
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
		timer += Time.deltaTime;

		if (timer >= changeTextInterval && !isVisible) {
			timer = 0f;
			canvas.enabled = true;
			isVisible = true;
		}
		else if (timer >= displayInterval && isVisible) {
			timer = 0f;
			canvas.enabled = false;
			isVisible = false;
		}
	}

	// Orientation is screwed up by PlayerController, this fixes it. There is prob a better solution.
	public void ReRotate(){
			text.transform.Rotate (0,180,0);
	}
}
