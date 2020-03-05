using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_DoorSceneLoad : MonoBehaviour
{
    private AudioSource audioSource;
    public string scene;
    public SceneHandler sceneHandler;

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
        sceneHandler.LoadLevel(scene);
    }
}
