using UnityEngine;
using System.Collections;

public class PrefabSpawner : MonoBehaviour {

	public GameObject prefab;

	void Awake()
	{
		Object go = Instantiate(prefab,transform.position,transform.rotation);
		GameObject temp = (GameObject)go;
		temp.transform.localScale = transform.localScale;
		Destroy( gameObject );
	}
}
