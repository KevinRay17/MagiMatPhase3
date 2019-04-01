using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpriteManager : MonoBehaviour
{

    
    // Frameworks for player animation 
    
    
    
    
    private SpriteRenderer playerSprite;
    
    
    public Sprite spriteMoveRight;
    public Sprite spriteMoveLeft;
    public Sprite spriteDashRight;
    public Sprite spriteDashLeft;
    public Sprite spritePlayerJump;
    

    public void Start(){
       playerSprite = GetComponent<SpriteRenderer>();
    }
    
    //TO ADD ANY ANIMATION TO ANYTHING IN PLAYERMOVEMENT.CS
    // USE:   animate.FunctionNameInThisScript();
    
    public void AnimatePlayerMoveRight()
    {
        //Replace this with animation
        playerSprite.sprite = spriteMoveRight;
    }
    public void AnimatePlayerMoveLeft()
    {
        //Replace this with animation
        playerSprite.sprite = spriteMoveLeft;
    }
    public void AnimatePlayerDashRight()
    {
        //replace this with animation
        playerSprite.sprite = spriteDashRight;
    }
    public void AnimatePlayerDashLeft()
    {
        //replace this with animation
        playerSprite.sprite = spriteDashLeft;
    }
    public void AnimatePlayerJump()
    {
        //replace this with animation
        playerSprite.sprite = spritePlayerJump;
    }
}
