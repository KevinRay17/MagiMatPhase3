using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpriteManager : MonoBehaviour
{

    private SpriteRenderer playerSprite;

    public void Start(){
       playerSprite = GetComponent<SpriteRenderer>();
    }
    
    //TO ADD ANY ANIMATION TO ANYTHING IN PLAYERMOVEMENT.CS
    // USE:   animate.FunctionNameInThisScript();
    
    public void AnimatePlayerMoveRight()
    {
        //Replace this with animation
       
    }
    public void AnimatePlayerMoveLeft()
    {
        //Replace this with animation
       
    }
    public void AnimatePlayerDashRight()
    {
        //replace this with animation
       
    }
    public void AnimatePlayerDashLeft()
    {
        //replace this with animation
       
    }
    public void AnimatePlayerJump()
    {
        //replace this with animation
       
    }
}
