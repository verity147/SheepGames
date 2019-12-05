using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HR_ParallaxScroll : MonoBehaviour
{
    public Renderer background;
    public Renderer foreground;
    public GameObject mainCam;
    public float backgroundSpeed = 0.02f;
    public float foregroundSpeed = 0.06f;
    public float backgroundHeight = 0.001f;
    public float foregroundHeight = 0.002f;

    private float offsetX = 0f;
    private float offsetY = 0f;
    private float startHeight;

    private void Start()
    {
        startHeight = transform.position.y;
    }

    private void Update()
    {
        offsetX = mainCam.transform.position.x;
        offsetY = mainCam.transform.position.y;

        float backgroundOffset = offsetX * backgroundSpeed;
        float foregroundOffset = offsetX * foregroundSpeed;

        float backgroundYoffset = offsetY * backgroundHeight;
        float foregroundYoffset = offsetY * foregroundHeight;

        background.material.mainTextureOffset = new Vector2(backgroundOffset, backgroundYoffset);
        foreground.material.mainTextureOffset = new Vector2(foregroundOffset, foregroundYoffset);
    }
}
