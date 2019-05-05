using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    [Tooltip("DO NOT CHECK if not cape object")]
    public bool cape;

    [Tooltip("DO NOT CHECK if not collar object")]
    public bool collar;


    [Header("Colors for cape")]
    public Color baseColor;
    public Color fireColor;
    public Color vineColor;
    public Color rockColor;

    [Header("Animators for collar")]
    //public AnimatorOverrideController blankController;
    public AnimatorOverrideController fireCollarController;
    public AnimatorOverrideController vineCollarController;
    public AnimatorOverrideController rockCollarController;
    public Color transparent;
    public Color solid;

    private Animator _anim;
    private Animator _parentAnim;
    private Rigidbody2D _rb2d;
    private SpriteRenderer _sr;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _sr = GetComponent<SpriteRenderer>();
        if (transform.parent != null)
        {
            _rb2d = GetComponentInParent<Rigidbody2D>();
            _parentAnim = transform.parent.GetComponent<Animator>();
        }
        else
            _rb2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //please kill me
        if (_parentAnim != null)
        {
            _anim.SetBool("Fire", _parentAnim.GetBool("Fire"));
            _anim.SetBool("Vine", _parentAnim.GetBool("Vine"));
            _anim.SetBool("Rock", _parentAnim.GetBool("Rock"));
        }

        //CAPE COLOR
        if (cape)
        {
            if (_anim.GetBool("Fire"))
                _sr.color = fireColor;
            else if (_anim.GetBool("Vine"))
                _sr.color = vineColor;
            else if (_anim.GetBool("Rock"))
                _sr.color = rockColor;
            else
                _sr.color = baseColor;
        }

        //COLLAR ANIMATOR

        if (collar && _parentAnim != null)
        {
            if (_anim.GetBool("Fire"))
                _anim.runtimeAnimatorController = fireCollarController;
            else if (_anim.GetBool("Vine"))
                _anim.runtimeAnimatorController = vineCollarController;
            else if (_anim.GetBool("Rock"))
                _anim.runtimeAnimatorController = rockCollarController;
        }

        if (collar)
        {
            if (_anim.GetBool("Fire") || _anim.GetBool("Vine") || _anim.GetBool("Rock"))
                _sr.color = solid;
            else
                _sr.color = transparent;
        }


        
        //checking for current material and changing sprite
        if (PlayerManager.instance.material == Material.None)
        {
            _anim.SetBool("Fire", false);
            _anim.SetBool("Vine", false);
            _anim.SetBool("Rock", false);
        }
        else if (PlayerManager.instance.material == Material.Vine)
        {
            _anim.SetBool("Vine", true);
            _anim.SetBool("Fire", false);
            _anim.SetBool("Rock", false);
        }
        else if (PlayerManager.instance.material == Material.Fire)
        {
            _anim.SetBool("Fire", true);
            _anim.SetBool("Rock", false);
            _anim.SetBool("Vine", false);
        }
        else if (PlayerManager.instance.material == Material.Rock)
        {
            _anim.SetBool("Rock", true);
            _anim.SetBool("Fire", false);
            _anim.SetBool("Vine", false);
        }
        
        
        //if horizontal input is being pressed, change animation to walking
        //Debug.Log(horizontal);
        if (PlayerManager.instance.playerMovement.horizontal != 0)
            _anim.SetBool("Moving", true);
        else
            _anim.SetBool("Moving", false);

        //at close to the peak of the jump, start the fall animation
        if (!PlayerManager.instance.playerMovement.isGrounded && _rb2d.velocity.y <= 0.7f && _rb2d.velocity.y > -0.5F)
        {
            _anim.SetBool("FallTransfer", true);
            _anim.SetBool("Jumping", false);
            _anim.SetBool("Jumpup", false);
        }
        //constant fall animation
        else if (_rb2d.velocity.y <= -0.5f && !PlayerManager.instance.playerMovement.isGrounded)
        {
            _anim.SetBool("FallTransfer", false);
            _anim.SetBool("Falling", true);
        }

        //if hit ground, play the landing animation (animation cancels on its own when finished)
        if (PlayerManager.instance.playerMovement.isGrounded && !PlayerManager.instance.playerMovement.wasGrounded)
        {
            _anim.SetBool("Falling", false);
            _anim.SetBool("Jumping", false);
            _anim.SetBool("Jumpup", false);
            _anim.SetBool("Landing", true);
        }

        if (PlayerManager.instance.playerMovement.hasJumped)
        {
            _anim.SetBool("Landing", false);
            _anim.SetBool("Jumping", true);
        }
    }


    public void TurnRockAniOff()
    {
        _anim.SetBool("Rockslam", false);
        _anim.SetBool("Rockexplo", false);
    }

    public void TurnFireAniOff()
    {
        _anim.SetBool("Firearc", false);
    }

    public void TurnOffVineAni()
    {
        _anim.SetBool("Vineatk", false);
        _anim.SetBool("VineUp", false);
        _anim.SetBool("VineDown", false);
    }

    public void TurnAtkAniOff()
    {
        _anim.SetBool("BasicAtk", false);
    }

    public void JumpUpTransfer()
    {
        _anim.SetBool("Jumpup", true);
        _anim.SetBool("Jumping", false);
    }

    public void Falling()
    {
        _anim.SetBool("FallTransfer", false);
        _anim.SetBool("Falling", true);
    }

    public void DeadAnimation()
    {
        _anim.SetBool("Dead", true);
    }

    public void TurnOffLanding()
    {
        _anim.SetBool("Landing", false);
    }
}
