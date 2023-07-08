using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    public Rigidbody2D body;

    public SpriteRenderer spriteRenderer;

    public List<Sprite> northSprites;
    public List<Sprite> northEastSprites;
    public List<Sprite> eastSprites;
    public List<Sprite> southEastSprites;
    public List<Sprite> southSprites;

    public float walkSpeed;
    public float frameRate;
    public Vector2 direction;

    public Equip pickup; 

    public float heldCandy = 0.0F;
    public int pickup = 0;

    private float _idleTime;

    void Update()
    {
        UpdateSpriteFlip();
        UpdateSpriteFrame();
    }

    void FixedUpdate()
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
        this.direction = new Vector2(xInput, yInput).normalized;
        body.velocity = this.direction * this.walkSpeed * this.GetCandyPenalty() * Time.deltaTime;
    }

    private void UpdateSpriteFlip()
    {
        if (this.direction.x != 0)
        {
            spriteRenderer.flipX = this.direction.x < 0;
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

        spriteRenderer.sprite = sprites[frame];
    }

    private List<Sprite> GetAnimationSprites()
    {
        var movingHorizontally = Mathf.Abs(this.direction.x) > 0;

        if (this.direction.y > 0)
        {
            return movingHorizontally ? this.northEastSprites : this.northSprites;
        }

        if (this.direction.y < 0)
        {
            return movingHorizontally ? this.southEastSprites : this.southSprites;
        }

        return movingHorizontally ? this.eastSprites : null;
    }
}
