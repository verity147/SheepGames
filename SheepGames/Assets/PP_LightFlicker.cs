using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_LightFlicker : MonoBehaviour {

    public Light myLight;

    [Range(0.1f, 2f)]
    public float minIntensity = 0.1f;

    [Range(0.1f, 5f)]
    public float maxIntensity = 2f;

    public float flickerSpeed;

    //file:///C:/Program%20Files/Unity/Editor/Data/Documentation/en/ScriptReference/Mathf.Lerp.html

    private void Update()
    {
        if (myLight.intensity<=minIntensity)
        {
            print("smol");
            myLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, flickerSpeed);
        }
        else if (myLight.intensity >= maxIntensity)
        {
            print("big");
            myLight.intensity = Mathf.Lerp(maxIntensity, minIntensity, flickerSpeed);
        }
    }
}
