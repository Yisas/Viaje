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
    protected bool paused = false;

    // Update is called once per frame
    void Update()
    {
        if (!audioSource.isPlaying)
        {
            PlayClip();
        }
    }

    public void Pause()
    {
        paused = true;
        audioSource.Pause();
    }

    public void UnPause()
    {
        paused = false;
        audioSource.UnPause();
    }

    protected virtual void PlayClip()
    {
        if (paused)
            return;

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
