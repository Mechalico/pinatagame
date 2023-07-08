using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitBehavior : MonoBehaviour
{
    public PlayerBehavior player;
    public bool victory = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var complete = true;
        if (other.gameObject == player.gameObject)
        {
            var Candybowls = GameObject.FindGameObjectsWithTag("Candybowl");
            if (Candybowls != null)
            {
                foreach (var candybowl in Candybowls)
                {
                    var script = candybowl.GetComponent<CandybowlBehavior>();
                    if (script != null)
                        if (!script.empty)
                            complete = false;
                }
            }

            if (complete)
            {
                victory = true; 
            }

        }
    }

}
