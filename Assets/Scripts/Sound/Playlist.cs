using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playlist : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] clips;
    public float[] clipVolumes;

    protected int clipIndex = -1;
    protected int volumeIndex = -1;

    // Update is called once per frame
    void Update()
    {
        if (!audioSource.isPlaying)
        {
            PlayClip();
        }
    }

    protected virtual void PlayClip()
    {
        clipIndex++;
        volumeIndex++;

        if (clipIndex > clips.Length - 1)
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
