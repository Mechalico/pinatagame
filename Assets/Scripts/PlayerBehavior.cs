using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    public List<Sprite> northSprites;
    public List<Sprite> northEastSprites;
    public List<Sprite> eastSprites;
    public List<Sprite> southEastSprites;
    public List<Sprite> southSprites;

    public float walkSpeed;
    public float frameRate;

    public Equip pickup; 

    public float heldCandy = 0.0F;

    private Rigidbody2D _body;
    private Vector2 _direction;
    private SpriteRenderer _spriteRenderer;
    private float _idleTime;

    private void Awake()
    {
        this._body = this.GetComponent<Rigidbody2D>();
        this._spriteRenderer = this.GetComponent<SpriteRenderer>();
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
            this._spriteRenderer.flipX = this._direction.x < 0;
        }
    }

    private void UpdateSpriteFrame()
    {
        var sprites = GetAnimationSprites();

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

    private List<Sprite> GetAnimationSprites()
    {
        var movingHorizontally = Mathf.Abs(this._direction.x) > 0;

        if (this._direction.y > 0)
        {
            return movingHorizontally ? this.northEastSprites : this.northSprites;
        }

        if (this._direction.y < 0)
        {
            return movingHorizontally ? this.southEastSprites : this.southSprites;
        }

        return movingHorizontally ? this.eastSprites : null;
    }
}
