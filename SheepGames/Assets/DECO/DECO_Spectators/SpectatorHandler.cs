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
    private AudioSource decoAudio;


    private void Awake()
    {
        decoAudio = GetComponent<AudioSource>();
        spectators = GetComponentsInChildren<Spectator>();
    }

    public void EndOfGameReaction(WinState winState)
    {
        string spectatorReaction;

        switch (winState)
        {
            case WinState.Loss:
                spectatorReaction = "sad";
                decoAudio.clip = reactionClips[2];
                decoAudio.Play();
                break;
            case WinState.Win:
                spectatorReaction = "cheer";
                decoAudio.clip = reactionClips[0];
                print("start sound");
                decoAudio.Play();
                break;
            case WinState.Neutral:
                spectatorReaction = "look";
                decoAudio.clip = reactionClips[1];
                decoAudio.Play();
                break;
            default:
                spectatorReaction = "look";
                decoAudio.clip = reactionClips[1];
                decoAudio.Play();
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
