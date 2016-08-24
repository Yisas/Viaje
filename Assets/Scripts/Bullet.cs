using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public float lifeTime;					// Time interval before destoying object
	public bool destroyAfterAnimation;		// Whether to ignore life time and destroy after animation loop.

	private float timer=0f;		

	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
		if (!destroyAfterAnimation) {
			timer += Time.deltaTime;
			if (timer >= lifeTime)
				Destroy (this.gameObject);
		} 
	}

	void OnCollisionEnter2D (Collision2D col){
		Debug.Log ("here");
		if (col.transform.tag=="Enemy")
			col.gameObject.GetComponent<EnemyController> ().Die ();
	}

	public void Destroy (){
		Destroy (this.gameObject);
	}
}
