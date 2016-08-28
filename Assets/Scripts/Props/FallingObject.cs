using UnityEngine;
using System.Collections;

public class FallingObject : MonoBehaviour {

	public float warningTime;					// Time the object is suspended is a pre falling animation
	public float despawnTime; 					// After hitting something, time until this object is destroyed.
	public AudioClip breakClip;					// Clip played on destroy.

	private Animator anim;
	private float warningTimer;					// Time remaining until the object falls.
	private float despawnTimer;					// Time remaining AFTER fall collision for object to be destroyed.
	bool isWarning=false;						// When true start pre falling sequence.
	bool isDespawning=false;					// When true start despawning timer.

	void Awake(){
		anim=GetComponent<Animator> ();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (isWarning) {
			warningTimer -= Time.deltaTime;
			anim.SetFloat ("warningTimer", warningTimer);
			if (warningTimer <= 0)
				GetComponent<Rigidbody2D> ().isKinematic = false;
		}

		if (isDespawning) {
			despawnTimer -= Time.deltaTime;
			if (despawnTimer <= 0)
				Destroy (this.gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.transform.tag == "Player") {
			if (!isWarning) {
				isWarning = true;
				warningTimer = warningTime;
				anim.SetFloat ("warningTimer", warningTimer);
			}
		}
	}

	void OnCollisionEnter2D(Collision2D col){
		if (col.transform.tag == "Player") {
			GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerHealth> ().TakeDamage (this.transform);
			AudioSource.PlayClipAtPoint (breakClip, transform.position);
		}

		if (col.collider.gameObject.layer == LayerMask.NameToLayer("Ground")){
			AudioSource.PlayClipAtPoint (breakClip, transform.position);
			StartDespawning ();
		}
	}

	private void StartDespawning(){
		isDespawning = true;
		despawnTimer = despawnTime;
	}
}
