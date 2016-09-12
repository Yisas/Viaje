using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

    public void Open()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0;

        Cursor.visible = true;
        AudioSource[] audioS = Object.FindObjectsOfType<AudioSource>();
        foreach (AudioSource audio in audioS)
            audio.Pause();
    }

    public void Close()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
        Cursor.visible = false;

        AudioSource[] audioS = Object.FindObjectsOfType<AudioSource>();
        foreach (AudioSource audio in audioS)
            audio.UnPause();
    }
}
