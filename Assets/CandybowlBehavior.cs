using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandybowlBehavior : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public float fullness;
    public float depletionRate;
    public bool empty = false;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer.color = Color.magenta;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!empty)
        {
            spriteRenderer.color = Color.red;
        }
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
        if (!empty)
        {
            spriteRenderer.color = Color.magenta;
        }
    }
}
