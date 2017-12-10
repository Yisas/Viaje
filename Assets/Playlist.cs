using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playlist : MonoBehaviour {

    public AudioSource audioSource;
    public AudioClip[] clips;
    public float[] clipVolumes;

    private int clipIndex = 0;
    private int volumeIndex = 0;
	
	// Update is called once per frame
	void Update () {
        if (!audioSource.isPlaying)
        {
            clipIndex++;
            volumeIndex++;

            if(clipIndex > clips.Length - 1)
            {
                clipIndex = 0;
            }

            if (volumeIndex > clipVolumes.Length - 1)
            {
                volumeIndex = 0;
            }

            audioSource.clip = clips[clipIndex];
            audioSource.volume = clipVolumes[volumeIndex];
            audioSource.Play();
        }
	}
}
