using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private static GameController instance;

    public int numberOfEnemies = 0;
    public int maxNumberOfEnemies;
    public string nextLevelName;

    private void Start()
    {
        instance = this;
    }

    public static GameController GetInstance()
    {
        if (instance == null)
        {
            instance = new GameController();
        }

        return instance;
    }

    void Awake()
    {
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
