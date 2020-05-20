using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anima2D;

public enum Expression { Happy, Neutral, Sad, Concerned, Shock }

public class HR_FaceManager : MonoBehaviour
{
    private HR_Player player;
    private int currentFaceIndex;

    //face change
    //GetComponent<SpriteMeshAnimation>().frame = 1;

    private void Awake()
    {
        player = GetComponentInParent<HR_Player>();
        currentFaceIndex = GetComponent<SpriteMeshAnimation>().frame;
    }

}
