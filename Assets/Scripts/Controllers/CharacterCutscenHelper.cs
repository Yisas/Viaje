using UnityEngine;
using System.Collections;

public class CharacterCutscenHelper : MonoBehaviour
{
	public GameObject[] objectsToDisable;
    public AudioSource backgroundAudioSource;
    public AudioSource busAudioSource;
    public AudioSource musicAudioSource;
    public AudioClip postCutsceneAudioClip;
    public float postCutsceneAudioClipVolume;

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

    public void StopSwimmingBackground()
    {
        backgroundAudioSource.Stop();
    }

    public void PlayBusSound()
    {
        busAudioSource.Play();
    }

	public void StartIdle(string tag){
		Animator anim = GetCharacterAnimation (tag);
		anim.SetBool ("idle", true);
	}

	public void StopIdle(string tag){
		Animator anim = GetCharacterAnimation (tag);
		anim.SetBool ("idle", false);
	}

	public void CutsceneCleanup(){

        musicAudioSource.clip = postCutsceneAudioClip;
        musicAudioSource.volume = postCutsceneAudioClipVolume;
        musicAudioSource.Play();

		foreach (GameObject go in objectsToDisable)
			go.SetActive (false);
	}

	public void TrackNewPlayer(string tag){
		cameraScript.SetCameraTarget (GetCharacter (tag).transform);
	}

	public void SwitchPlayers(){
		player.GetComponent<PlayerSwitch>().SwitchPlayers ();
	}

    public void LockPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerController>().enabled = false;
    }

    public void UnlockPlayer()
    {
        player.GetComponent<PlayerController>().enabled = true;
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
