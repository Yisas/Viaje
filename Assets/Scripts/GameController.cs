using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	[HideInInspector]
	public int numberOfEnemies = 0;
	public int maxNumberOfEnemies;
	public string nextLevelName;

	void Awake(){
		DontDestroyOnLoad (transform.gameObject);
	}
}
