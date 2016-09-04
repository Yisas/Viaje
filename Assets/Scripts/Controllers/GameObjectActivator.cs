using UnityEngine;
using System.Collections;

public class GameObjectActivator : MonoBehaviour
{

	public GameObject go;
	public bool delayedActivation = false;
	public float delay;
	public bool disableAfterActivation;

	private bool startTimer = false;
	private float timer;

	void Start ()
	{
		timer = delay;
	}

	void Update ()
	{
		if (delayedActivation && startTimer) {
			timer -= Time.deltaTime;
			if (timer <= 0)
				Activate ();
		}
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.transform.tag == "Player")
		if (delayedActivation)
			startTimer = true;
		else
			Activate ();
	}

	private void Activate(){
		go.SetActive (true);
		if (disableAfterActivation)
			this.enabled = false;
	}
}
