using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour {

	public GameObject[] visibleDuringLoad;
	public float loadMinDuration;
	public float waitBeforeLoad;

	private float timer;
	private bool loading = false;
	private float endTime = 100000f;
	private AsyncOperation ao;
	private GameController gameController;

	void Awake(){
		foreach (GameObject go in visibleDuringLoad)
			DontDestroyOnLoad (go);

        gameController = GameController.GetInstance();
	}

	// Use this for initialization
	void Start () {
		timer = waitBeforeLoad;
	}
	
	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;

		if (!loading && timer <= 0) {
			loading = true;
			endTime = Time.time + loadMinDuration;
			StartCoroutine (LoadScene());
		}

		if (Time.time >= endTime)
			ao.allowSceneActivation = true;
			
	}

	IEnumerator LoadScene(){
		ao = SceneManager.LoadSceneAsync (gameController.nextLevelName);
			ao.allowSceneActivation = false;
			yield return ao;
		}
}
