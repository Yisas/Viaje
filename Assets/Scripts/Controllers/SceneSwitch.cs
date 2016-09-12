using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour {

	public string sceneName;

    // Int instead of bool because I want to call this from the animation component, which doesn't support bool (go figure)
	public void SwitchScene(int cursorVisible){
		GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ().nextLevelName = sceneName;
        Cursor.visible = (cursorVisible == 1) ? true : false; 
		SceneManager.LoadScene ("LoadingScreen");
	}

    public void SwitchScene(int cursorVisible, string sceneName)
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().nextLevelName = sceneName;
        Cursor.visible = (cursorVisible == 1) ? true : false;
        SceneManager.LoadScene("LoadingScreen");
    }
}
