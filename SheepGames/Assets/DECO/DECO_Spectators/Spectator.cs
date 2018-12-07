using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spectator : MonoBehaviour {

    public float animPause = 10f;
    public float animVariation = 5f;
    public string randomAnim;
    public bool random = false;
    public bool doesSound = false;

    internal Animator anim;
    private AudioSource audioSource;
    private float timePassed = 0f;
    private float animPauseVaried;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        if (anim == null)
        {
            Debug.LogWarningFormat("{0} requires an Animator Component but has none!", gameObject.name);
        }
        if (doesSound)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                Debug.LogWarningFormat("{0} requires an Audio Source but has none!", gameObject.name);
            }
        }
        animPauseVaried = animPause;
    }

    private void Update()
    {
        if (anim == null)
            return;

        if (random)
        {
            ///trigger animations when animPause in seconds has passed
            if(timePassed >= animPauseVaried)
            {
                anim.SetTrigger(randomAnim);
                timePassed = 0f;
                animPauseVaried = Random.Range(animPause - animVariation, animPause + animVariation);
            }
        }
        timePassed += Time.deltaTime;
    }

    ///start this to have spectators in the scene react(string parameter is animation trigger),
    ///don't forget to stop the coroutines if the scene is re-used for subsequent tries
    public IEnumerator Reaction(string type)
    {
        anim.SetTrigger(type);
        float counter = Random.Range(animPause, animPause + animVariation);
        yield return new WaitForSecondsRealtime(counter);
        StartCoroutine(Reaction(type));
    }

    ///called from animation event
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
