using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_DoorSceneLoad : MonoBehaviour
{
    private AudioSource audioSource;
    public string scene;
    public SceneHandler sceneHandler;
    public TournamentTracker tournamentTracker;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void StartWaitRoutine()
    {
        StartCoroutine(WaitForSound());
    }

    IEnumerator WaitForSound()
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        if (TournamentTracker.IsTournamentRunning())
        {
            tournamentTracker.NextLevel();
        }
        else
        {
            sceneHandler.LoadLevel(scene);
        }
    }
}
