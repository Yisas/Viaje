using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedAudioSourcePlay : MonoBehaviour {

    public AudioSource audioSource;
    public float timerCountdown;

	// Update is called once per frame
	void Update () {
        timerCountdown -= Time.deltaTime;
        
        if(timerCountdown <=0)
        {
            audioSource.Play();
            Destroy(this);
        }
	}
}
