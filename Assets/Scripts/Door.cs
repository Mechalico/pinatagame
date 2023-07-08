using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool RequiresKey = false;
    public bool inRange;
    
    public SpriteRenderer spriteRenderer;
    public Equip key;
    public PlayerBehavior player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!inRange) return;
        if (!CanOpen()) return;
        if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            Destroy(gameObject);
        }
    }
    
    private bool CanOpen(){
        if (!RequiresKey) return true;
        return player.pickup == key;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        //guard clause to ensure door is touched by player
        if (other.gameObject != player.gameObject) return;
        
        if (CanOpen()){
            spriteRenderer.color = Color.cyan;
        }else{
            spriteRenderer.color = Color.red;
        }
        inRange = true;
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        //guard clause to ensure door is touched by player
        if (other.gameObject != player.gameObject) return;
        
        spriteRenderer.color = new Color(0.4f,0.4f,0.4f);
        inRange = false;
    }
}
