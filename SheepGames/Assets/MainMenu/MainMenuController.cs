using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuController : MonoBehaviour {

    public TMP_InputField nameInput;
    public GameObject nameInputObject;


    public void SetPlayerName()
    {
        DataCollector.currentPlayer = nameInput.text;
        DataCollector.CheckForSaveFile();
        nameInputObject.SetActive(false);
    }


}
