using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour {

	public string sceneName;

	public void SwitchScene(){
		GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ().nextLevelName = sceneName;
		SceneManager.LoadScene ("LoadingScreen");
	}
}
