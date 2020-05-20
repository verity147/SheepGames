using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HR_SpringManager : MonoBehaviour
{
    public HR_Player player;
    private List<HR_Spring> springs;

    private void Awake()
    {
        springs = new List<HR_Spring>(GetComponentsInChildren<HR_Spring>());
    }

    private void Start()
    {
        foreach (HR_Spring spring in springs)
        {
            spring.player = player;
        }
    }
}
