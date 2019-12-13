using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HR_PlayerCanvas : MonoBehaviour
{
    internal float fillValue = 0f;

    internal HR_Player player;
    internal Slider drinkMeter;
    internal bool visible;

    private void Awake()
    {
        drinkMeter = GetComponentInChildren<Slider>();
        player = GetComponentInParent<HR_Player>();
    }

    private void Start()
    {
        drinkMeter.gameObject.SetActive(false);
    }

    internal void FlipDrinkBar()
    {
        if (IsTurned())
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    private bool IsTurned()
    {
        return player.transform.lossyScale.x < 0 && transform.localScale.x > 0 ||
               player.transform.lossyScale.x > 0 && transform.localScale.x < 0;
    }

}
