using UnityEngine;
using System.Collections;

public class Haze : MonoBehaviour {

	private SpriteRenderer hazeSprite;

	void Awake(){
		hazeSprite = GetComponentInChildren<SpriteRenderer> ();
	}


	// Use this for initialization
	void Start () {
		hazeSprite.color = new Color (1f, 1f, 1f, .5f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
