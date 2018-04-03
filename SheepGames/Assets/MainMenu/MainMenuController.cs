using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuController : MonoBehaviour {

    public TMP_InputField nameInput;
    public GameObject nameInputObject;
    public GameObject optionsMenu;
    public GameObject creditsMenu;
    public GameObject highscoreMenu;
    private GameObject[] menus;

    private void Start()
    {
        menus = new GameObject[] { nameInputObject, optionsMenu, creditsMenu, highscoreMenu };
    }

    public void SetMenuActive(GameObject menu)
    {
        if(menu.activeInHierarchy == false)
        {
            menu.SetActive(true);
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
        DataCollector.CheckForSaveFile();
        nameInputObject.SetActive(false);
    }


}
