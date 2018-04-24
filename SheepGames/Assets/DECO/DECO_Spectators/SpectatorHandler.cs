using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectatorHandler : MonoBehaviour {

    public float animPause = 100f;
    public string randomAnim;
    public string cheerAnim;
    public string sadAnim;
    public string lookAnim;
    public bool random = false;

    private Animator anim;
    private AudioSource audioSource;
    private float timePassed = 0f;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        if (anim == null)
        {
            Debug.LogWarningFormat("{0} requires an Animator Component but has none!", gameObject.name);
        }
    }

    private void Update()
    {
        if (anim == null)
            return;

        if (random)
        {
            ///trigger animations when animPause in seconds has passed
            if(timePassed >= animPause)
            {
                anim.SetTrigger(randomAnim);
                timePassed = 0f;
                animPause = Random.Range(animPause - 20f, animPause + 20f);
            }
        }
        timePassed += Time.deltaTime;
    }

    public void PlaySound()
    {
        audioSource.Play();
    }

}
