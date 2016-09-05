using UnityEngine;
using System.Collections;

public class TimedDespawner : MonoBehaviour {

	public float despawnTimer;

	private float timer;

	void Awake(){
		timer = despawnTimer;
	}

	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;
		if (timer <= 0)
			Destroy (this.gameObject);
	}
}
