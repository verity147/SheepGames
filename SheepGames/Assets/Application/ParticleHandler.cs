﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHandler : MonoBehaviour {

    private ParticleSystem partSys;

    private void Awake()
    {
        partSys = GetComponent<ParticleSystem>();
    }

    public void RunParticleSystem()
    {
        if (partSys.isPlaying)
        {
            partSys.Stop();
        }
        partSys.Play();
    }
}
