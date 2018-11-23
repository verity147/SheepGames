using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour {

    public static string GetSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }

    public void LoadLocalizationMenu()
    {
        SceneManager.LoadSceneAsync("03_LocalizationChoice");
    }    

    public void LoadLevel(string name)
    {
        SceneManager.LoadSceneAsync(name);
    }
   
}
