using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JH_Parallax : MonoBehaviour {

    public Transform[] backgrounds;
    private float[] parallaxMagnitudes = new float[4] { 12f, 8f, 4f, 2f };
    public Transform foreground;
    public float parallaxStrength = 1f;

    private Vector3[] backgroundPositions;
    private Vector3 forgroundPos;

    private void Start()
    {
        backgroundPositions = new Vector3[backgrounds.Length];
        for (int i = 0; i < backgrounds.Length; i++)
        {
            backgroundPositions[i] = backgrounds[i].position;
        }
        forgroundPos = foreground.position;
        print(parallaxMagnitudes);
    }

    internal void MoveParallax(float cameraDistance)
    {
        Vector3 target;
        for (int i = 0; i < backgrounds.Length; i++)
        {
            target = new Vector3(backgrounds[i].position.x - cameraDistance*parallaxMagnitudes[i]*Time.deltaTime, backgrounds[i].position.y, backgrounds[i].position.z);
            //print("BG: " + background.position + " Target: " + target);
            backgrounds[i].position = Vector3.MoveTowards(backgrounds[i].position, target, parallaxStrength);
        }
        target = new Vector3(foreground.position.x - cameraDistance, foreground.position.y, foreground.position.z);
        foreground.position = Vector3.MoveTowards(foreground.position * -50f * Time.deltaTime, target, parallaxStrength);
    }

    internal void ResetPositions()
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            backgrounds[i].position = backgroundPositions[i];
        }
        foreground.position = forgroundPos;
    }
}
