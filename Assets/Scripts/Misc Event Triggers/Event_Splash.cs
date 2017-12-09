using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Event_Splash : MonoBehaviour {

    public AudioSource localAudioSource;
    public AudioSource backgroundAudioSource;

    private void OnTriggerEnter2D()
    {
        localAudioSource.Play();
        backgroundAudioSource.Play();
    }
}
