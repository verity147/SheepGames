using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HR_DrinkCheck : MonoBehaviour
{
    private HR_Player player;
    private Collider2D myCollider;

    private void Awake()
    {
        player = GetComponentInParent<HR_Player>();
        myCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        player.drinkingAllowed = myCollider.IsTouchingLayers(LayerMask.GetMask("HR_Troughs"));
    }
}
