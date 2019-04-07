using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FireToad : MonoBehaviour
{

    private Animator _anim;
    protected Rigidbody2D _rigidbody2D;
    private bool _isGrounded;
    private bool _isJumping;
    private bool _isRunning = false;
    public float jumpPower;
    public GameObject Fireball;
    public float retreatSpeed;
    LayerMask layerMask = 1 << 8;
    private LayerMask playerMask = 1 << 11;
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }
    void Update()
    {
        //Will jump if it is on the ground and isn't already in the jumping process
        if (_isGrounded && !_isJumping && !_isRunning)
        {
            StartCoroutine(JumpAttack());
        }
        //If enemies are close enough, run away and don't attack
        if (Physics2D.OverlapCircle(
            transform.position, 3, playerMask) && !_isRunning)
        {
            transform.position = Vector2.MoveTowards(transform.position,
                PlayerManager.instance.gameObject.transform.position, -retreatSpeed * Time.deltaTime);
           // StartCoroutine(JumpAway());
        }
    }
    void Jump(float power)
    {
        _anim.SetBool("Jump", true);
        //add upward force for jump
        //set y velocity to 0 for consistent jump height even if there was previously a downward velocity
        
        Vector2 velocity = _rigidbody2D.velocity;
        velocity.y = 0;
        _rigidbody2D.velocity = velocity;
        _rigidbody2D.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
    }

    IEnumerator JumpAway()
    {
        _isRunning = true;
        float timer = 0;
        float timerMax = 1000;
        yield return 0;

        while (timer <= timerMax)
        {
            
            timer+= 1;
            yield return 0;
        }
     

    }

    //Wait a few seconds, jumps, attacks during the jump at different heights
    IEnumerator JumpAttack()
    {
        _isJumping = true;
        float JumpWait = Random.Range(3f,4f);
        float AttackWait = Random.Range(.3f, .4f);
        yield return new WaitForSeconds(JumpWait);
        Jump(jumpPower);
        _isGrounded = false;
        yield return new WaitForSeconds(AttackWait);
        if (Physics2D.OverlapCircle(
            transform.position, 12, playerMask))
        {
            GameObject FireballClone = Instantiate(Fireball, transform.position, Quaternion.identity);
        }

        _isJumping = false;
    }

    //Set grounded when on the ground
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == 8)
        {
            _anim.SetBool("Jump", false);
            _isGrounded = true;
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("HurtBox"))
        {
            Destroy(gameObject);
        }
    }
}