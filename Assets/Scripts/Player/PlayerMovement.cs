﻿using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;

//handles player movement: running, jumping, climbing
//uses Physics2D to check for ground and climbables, does not use attached colliders

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    
    private Vector2 _inputVector;
    [HideInInspector]
    public Vector2 inputVector
    {
        get { return _inputVector;  }
    }

    public Vector3 teleLastPos;

    [HideInInspector] public int faceDirection; //1 = UP, 2 = RIGHT, 3 = DOWN, 4 = LEFT

    //states
    public bool justJumped;
    [HideInInspector] public bool hasJumped; //whether or not the player has jumped recently, set to false when recently grounded/climbing
    [HideInInspector] public bool isGrounded;
    [HideInInspector] public bool wasGrounded;
    [HideInInspector] public bool isClimbing;
    [HideInInspector] public bool canClimb;
    [HideInInspector] public bool canMove;
    
    //movement stats
    public float acceleration;
    public float groundFriction; //drag force
    public float maxMoveSpeed; //max horizontal velocity, does not cap velocity, instead determines when more force can be added
    public float climbSpeed;
    public float jumpPower;
    public float jumpDownwardForce = 500f;

    public Collider2D nearbyClimbable;
    
    public LayerMask groundLayers;
    public LayerMask ground;
    public LayerMask climbableLayers;
    
    public float gravityScale;

    protected Animator _anim;
    [HideInInspector] public Animator anim
    {
        get { return _anim; }
    }
    protected SpriteRenderer _spriteRenderer;
    [HideInInspector] public SpriteRenderer spriteRenderer
    {
        get { return _spriteRenderer; }
    }

    [HideInInspector] public Vector2 getSpeed()
    {
        return _rigidbody2D.velocity;
    }
    
    void Awake()
    { 
        //assign components
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        //store original gravity scale in case it is changed later
        gravityScale = _rigidbody2D.gravityScale;

        canMove = true;
        teleLastPos = transform.position;
    }

    
    [HideInInspector]
    public float horizontal;
    protected float _horizontal
    {
        get { return horizontal; }
    }
    [HideInInspector]
    public float vertical;
    protected float _vertical
    {
        get { return vertical; }
    }

    Vector3 verticalInput;



    void Update()
    {
        justJumped = false;
        /*
         * sorry i have to move this for animation
        //axis inputs to Vector2
<<<<<<< HEAD
        float horizontal = Input.GetAxisRaw("LeftJSHorizontal");
        float vertical = Input.GetAxisRaw("LeftJSVertical");
        */
       // horizontal = Input.GetAxisRaw("LeftJSHorizontal");
        //vertical = Input.GetAxisRaw("LeftJSVertical");
        
        //_inputVector = new Vector2(horizontal, vertical);
        
//=======
        float horizontal = InputManager.GetMovementAxisHorizontal();
        float vertical = InputManager.GetMovementAxisVertical();
        _inputVector = new Vector2(horizontal, vertical);

        //if A or D are being pressed, set animation to walking
        //Debug.Log(horizontal);
        if (horizontal != 0)
            anim.SetBool("Moving", true);
        else
            anim.SetBool("Moving", false);

//>>>>>>> origin/master
        //grounded check and check if the player was recently grounded
        //if player was recently grounded, set _hasJumped to false
        wasGrounded = isGrounded;
        isGrounded = GroundedCheck();
        if (isGrounded && !wasGrounded)
        {
            hasJumped = false;
            var clip = Resources.Load<AudioClip>("Audio/JumpLand");
            AudioManager.instance.PlaySound(clip);
        }

        if (isGrounded)
        {
            //check if ground is below the player and set last grounded position for respawn
            if (Physics2D.OverlapArea(transform.position + new Vector3(-0.1f, -0.94f, 0),
                transform.position + new Vector3(0.1f, -1.04f, 0),
                ground))
            {
                Debug.Log("NEWSPAWN");
                teleLastPos = this.transform.position;
            }   
        }
        
        //animation stuff
        if (isGrounded && anim.GetBool("Landing") && horizontal != 0)
            anim.SetBool("Landing", false);


        //player can jump if grounded or climbing and has not jumped recently
        if (InputManager.GetJumpButtonDown() && (isGrounded || isClimbing) && canMove)
        {
            _anim.SetBool("Climb", false);

            if (isClimbing)
            {
                StoppedClimbing();
            }
            Jump(jumpPower);
            hasJumped = true;
            justJumped = true;
        }

        //Debug.Log(_rigidbody2D.velocity.y);

        //if the player is not currently climbing, circleCast nearby to look for climbables
        //if there is a nearby climbable, press W to start climbing
        if (!isClimbing)
        {
            if (nearbyClimbable != ClimbableNearby())
            {
                nearbyClimbable = ClimbableNearby();
                canClimb = true;
            }

            if (nearbyClimbable)
            {
                if ((InputManager.GetMovementAxisVertical() < -0.5) && canClimb)
                {
                    //changes for changing movement mode to climbing
                    isClimbing = true;
                    hasJumped = false;
                    _rigidbody2D.gravityScale = 0;
                    _rigidbody2D.velocity = Vector2.zero;
                    SnapXToClimbable(nearbyClimbable);
                }
            }
        }
        else
        {
            //if player is climbing, check if they are still on the climbable
            if (canMove && !ClimbableNearby())
            {
                StoppedClimbing();
            }
        }
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            if (isClimbing)
            {
                Climb();
            }
            else
            {
                Run();
            }
        }

        if (hasJumped)
        {
            float vertVelocity = _rigidbody2D.velocity.y;
            if (vertVelocity > 0 && !InputManager.GetJumpButton())
            {
                _rigidbody2D.AddForce(Vector2.down * jumpDownwardForce * 4 * Time.deltaTime); 
            }
            else if (vertVelocity < 0)
            {
                _rigidbody2D.AddForce(Vector2.down * jumpDownwardForce * Time.deltaTime);
            }
        }
    }

    bool GroundedCheck()
    {
        //check below the player if there is ground or water or other objects
        
        return Physics2D.OverlapArea(
            transform.position + new Vector3(-0.1f, -0.94f, 0),
            transform.position + new Vector3(0.1f, -1.04f, 0), 
            groundLayers);
    }

    Collider2D ClimbableNearby()
    {
        //circle cast on the center of the player for climbables, return Collider2D if one is present
        
        return Physics2D.OverlapCircle(transform.position, 0.25f, climbableLayers);
    }

    void SnapXToClimbable(Collider2D climbable)
    {
        //when player starts climbing, center player's X position to climbable's center
        
        float snapX = climbable.transform.position.x;
        transform.position = new Vector2(snapX, transform.position.y);
    }

    void Run()
    {
        //handles horizontal movement when grounded or in the air
        Vector2 velocity = _rigidbody2D.velocity;
        Vector2 horizontalInput = new Vector2(_inputVector.x, 0);
        
        //set face direction if horizontalInput != 0;
        if (horizontalInput.x > 0)
        {
            /*
            //if (!anim.GetBool("Vineatk") && !anim.GetBool("BasicAtk"))
            {
                spriteRenderer.flipX = true;
                faceDirection = 2;
            }
            */
            if (!anim.GetBool("Atk"))
            {
                spriteRenderer.flipX = true;
                faceDirection = 2;
            }
        }
        else if (horizontalInput.x < 0)
        {
            /*
            //if (!anim.GetBool("Vineatk") && !anim.GetBool("BasicAtk"))
            {
                spriteRenderer.flipX = false;
                faceDirection = 4;
            }
            */
            if (!anim.GetBool("Atk"))
            {
                spriteRenderer.flipX = false;
                faceDirection = 4;
            }
        }

        //moved this from the climb function
        //handles vertical movement when climbing
        verticalInput = new Vector2(0, _inputVector.y);

        //set face direction if verticalInput != 0;
        if (verticalInput.y > 0)
        {
            faceDirection = 3;
        }
        else if (verticalInput.y < 0)
        {
            faceDirection = 1;
        }


        //get player's current velocity direction and input directions
        int currentDirection = 0;
        int moveDirection = 0;

        if (_rigidbody2D.velocity.x < 0)
        {
            currentDirection = -1;
        } 
        else if (_rigidbody2D.velocity.x > 0)
        {
            currentDirection = 1;
        }

        if (_inputVector.x < 0)
        {
            moveDirection = -1;
        }
        else if (_inputVector.x > 0)
        {
            moveDirection = 1;
        }
        
        if (isGrounded && moveDirection == 0)
        {
            
            
            //if the player is grounded and is no longer moving the character, apply drag to bring player to a fast stop
            Vector2 velocityX = new Vector2(velocity.x, 0);
            _rigidbody2D.AddForce(velocityX * -groundFriction * Time.fixedDeltaTime);
        } 
        else if (moveDirection == currentDirection)
        {
            if (Mathf.Abs(velocity.x) < maxMoveSpeed)
            {
                //if the player is moving forward (same direction as previous frame), don't apply force if velocity is past maxMoveSpeed
                _rigidbody2D.AddForce(horizontalInput * acceleration * Time.fixedDeltaTime, ForceMode2D.Force);
            }
        }
        else
        {
                //if the player is changing direction, don't check maxMoveSpeed
                _rigidbody2D.AddForce(horizontalInput * acceleration * Time.fixedDeltaTime, ForceMode2D.Force);
        }
    }

    void Climb()
    {
        Vector3 verticalinput = new Vector2(0, _inputVector.y);

        //set face direction if verticalInput != 0;
        if (verticalinput.y > 0)
        {
            _anim.SetBool("Climb", true);
            faceDirection = 3;
        }
        else if (verticalinput.y < 0)
        {
            _anim.SetBool("Climb", true);
            faceDirection = 1;
        }

        _rigidbody2D.MovePosition(transform.position + (-verticalinput * climbSpeed * Time.fixedDeltaTime));
    }

    public void Jump(float power)
    {
        //add upward force for jump
        //set y velocity to 0 for consistent jump height even if there was previously a downward velocity

        if (anim.GetBool("Landing"))
            anim.SetBool("Landing", false);


        Vector2 velocity = _rigidbody2D.velocity;
        velocity.y = 0;
        _rigidbody2D.velocity = velocity;
        
                
        //Audio Clip Stuff Below
        var clip = Resources.Load<AudioClip>("Sounds/JumpLaunch");
        AudioManager.instance.PlaySound(clip);

        _rigidbody2D.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
    }
    
    void StoppedClimbing()
    {
        anim.SetBool("Climbing", false);
        isClimbing = false;
        canClimb = false;
        _rigidbody2D.gravityScale = gravityScale;
    }
}