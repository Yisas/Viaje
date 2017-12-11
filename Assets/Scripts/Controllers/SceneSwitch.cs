using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour {

	public string sceneName;

    // Int instead of bool because I want to call this from the animation component, which doesn't support bool (go figure)
	public void SwitchScene(int cursorVisible){
        Cursor.visible = (cursorVisible == 1) ? true : false;
        GameController.GetInstance().LoadNextLevel(sceneName);
	}

    public void SwitchScene(int cursorVisible, string sceneName)
    {
        Cursor.visible = (cursorVisible == 1) ? true : false;
        GameController.GetInstance().LoadNextLevel(sceneName);
    }

    public void SplashCreenSwitch()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
