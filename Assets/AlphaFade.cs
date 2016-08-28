using UnityEngine;
using System.Collections;

public class AlphaFade : MonoBehaviour {

	public float minimum = 0.0f;
	public float maximum = 1f;
	public float displayedDuration = 2f;
	//[HideInInspector]
	public bool start = false;

	private float duration;									// Inherit from parent slideshow.
	private float timer = 0;
	private SpriteRenderer sprite;
	private float transitionTime;
	private bool started = false;

	void Start() {
		sprite = GetComponent<SpriteRenderer> ();
		duration = GetComponentInParent<Slideshow> ().slideshowInterval;
		transitionTime = (duration / 2) - (displayedDuration / 2);
	}

	void Update() {

		if (start) {
			timer = duration;
			start = false;
			started = true;
		}

		//Debug.Log (timer);
		if (started) {
			// Before waiting interval
			if (timer <= transitionTime)
				sprite.color = new Color (1f, 1f, 1f, Mathf.SmoothStep (minimum, maximum, timer / transitionTime));
			// After waiting interval
			else if (timer > transitionTime + displayedDuration){
				sprite.color = new Color (1f, 1f, 1f, Mathf.SmoothStep (maximum, minimum, (timer - transitionTime - displayedDuration) / transitionTime));  
			}

			timer -= Time.deltaTime;

			if (sprite.enabled == false)
				sprite.enabled = true;
		}
	}

	public void Reset(){
		started = false;
	}
}