using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    VineMaterial vineCS;

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

    GameObject player;
    private Animator _anim;
    private Animator _parentAnim;
    private Rigidbody2D _rb2d;
    private SpriteRenderer _sr;
    private SpriteRenderer _parentsr;

    private void Start()
    {
        if (transform.parent != null)
            player = transform.parent.gameObject;
        else
            player = gameObject;

        vineCS = FindObjectOfType<VineMaterial>();

        _anim = GetComponent<Animator>();
        _sr = GetComponent<SpriteRenderer>();
        if (transform.parent != null)
        {
            _rb2d = GetComponentInParent<Rigidbody2D>();
            _parentAnim = transform.parent.GetComponent<Animator>();
            _parentsr = transform.parent.GetComponent<SpriteRenderer>();
        }
        else
            _rb2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _anim.SetInteger("direction", PlayerManager.instance.playerMovement.faceDirection);

        //please kill me
        if (_parentAnim != null)
        {
            _anim.SetBool("Fire", _parentAnim.GetBool("Fire"));
            _anim.SetBool("Vine", _parentAnim.GetBool("Vine"));
            _anim.SetBool("Rock", _parentAnim.GetBool("Rock"));
            _sr.flipX = PlayerManager.instance.playerMovement.spriteRenderer.flipX;
            _anim.SetBool("Landing", _parentAnim.GetBool("Landing"));
            _anim.SetBool("Throw", _parentAnim.GetBool("Throw"));
            _anim.SetBool("Atk", _parentAnim.GetBool("Atk"));
            _anim.SetBool("Special", _parentAnim.GetBool("Special"));
            _anim.SetBool("Dead", _parentAnim.GetBool("Dead"));
            _anim.SetBool("Hug", _parentAnim.GetBool("Hug"));
            _anim.SetBool("HugTop", _parentAnim.GetBool("HugTop"));
            _anim.SetBool("Climb", _parentAnim.GetBool("Climb"));
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

            Color a = _sr.color;

            if (PlayerManager.instance.playerHealth.invincible)
                a.a = _parentsr.color.a;

            if (PlayerManager.instance.playerHealth.isPlayerDead || _anim.GetBool("Special") || _anim.GetBool("Dead"))
                a.a = 0f;

            _sr.color = a;
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
            if (PlayerManager.instance.playerHealth.invincible)
                _sr.color = _parentsr.color;
            else if (_anim.GetBool("Fire") || _anim.GetBool("Vine") || _anim.GetBool("Rock"))
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


        /*
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
        */



        if (PlayerManager.instance.playerMovement.justJumped)
        {
            //print("jumped");
            _anim.SetTrigger("jump");
        }

        if (PlayerManager.instance.playerActions.didAttack && PlayerManager.instance.playerMovement.anim.GetBool("Landing"))
        {
            PlayerManager.instance.playerMovement.anim.SetBool("Landing", false);
            PlayerManager.instance.playerActions.didAttack = false;
        }

        if (PlayerManager.instance.playerMovement.isGrounded && !PlayerManager.instance.playerMovement.wasGrounded)
        {
            _anim.SetBool("Landing", true);
        }
        else if (_anim.GetBool("Atk"))
            _anim.SetBool("Landing", false);

        if (PlayerManager.instance.playerHealth.health <= 0)
            _anim.SetBool("Dead", true);
        else
            _anim.SetBool("Dead", false);


        _anim.SetBool("grounded", PlayerManager.instance.playerMovement.isGrounded);
        _anim.SetFloat("xVel", Mathf.Abs(PlayerManager.instance.playerMovement.getSpeed().x));
        _anim.SetFloat("yVel", PlayerManager.instance.playerMovement.getSpeed().y);
        
        //Debug.Log(vineCS.onWall);

        if (vineCS.onWall)
        {
            /*
             * fuck this
            if (Mathf.Abs(player.transform.position.y) > (Mathf.Abs(vineCS.grapplepos.y) + 2f))
                Debug.Log("hug");
            else
                Debug.Log("ahh");
            */
            _anim.SetBool("Hug", true);
                
            if (player.transform.position.x > vineCS.grapplepos.x)
                PlayerManager.instance.playerMovement.spriteRenderer.flipX = true;
            else
                PlayerManager.instance.playerMovement.spriteRenderer.flipX = false;
        }
        else if (!vineCS.onWall)
        {
            _anim.SetBool("Hug", false);
            _anim.SetBool("HugTop", false);
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
        //_anim.SetBool("BasicAtk", false);
        _anim.SetBool("Atk", false);
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
    
    public void TurnOffThrow()
    {
        _anim.SetBool("Throw", false);
    }

    public void TurnOffSpecial()
    {
        _anim.SetBool("Special", false);
    }
}
