using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour {

    public static SceneHandler sceneHandler;

    private void Awake()
    {
        if (sceneHandler == null)
        {
            sceneHandler = this;
        }
        else if (sceneHandler != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void LoadLocalizationMenu()
    {
        SceneManager.LoadSceneAsync("LocalizationChoice");
    }

    

    public void LoadLevel(string name)
    {
        SceneManager.LoadSceneAsync(name);
    }

    public static string FindActiveSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }
    
}
