using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HR_TroughManager : MonoBehaviour
{
    public HR_Player player;

    private List<HR_Trough> troughs;

    private void Awake()
    {
        troughs = new List<HR_Trough>(GetComponentsInChildren<HR_Trough>());
    }

    private void Start()
    {
        foreach (HR_Trough trough in troughs)
        {
            trough.player = player;
        }
    }

}
