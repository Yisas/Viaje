using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Event_Splash : MonoBehaviour {

    public AudioSource audioSource;

	private void OnTriggerEnter2D()
    {
        audioSource.Play();
    }
}
