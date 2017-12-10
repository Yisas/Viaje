using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPlaylist : Playlist
{
    protected override void PlayClip()
    {
        // Choose randomly
        int randomIndex = Random.Range(0, clips.Length - 1);

        audioSource.clip = clips[randomIndex];
        audioSource.volume = clipVolumes[randomIndex];
        audioSource.Play();
    }
}
