using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour {

    public bool go;

    private void Update()
    {
        if (go)
        {
            Group[] dudes = FindObjectsOfType<Group>();
            foreach(Group dude in dudes)
            {
                dude.CallThisDude();
            }
        }
    }
}
