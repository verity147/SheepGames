using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TournamentTracker : MonoBehaviour
{
    public static bool tournament = false; //set index to 0 when reset to false
    public SceneHandler sceneHandler;
    private int tournamentIndex = 0;
    private readonly string[] tournamentOrder = { "JH_GameLV_01", "JH_GameLV_02", "JH_GameLV_03",
                                                         "PW_GameLV_01", "PW_GameLV_02", "PW_GameLV_03",
                                                         "PP_GameLV_00", "PP_GameLV_01", "PP_GameLV_02",
                                                         "PP_GameLV_03", "HR_GameLV_01", "05_Party" };

    public void RunTournament(bool run)
    {
        tournament = run;
    }


    public static bool IsTournamentRunning()
    {
        return tournament;
    }

    public void NextLevel()
    {
        sceneHandler.LoadLevel(tournamentOrder[tournamentIndex]);
        tournamentIndex++;
    }
}
