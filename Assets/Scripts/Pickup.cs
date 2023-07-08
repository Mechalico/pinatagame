using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public int pickupType; //replace with enum?
    public Sprite[] sprites;

    public SpriteRenderer spriteRenderer;

    public PlayerBehavior player;


    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject != player.gameObject) return;
        //guard clauses ensure pickup is being touched by the player
        int oldPickup = player.pickup;
        player.pickup = pickupType;
        pickupType = oldPickup;
        if (oldPickup == 0) Destroy(gameObject);
        pickupSprite();
    }
    // Start is called before the first frame update
    void Start()
    {
        pickupSprite();
    }

    void pickupSprite()
    {
        if (pickupType != 0)
        {
            spriteRenderer.sprite = sprites[pickupType - 1];
        }
    }

}
