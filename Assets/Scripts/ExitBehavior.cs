using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitBehavior : MonoBehaviour
{
    PlayerBehavior player;
    public bool victory = false;
    public GameObject go;
    public GameObject winrar;

    // Start is called before the first frame update
    void Start()
    {
        var playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null && playerObject.GetComponent<PlayerBehavior>() != null)
            player = playerObject.GetComponent<PlayerBehavior>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public bool AllBowlsDone(){
        var complete = true;
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
        return complete;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == player.gameObject)
        {

            if (AllBowlsDone())
            {
                victory = true; 
                winrar.SetActive(true);
            }

        }
    }

}
