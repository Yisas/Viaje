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
			StartCoroutine (LoadNextSceneAsync());
		}

        /*
		if (Time.time >= endTime)
			ao.allowSceneActivation = true;
        */
			
	}

    IEnumerator LoadNextSceneAsync()
    {

        // The Application loads the Scene in the background at the same time as the current Scene.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(gameController.nextLevelName);

        //Wait until the last operation fully loads to return anything
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
