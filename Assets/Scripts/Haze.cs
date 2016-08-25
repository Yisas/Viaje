using UnityEngine;
using System.Collections;

public class Haze : MonoBehaviour {

	public float hazePeriod;					// Amount of time the haze effect is visible on screen.

	private Canvas hazeCanvas;
	private SpriteRenderer hazeSprite;
	private float timer = 0;					// Countdown timer for haze effect being visible on screen
	private bool isDisplaying = false;

	void Awake(){
		hazeCanvas = GameObject.FindGameObjectWithTag ("HazeEffectCanvas").GetComponent<Canvas>();
		hazeSprite = hazeCanvas.GetComponentInChildren<SpriteRenderer> ();
	}


	// Use this for initialization
	void Start () {
		hazeSprite.color = new Color (1f, 1f, 1f, .5f);
	}
	
	// Update is called once per frame
	void Update () {
		if (isDisplaying && (Time.time - timer) >= hazePeriod) {
				timer = 0;
				hazeSprite.enabled = false;
				isDisplaying = false;
			}
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.transform.tag == "Player") {
			timer = Time.time;
			hazeSprite.enabled = true;
			isDisplaying = true;
		}
	}
}
