using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private static GameController instance;

    public static GameObject go;
    public int numberOfEnemies = 0;
    public int maxNumberOfEnemies;
    public string nextLevelName;

    public static GameController GetInstance()
    {
        return instance;
    }

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(transform.gameObject);
    }

    public void ReloadLevel()
    {
        numberOfEnemies = 0;
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void LoadNextLevel(string nextLevelName)
    {
        this.nextLevelName = nextLevelName;
        numberOfEnemies = 0;
        SceneManager.LoadScene("LoadingScreen");
    }
}
