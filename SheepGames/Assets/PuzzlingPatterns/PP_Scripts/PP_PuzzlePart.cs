using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new puzzle part", menuName = "VS_PuzzlePart")]
public class PP_PuzzlePart : ScriptableObject {

    //attributes
    public Vector2 correctPosition;
    public bool isPlaced = false;
    public Sprite picture;
    public int positionsTried = 0;
	
    
}
