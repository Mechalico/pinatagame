using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public Equip pickupType;

    public SpriteRenderer spriteRenderer;

    public PlayerBehavior player;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject != player.gameObject) return;
        //guard clauses ensure pickup is being touched by the player
        Equip oldPickup = player.pickup;
        player.pickup = pickupType;
        pickupType = oldPickup;
        if (oldPickup == null) Destroy(gameObject);
        pickupSprite();
    }
    
    void Start()
    {
        pickupSprite();
    }

    void pickupSprite()
    {
        spriteRenderer.sprite = pickupType.sprite;
    
    }

}
