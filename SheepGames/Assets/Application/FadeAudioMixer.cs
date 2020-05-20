using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// comes from Method No 2 on https://gamedevbeginner.com/how-to-fade-audio-in-unity-i-tested-every-method-this-ones-the-best/#second_method
/// </summary>

public static class FadeMixerGroup
{

    public static IEnumerator StartFade(AudioMixer audioMixer, string exposedParam, float duration, float targetVolume)
    {
        float currentTime = 0f;
        audioMixer.GetFloat(exposedParam, out float currentVol);
        currentVol = Mathf.Pow(10f, currentVol / 20f);
        float targetValue = Mathf.Clamp(targetVolume, 0.0001f, 1f);

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float newVol = Mathf.Lerp(currentVol, targetVolume, currentTime / duration);
            audioMixer.SetFloat(exposedParam, Mathf.Log10(newVol) * 20f);
            yield return null;
        }
        yield break;
    }
}
