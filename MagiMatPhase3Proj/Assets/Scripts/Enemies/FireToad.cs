using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireToad : MonoBehaviour
{
    
     
    protected Rigidbody2D _rigidbody2D;
    public bool _isGrounded;
    public bool _isJumping;
    public float jumpPower;
    public GameObject Fireball;
    LayerMask layerMask = 1 << 8;
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        //Will jump if it is on the ground and isn't already in the jumping process
        if (_isGrounded && !_isJumping)
        {
            StartCoroutine(JumpAttack());
        }
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

    //Wait a few seconds, jumps, attacks during the jump at different heights
    IEnumerator JumpAttack()
    {
        _isJumping = true;
        float JumpWait = 3;
        float AttackWait = Random.Range(.3f, .4f);
        yield return new WaitForSeconds(JumpWait);
        Jump(jumpPower);
        _isGrounded = false;
        yield return new WaitForSeconds(AttackWait);
        GameObject FireballClone = Instantiate(Fireball, transform.position, Quaternion.identity);
        _isJumping = false;
    }

    //Set grounded when on the ground
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == 8)
        {
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