using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour {

	private float timer;
	private bool loading = false;

	// Use this for initialization
	void Start () {
		timer = 5;
	}
	
	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;
		if (timer <= 0 && !loading) {
			loading = true;
			SceneManager.LoadScene ("Llovizna");
		}
	}
}
