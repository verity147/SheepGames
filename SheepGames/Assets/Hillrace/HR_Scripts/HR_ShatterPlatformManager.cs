using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HR_ShatterPlatformManager : MonoBehaviour
{
    public HR_Player player;
    private List<HR_ShatterPlatform> platforms;

    private void Awake()
    {
        platforms = new List<HR_ShatterPlatform>(GetComponentsInChildren<HR_ShatterPlatform>());
    }

    private void Start()
    {
        foreach (HR_ShatterPlatform platform in platforms)
        {
            platform.player = player;
        }
    }
}
