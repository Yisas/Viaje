using UnityEngine;

public class FriendController : MonoBehaviour {

	public bool isSceneChanger;
	public string levelName;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D col){
		if (col.transform.tag == "Player") {
            GameController.GetInstance().LoadNextLevel(levelName);
		}
	}
}
