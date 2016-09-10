using UnityEngine;
using System.Collections;

public class QuitOnClick : MonoBehaviour {

	public void Quit(){
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false; // Exit playmode
		#else
		Application.Quit();
		#endif
	}
}
