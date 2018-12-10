using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WinState
{
    Loss = 0,
    Win = 1,
    Neutral = 2
}

public class SpectatorHandler : MonoBehaviour {

    public AudioClip[] reactionClips;

    private Spectator[] spectators;
    private AudioSource audioSource;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        spectators = GetComponentsInChildren<Spectator>();
    }

    public void EndOfGameReaction(WinState winState)
    {
        string spectatorReaction;

        switch (winState)
        {
            case WinState.Loss:
                spectatorReaction = "sad";
                audioSource.clip = reactionClips[2];
                audioSource.Play();
                break;
            case WinState.Win:
                spectatorReaction = "cheer";
                audioSource.clip = reactionClips[0];
                audioSource.Play();
                break;
            case WinState.Neutral:
                spectatorReaction = "look";
                audioSource.clip = reactionClips[1];
                audioSource.Play();
                break;
            default:
                spectatorReaction = "look";
                audioSource.clip = reactionClips[1];
                audioSource.Play();
                break;
        }

        ///check for each spectator if they have the appropriate animation and if yes, start their coroutine loop
        foreach (Spectator spectator in spectators)
        {
            foreach (AnimatorControllerParameter param in spectator.anim.parameters)
            {
                if (param.name == spectatorReaction)
                {
                    StartCoroutine(spectator.Reaction(spectatorReaction));
                }
            }
            if (spectatorReaction != "cheer")
            {
                spectator.anim.SetBool("sadFace", true);
            }
            else if (spectatorReaction == "cheer")
            {
                spectator.anim.SetBool("sadFace", false);
            }
        }
    }
}
