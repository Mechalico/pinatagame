using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CandybowlBehavior : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public float fullness;
    public float depletionRate;
    public bool empty = false;
    public bool taking = false;
    public Slider slider;
    public PlayerBehavior player;

    void Start()
    {
        spriteRenderer.color = Color.magenta;
    }

    void FixedUpdate()
    {
        if (taking)
        {
            player.heldCandy += Deplete();
            slider.value = fullness / 100f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (empty) return;
        if (collider.gameObject != player.gameObject) return;
        //guard clauses ensure bowl is being touched by the player
        spriteRenderer.color = Color.red;
        slider.gameObject.SetActive(true);
        taking = true;
        
    }

    public float Deplete()
    {
        float toAdd;

        if (fullness < depletionRate)
        {
            toAdd = fullness;
            fullness = 0;
            spriteRenderer.color = Color.gray;
            empty = true;
        }
        else
        {
            toAdd = depletionRate;
            fullness -= depletionRate;
        }
        return toAdd;
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject != player.gameObject) return;
        //player left this bowl
        if (!empty) spriteRenderer.color = Color.magenta;
        slider.gameObject.SetActive(false);
        taking = false;
    }
}
