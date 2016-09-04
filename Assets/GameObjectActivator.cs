using UnityEngine;
using System.Collections;

public class GameObjectActivator : MonoBehaviour {

	public GameObject go;

	void OnTriggerEnter2D(Collider2D col){
		if (col.transform.tag == "Player")
			go.SetActive(true);
	}
}
