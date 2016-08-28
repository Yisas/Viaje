using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Slideshow : MonoBehaviour {

	public float slideshowInterval;

	private SpriteRenderer[] pictures;
	private float slideshowTimer;
	private int currentTextureIndex = 0;

	void Awake(){

	}

	// Use this for initialization
	void Start () {
		pictures = transform.GetComponentsInChildren<SpriteRenderer> ();
		slideshowTimer = slideshowInterval;
		StartDisplay (currentTextureIndex);
		currentTextureIndex++;
		Mathf.Clamp (currentTextureIndex, 0, pictures.Length-1);
	}
	
	// Update is called once per frame
	void Update () {
		slideshowTimer -= Time.deltaTime;
		if (slideshowTimer <= 0) {
			slideshowTimer = 0;
			StopDisplay(currentTextureIndex);
			currentTextureIndex++;
			if (currentTextureIndex > pictures.Length - 1)
				currentTextureIndex = 0;
			StartDisplay(currentTextureIndex);
			slideshowTimer = slideshowInterval;
		}
	}

	private void StopDisplay(int index){
		pictures[index].enabled = false;
		pictures[index].GetComponent<AlphaFade> ().Reset ();
	}

	private void StartDisplay(int index){
		pictures[index].GetComponent<AlphaFade> ().start = true;
	}
}
