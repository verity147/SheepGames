using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectatorHandler : MonoBehaviour {

    public float animPause = 100f;
    public float animVariation = 20f;
    public string randomAnim;
    public bool random = false;

    internal Animator anim;
    private AudioSource audioSource;
    private float timePassed = 0f;
    private float animPauseVaried;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        if (anim == null)
        {
            Debug.LogWarningFormat("{0} requires an Animator Component but has none!", gameObject.name);
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

    ///start this to have spectators ind the scene react(string parameter is animation trigger),
    ///don't forget to stop the coroutines if the scene is re-used for subsequent tries
    public IEnumerator Reaction(string type)
    {
        anim.SetTrigger(type);
        float counter = Random.Range(animPause - animVariation, animPause + animVariation);
        yield return new WaitForSecondsRealtime(counter);
        StartCoroutine(Reaction(type));
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
