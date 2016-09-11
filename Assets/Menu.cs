using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

    public void Open()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void Close()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
}
