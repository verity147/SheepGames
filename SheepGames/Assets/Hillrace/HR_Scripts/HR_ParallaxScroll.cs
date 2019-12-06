using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HR_ParallaxScroll : MonoBehaviour
{
    public GameObject mainCam;

    public List<HR_ParallaxLayer> parallaxLayers = new List<HR_ParallaxLayer>();

    private float camOffsetX = 0f;
    private float camOffsetY = 0f;

    private void Update()
    {
        camOffsetX = mainCam.transform.position.x;
        camOffsetY = mainCam.transform.position.y;

        foreach(HR_ParallaxLayer layer in parallaxLayers)
        {
            float horizOffset = layer.horizontalSpeed * camOffsetX;
            float vertOffset = layer.verticalSpeed * camOffsetY;
            layer.rend.material.mainTextureOffset = new Vector2(horizOffset, vertOffset);
        }
    }
}

[System.Serializable]
public class HR_ParallaxLayer
{
    public string name;
    public Renderer rend;
    public float horizontalSpeed;
    public float verticalSpeed;

    public HR_ParallaxLayer(string newName, Renderer newRend, float newHorizontalSpeed, float newVerticalSpeed)
    {
        name = newName;
        rend = newRend;
        horizontalSpeed = newHorizontalSpeed;
        verticalSpeed = newVerticalSpeed;
    }
}


