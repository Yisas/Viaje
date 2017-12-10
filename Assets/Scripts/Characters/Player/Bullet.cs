using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public float lifeTime;					// Time interval before destoying object
	public bool destroyAfterAnimation;		// Whether to ignore life time and destroy after animation loop.
    public bool destroyAfterCollision = true;
    public AudioClip hitSound;

	private float timer = 0f;		

	// Update is called once per frame
	protected void Update () {
		if (!destroyAfterAnimation) {
			timer += Time.deltaTime;
			if (timer >= lifeTime)
				Destroy (this.gameObject);
		} 
	}

	void OnCollisionEnter2D (Collision2D col){
        if (col.transform.tag == "Enemy")
        {
            col.gameObject.GetComponent<EnemyController>().Die(1);

            if (hitSound)
                AudioSource.PlayClipAtPoint(hitSound, transform.position);
        }

		if (col.gameObject.tag == "EnemyUntracked") {
			// Switch sprite to dead 
			col.gameObject.GetComponent<SpriteSwitch> ().Switch ();
			col.gameObject.GetComponent<Animator> ().SetTrigger ("die");

            if (hitSound)
                AudioSource.PlayClipAtPoint(hitSound, transform.position);
        }

		if (col.gameObject.tag == "RudessOnFoot")
			col.gameObject.GetComponent<RudessOnFoot> ().TakeDamage ();

        if (destroyAfterCollision)
            Destroy(this.gameObject);
        else
            // Stop animation if whatever it hit is not the ground (cancel animation spinning or whatnot)
            if(col.gameObject.layer != LayerMask.NameToLayer("Ground"))
            GetComponent<Animator>().enabled = false;
	}
    /*
    void OnParticleTrigger(Collision col)
    {
        Debug.Log("here");
        if (col.transform.tag == "Enemy")
        {
            col.gameObject.GetComponent<EnemyController>().Die(1);
        }
    }
    */

    public void Destroy (){
		Destroy (this.gameObject);
	}
}
