using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowScoreMenu : MonoBehaviour
{
    public GameObject scoreTextObject;
    public GameObject scoreTextObject_T;
    public GameObject endOfGameMenu;
    public GameObject endOfGameMenu_T;

    private HighscoreHandler highscoreHandler;
    private PopulateHighscore populateHighscore;

    private void Awake()
    {
        highscoreHandler = FindObjectOfType<HighscoreHandler>();
    }

    public void BuildLevelEndMenu()
    {
        if (TournamentTracker.IsTournamentRunning())
        {
            endOfGameMenu_T.SetActive(true);
            populateHighscore = endOfGameMenu_T.GetComponentInChildren<PopulateHighscore>();
            scoreTextObject_T.GetComponent<TMP_Text>().text = highscoreHandler.GetHighscoreText();
        }
        else
        {
            endOfGameMenu.SetActive(true);
            populateHighscore = endOfGameMenu.GetComponentInChildren<PopulateHighscore>();
            scoreTextObject.GetComponent<TMP_Text>().text = highscoreHandler.GetHighscoreText();
        }
        populateHighscore.NewGrid();
        populateHighscore.NewLevelScore(SceneHandler.GetSceneName());
    }
}
