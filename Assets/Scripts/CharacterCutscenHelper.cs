using UnityEngine;
using System.Collections;

public class CharacterCutscenHelper : MonoBehaviour
{
	public GameObject[] objectsToDisable;

	private Animator[] anims;

	void Awake ()
	{
		anims = GetComponentsInChildren<Animator> ();
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

	private Animator GetCharacterAnimation (string tag)
	{
		foreach (Animator characterAnim in anims)
			if (characterAnim.gameObject.tag == tag)
				return characterAnim;

		return null;
	}
}
