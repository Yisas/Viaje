using UnityEngine;
using System.Collections;

public class PlayerSwitch : MonoBehaviour {

	public GameObject nextPlayer;

	private CameraTracking cameraScript;

	void Awake(){
		cameraScript = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<CameraTracking> ();
	}

	public void SwitchPlayers(){
		nextPlayer.SetActive (true);
		this.gameObject.SetActive(false);
		cameraScript.RefreshPlayerReference ();
	}

}
