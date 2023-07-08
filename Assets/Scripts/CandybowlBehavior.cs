using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CandybowlBehavior : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public float maxFullness;
    public float fullness;
    public float depletionRate;
    public bool empty = false;
    bool readyToTake = false;
    bool taking = false;
    // public Slider slider;
    PlayerBehavior player;

    void Start()
    {
        fullness = maxFullness;
        spriteRenderer.color = Color.magenta;
        var playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null && playerObject.GetComponent<PlayerBehavior>() != null )
            player = playerObject.GetComponent<PlayerBehavior>();
    }

    private void Update()
    {
        if (readyToTake && !empty)
        {
            if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.JoystickButton0))
            {
                spriteRenderer.color = Color.blue;
                taking = true;
            }
            if (Input.GetKeyUp(KeyCode.J) || Input.GetKeyUp(KeyCode.JoystickButton0))
            {
                spriteRenderer.color = Color.red;
                taking = false;
            }
        }
    }

    void FixedUpdate()
    {
        if (empty) { return; }
        if (taking)
        {
            player.AddCandy(Deplete(depletionRate * Time.deltaTime));
            //slider.value = fullness / 100f;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //guard clause to ensure bowl is being touched by the player
        if (other.gameObject == player.gameObject)
        {
            if (!empty)
            {
                spriteRenderer.color = Color.red;
                readyToTake = true;
                //slider.gameObject.SetActive(true);
            }
        }

    }

    public float Deplete(float amountToDeplete)
    {
        float toAdd;

        if (fullness < amountToDeplete)
        {
            toAdd = fullness;
            fullness = 0;
            empty = true;
            spriteRenderer.color = Color.gray;
            //slider.gameObject.SetActive(false);
        }
        else
        {
            toAdd = amountToDeplete;
            fullness -= amountToDeplete;
        }
        return toAdd;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject != player.gameObject) return;
        //player left this bowl

        if (!empty) spriteRenderer.color = Color.magenta;
        // slider.gameObject.SetActive(false);
        readyToTake = false;
        taking = false;
    }
}
