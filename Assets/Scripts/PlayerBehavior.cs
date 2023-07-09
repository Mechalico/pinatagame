using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D.Animation;

public class PlayerBehavior : MonoBehaviour
{
    public float walkSpeed = 500;
    public float frameRate = 60;

    public Equip pickup; 
    
    public Slider slider;

    public float heldCandy = 0.0F;

    private Rigidbody2D _body;
    private Vector2 _direction;
    private SpriteRenderer _spriteRenderer;
    private float _idleTime;
    private SpriteLibraryAsset _spriteLibraryAsset;

    private void Awake()
    {
        this._body = this.GetComponent<Rigidbody2D>();
        this._spriteRenderer = this.GetComponent<SpriteRenderer>();
        this._spriteLibraryAsset = this.GetComponent<SpriteLibrary>().spriteLibraryAsset;
    }

    private void Update()
    {
        UpdateSpriteFlip();
        UpdateSpriteFrame();
    }

    private void FixedUpdate()
    {
        UpdateVelocity();
    }

    public void AddCandy(float candyToAdd)
    {
        this.heldCandy += candyToAdd;
    }

    private float GetCandyPenalty()
    {
        return 1 - Mathf.Min(this.heldCandy / 1250, 0.8f);
    }

    private void UpdateVelocity()
    {
        var xInput = Input.GetAxisRaw("Horizontal");
        var yInput = Input.GetAxisRaw("Vertical");
        // We normalize the vector to curve the input and prevent diagonal movement having higher velocity
        this._direction = new Vector2(xInput, yInput).normalized;
        this._body.velocity = this._direction * this.walkSpeed * this.GetCandyPenalty() * Time.deltaTime;
    }

    private void UpdateSpriteFlip()
    {
        if (this._direction.x != 0)
        {
            this._spriteRenderer.flipX = this._direction.x > 0;
        }
    }

    private void UpdateSpriteFrame()
    {
        var sprites = GetSpritesForDirection();

        if (sprites == null)
        {
            this._idleTime = Time.time;
            return;
        }

        var walkTime = Time.time - this._idleTime;
        var totalFrames = (int) (walkTime * frameRate);
        var frame = totalFrames % sprites.Count;

        this._spriteRenderer.sprite = sprites[frame];
    }

    private List<Sprite> GetSpritesForDirection()
    {
        var direction = "";

        if (this._direction.y > 0) direction = "Up";
        if (this._direction.y < 0) direction = "Down";
        if (Mathf.Abs(this._direction.x) > 0.1) direction += "Side";

        return string.IsNullOrEmpty(direction) ? null : GetSprites("move" + direction);
    }

    private List<Sprite> GetSprites(string category)
    {
        return this._spriteLibraryAsset.GetCategoryLabelNames(category)
            .Select(label => this._spriteLibraryAsset.GetSprite(category, label))
            .ToList();
    }
}