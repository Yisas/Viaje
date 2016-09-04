using UnityEngine;
using System.Collections;

public class CharacterCutscenHelper : MonoBehaviour
{
	public GameObject[] objectsToDisable;

	private Animator[] anims;
	private GameObject player;
	private CameraTracking cameraScript;

	void Awake ()
	{
		anims = GetComponentsInChildren<Animator> ();
		player = GameObject.FindGameObjectWithTag ("Player");
		cameraScript = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<CameraTracking> ();
	}

	//-------------------------------------------------------------------------------------
	public void StartRunning (string tag)
	{
		Animator anim = GetCharacterAnimation (tag);
		anim.SetBool ("running", true);
	}
		
	public void StopRunning (string tag)
	{
		Animator anim = GetCharacterAnimation (tag);
		anim.SetBool ("running", false);
	}
		
	public void StopSwimming (string tag)
	{
		Animator anim = GetCharacterAnimation (tag);
		anim.SetTrigger ("stopSwimming");
	}

	public void CutsceneCleanup(){
		foreach (GameObject go in objectsToDisable)
			go.SetActive (false);
	}

	public void TrackNewPlayer(string tag){
		cameraScript.SetCameraTarget (GetCharacter (tag).transform);
	}

	public void SwitchPlayers(){
		player.GetComponent<PlayerSwitch>().SwitchPlayers ();
	}

	public void HideActivePlayer(){
		player.GetComponent<PlayerController> ().HideCharacter ();
	}

	private Animator GetCharacterAnimation (string tag)
	{
		foreach (Animator characterAnim in anims)
			if (characterAnim.gameObject.tag == tag)
				return characterAnim;

		return null;
	}

	private GameObject GetCharacter(string tag){
		foreach (Animator characterAnim in anims)
			if (characterAnim.gameObject.tag == tag)
				return characterAnim.gameObject;

		return null;
	}
}
