using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public Equip pickupType;
    public SpriteRenderer spriteRenderer;

    private PlayerBehavior _player;

    private void Start()
    {
        var player = GameObject.FindGameObjectWithTag("Player");

        if (player?.GetComponent<PlayerBehavior>() != null)
        {
            this._player = player.GetComponent<PlayerBehavior>(); 
        }

        RefreshSprite();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (this._player.gameObject != other.gameObject)
        {
            return;
        }

        var existingType = this._player.pickup;

        this._player.pickup = this.pickupType;

        if (existingType != null)
        {
            this.pickupType = existingType;
        }
        else
        {
            Destroy(this.gameObject);
        }

        this.RefreshSprite();
    }

    void RefreshSprite()
    {
        this.spriteRenderer.sprite = this.pickupType.sprite;
    }
}
