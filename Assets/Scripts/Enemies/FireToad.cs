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
       
        //If enemies are close enough, run away and don't attack
        if (Physics2D.OverlapCircle(
            transform.position, 3, playerMask) && !_isRunning && !_isJumping)
        {
            //transform.position = Vector2.MoveTowards(transform.position,
              //  PlayerManager.instance.gameObject.transform.position, -retreatSpeed * Time.deltaTime);
            StartCoroutine(JumpAway());
        }
        else if (_isGrounded && !_isJumping && !_isRunning)
        {
            StartCoroutine(JumpAttack());
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
        float JumpWait = Random.Range(.15f,.25f);
        int gen = Random.Range(0, 2);
        yield return new WaitForSeconds(JumpWait);
        if (gen == 1)
        {
            _rigidbody2D.AddForce(Vector2.right * jumpPower/4, ForceMode2D.Impulse);
            Debug.Log("ad");
        }
        else
        {
            _rigidbody2D.AddForce(Vector2.left * jumpPower/4, ForceMode2D.Impulse);
            Debug.Log("sajfcn");
        }
        _rigidbody2D.AddForce(Vector2.up * jumpPower/2, ForceMode2D.Impulse);
        _anim.SetBool("Jump", true);
        yield return 0;
        


    }

    //Wait a few seconds, jumps, attacks during the jump at different heights
    IEnumerator JumpAttack()
    {
        _isJumping = true;
        float JumpWait = Random.Range(1f,2f);
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
        yield return new WaitForSeconds(3);
        _isJumping = false;
    }

    //Set grounded when on the ground
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == 8)
        {
            _anim.SetBool("Jump", false);
            _isGrounded = true;
            _isRunning = false;
            _isJumping = false;
            
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