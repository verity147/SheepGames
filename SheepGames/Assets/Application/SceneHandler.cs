using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour {

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadSceneAsync("MainMenu");
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
