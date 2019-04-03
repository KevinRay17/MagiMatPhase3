using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

//handles player movement: running, jumping, climbing
//uses Physics2D to check for ground and climbables, does not use attached colliders

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;

    private Vector2 _inputVector;

    [HideInInspector] public int faceDirection; //1 = UP, 2 = RIGHT, 3 = DOWN, 4 = LEFT
    
    //states
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

    public Collider2D nearbyClimbable;
    
    public LayerMask groundLayers;
    public LayerMask climbableLayers;
    
    public float gravityScale;
    
    [FormerlySerializedAs("_anim")] [HideInInspector] private Animator anim;
    [FormerlySerializedAs("_spriteRenderer")] [HideInInspector] public SpriteRenderer spriteRenderer;

    public void Animate(string toSet)
    {
        Debug.Log(this);

        if (anim == null) anim = gameObject.GetComponent<Animator>();
        
        if (anim != null) anim.SetBool(toSet, true);
    }
    
    void Awake()
    { 
        //assign components
        _rigidbody2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        Debug.Log("Anim: " + anim);
        
        spriteRenderer = GetComponent<SpriteRenderer>();

        //store original gravity scale in case it is changed later
        gravityScale = _rigidbody2D.gravityScale;

        canMove = true;

        anim.SetBool("Rockexplo", true);
    }

    void Update()
    {   
        //axis inputs to Vector2
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        _inputVector = new Vector2(horizontal, vertical);

        //grounded check and check if the player was recently grounded
        //if player was recently grounded, set _hasJumped to false
        wasGrounded = isGrounded;
        isGrounded = GroundedCheck();
        if (isGrounded && !wasGrounded)
        {
            hasJumped = false;
        }

        //for jump animation to play i guess
        if (isGrounded)
            anim.SetBool("Jumping", false);
        else
            anim.SetBool("Jumping", true);

        //player can jump if grounded or climbing and has not jumped recently
        if (Input.GetKeyDown(KeyCode.Space) && (isGrounded || isClimbing) && !hasJumped && canMove)
        {
            if (isClimbing)
            {
                StoppedClimbing();
            }
            Jump(jumpPower);
           hasJumped = true;
        }

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
                if (Input.GetKey(KeyCode.W) && canClimb)
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
            if (!ClimbableNearby())
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
    }

    bool GroundedCheck()
    {
        //check below the player if there is ground
        
        return Physics2D.OverlapArea(
            transform.position + new Vector3(-0.3f, -0.94f, 0),
            transform.position + new Vector3(0.3f, -1.04f, 0), 
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

        if (velocity.x == 0f)
        {
            anim.SetBool("Moving", false);
        }
        else
        {
            anim.SetBool("Moving", true);
        }

        //set face direction if horizontalInput != 0;
        if (horizontalInput.x > 0)
        {
            spriteRenderer.flipX = true;
            faceDirection = 2;    
        }
        else if (horizontalInput.x < 0)
        {
            spriteRenderer.flipX = false;
            faceDirection = 4;
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
        anim.SetBool("Moving", false);
        anim.SetBool("Climbing", true);

        //handles vertical movement when climbing

        Vector3 verticalInput = new Vector2(0, _inputVector.y);
        
        //set face direction if verticalInput != 0;
        if (verticalInput.y > 0)
        {
            faceDirection = 1;
        }
        else if (verticalInput.y < 0)
        {
            faceDirection = 3;
        }
        
        _rigidbody2D.MovePosition(transform.position + (verticalInput * climbSpeed * Time.fixedDeltaTime));
    }

    void Jump(float power)
    {
        //add upward force for jump
        //set y velocity to 0 for consistent jump height even if there was previously a downward velocity
        
        Vector2 velocity = _rigidbody2D.velocity;
        velocity.y = 0;
        _rigidbody2D.velocity = velocity;

        _rigidbody2D.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
    }

    void StoppedClimbing()
    {
        anim.SetBool("Climbing", false);
        isClimbing = false;
        canClimb = false;
        _rigidbody2D.gravityScale = gravityScale;
    }
    
    //ANIMATION STUFF
    public void TurnRockAniOff()
    {
        anim.SetBool("Rockslam", false);
        anim.SetBool("Rockexplo", false);
    }

    public void TurnFireAniOff()
    {
        anim.SetBool("Firearc", false);
    }

    public void TurnOffVineAni()
    {
        anim.SetBool("Vineatk", false);
    }
}
