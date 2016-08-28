using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class FriendController : MonoBehaviour {

	public bool isSceneChanger;
	public string leveName;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D col){
		if (col.transform.tag == "Player")
			SceneManager.LoadScene (leveName);
	}
}
