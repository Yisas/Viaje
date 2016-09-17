using UnityEngine;
using System.Collections;

public class PrefabSpawner : MonoBehaviour {

	public GameObject parent;
	public GameObject prefab;

	void Awake()
	{
        Object go;
        GameObject temp;

        if (parent)
        {
            go = Instantiate(prefab, parent.transform);
            temp = (GameObject)go;
            temp.transform.position = new Vector3(0, 0, 0);

        }
        else
            go = Instantiate(prefab, transform.position, transform.rotation);

		temp = (GameObject)go;
		temp.transform.localScale = transform.localScale;

       	Destroy( gameObject );
	}
}
