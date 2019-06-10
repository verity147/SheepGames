using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PP_UIHandler : MonoBehaviour {

    public GameObject scoreTextObject;
    public GameObject endOfGameMenu;

    private HighscoreHandler highscoreHandler;
    private PopulateHighscore populateHighscore;

    private void Start()
    {
        highscoreHandler = FindObjectOfType<HighscoreHandler>();
        populateHighscore = GetComponentInChildren<PopulateHighscore>(true);
    }

    public void BuildLevelEndMenu()
    {
        endOfGameMenu.SetActive(true);
        populateHighscore.NewGrid();
        populateHighscore.NewLevelScore(SceneHandler.GetSceneName());
        scoreTextObject.GetComponent<TMP_Text>().text = highscoreHandler.GetHighscoreText();
    }
}
