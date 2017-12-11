using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_TriggerSound : MonoBehaviour {

    public AudioSource audioSource;
    public bool destroyAfterPlaying = true;

    private bool playing = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (playing)
            return;

        if (collision.transform.tag == "Player")
        {
            Play();
        }
    }

    private void Update()
    {
        if (playing)
        {
            if (!audioSource.isPlaying && destroyAfterPlaying)
            {
                Destroy(gameObject);
            }
        }
    }

    public void Play()
    {
        audioSource.Play();

        playing = true;
    }
}
