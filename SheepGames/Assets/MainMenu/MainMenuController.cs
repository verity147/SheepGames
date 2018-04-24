using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuController : MonoBehaviour {

    public TMP_InputField nameInput;
    public GameObject nameInputMenu;
    public GameObject optionsMenu;
    public GameObject creditsMenu;
    public GameObject highscoreMenu;
    public GameObject confirmQuitMenu;

    private GameObject[] menus;
    private GameObject localizationManager;

    private void Awake()
    {
        localizationManager = FindObjectOfType<LocalizationManager>().gameObject;
        if(DataCollector.currentPlayer == "NONE" || DataCollector.currentPlayer == null)
        {
            SetMenuActive(nameInputMenu);
        }
    }

    private void Start()
    {
        menus = new GameObject[] { nameInputMenu, optionsMenu, creditsMenu, highscoreMenu, confirmQuitMenu };
    }

    public void SetMenuActive(GameObject menu)
    {
        if(menu.activeInHierarchy == false)
        {
            menu.SetActive(true);
        }
        if (menu == optionsMenu)
        {
            //optionsMenu.GetComponentInChildren<Slider>().name == "MusicSlider"
        }
    }

    public void BackToMainMenu()
    {
        foreach (GameObject menu in menus)
        {
            if (menu.activeInHierarchy == true)
            {
                menu.SetActive(false);
            }
        }
    }

    public void SetPlayerName()
    {
        DataCollector.currentPlayer = nameInput.text;

        if (DataCollector.currentPlayer != "NONE" && DataCollector.currentPlayer.Length >=1)
        {
            DataCollector.CheckForSaveFile();
            BackToMainMenu();
            foreach (KeyValuePair<string, Dictionary<string, int>> kvp in DataCollector.tempPlayerDict)
            {
                string player = kvp.Key;
                ICollection coll = kvp.Value;
                foreach (KeyValuePair<string, int> item in coll)
                {
                    Debug.Log("Player: " + player + "; Level: " + item.Key + ": " + item.Value);
                }
            }
        }
        else
        {
            ///put focus back to input field
            nameInput.ActivateInputField();
            Debug.LogWarning("You need to enter a name");
            return;
        }
    }

    public void ChangeLanguage(string file)
    {
        localizationManager.GetComponent<LocalizationManager>().LoadLocalization(file);
    }

    public void OpenWebsite(string url)
    {
        Application.OpenURL(url);
    }

    public void QuitGame()
    {
        if (DataCollector.tempPlayerDict != null)
        {
            SaveLoadManager.Save();
        }
        print("Quit");
        Application.Quit();
    }
}
