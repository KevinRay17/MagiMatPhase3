using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.SceneManagement;

//handles player actions: absorbing materials, abilities, attacking

public class PlayerActions : MonoBehaviour
{
    //private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody2D;
    private Collider2D _collider;
    
    public Vector2 inputVector;
    public Vector2 mousePos;
    public Vector2 mouseDirection;

    public bool materialAbsorberOut;
    public float materialAbsorberSpeed;
    public GameObject materialAbsorberPrefab;

    private float _downwardStompSpeed = -1000f;
    private bool _groundPounding = false;

    //ANIMATION STUFF
    [Header("Animation prefabs")]
    public GameObject fireAniPrefab;
    public GameObject rockslamAniPrefab;

    //private AudioClip someAudio;

    void Awake()
    {
        //assign components
        
        //_spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
    }

    void Update()
    {   
        //axis inputs to Vector2
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        inputVector = new Vector2(horizontal, vertical);
        
        //get mousePos and mouseDirection
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPos = transform.position;
        mouseDirection = (mousePos - playerPos).normalized;

        if (Input.GetMouseButtonDown(1) && !materialAbsorberOut)
        {
            ThrowMaterialAbsorber(mouseDirection);
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            if (ResourceController.currentMana > 10)
            Attack();
        }
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (ResourceController.currentMana > 25)
            Special();
        }
        
        DebugChangeMaterial();
    }

    void ThrowMaterialAbsorber(Vector2 direction)
    {
        materialAbsorberOut = true;
        
        //for aiming via inputAxis
        /*if (direction == Vector2.zero)
        {
            direction = GlobalFunctions.FaceDirectionToVector2(PlayerManager.instance.playerMovement.faceDirection);
        }*/
        
        GameObject projectile = Instantiate(materialAbsorberPrefab, transform.position, Quaternion.identity);
        
        Rigidbody2D projRB = projectile.GetComponent<Rigidbody2D>();
        projRB.velocity = direction * materialAbsorberSpeed;
    }
    
    void Attack()
    {
        ResourceController.currentMana -= 10f;
        PlayerManager.instance.materialScript.Attack(this.gameObject);
        //Ground pound with Rock Abilities
        if (PlayerManager.instance.material == Material.Rock)
        {
            //only create the rocks if you're on the ground
            if (PlayerManager.instance.playerMovement.isGrounded)
            {
                PlayerManager.instance.playerMovement.anim.SetBool("Rockslam", true);
                GameObject rockslamObj = Instantiate(rockslamAniPrefab, transform.position, Quaternion.identity);
                rockslamObj.transform.parent = gameObject.transform;
                SpriteRenderer rockslamSR = rockslamObj.GetComponent<SpriteRenderer>();
                //flip with the player
                if (PlayerManager.instance.playerMovement.spriteRenderer.flipX)
                    rockslamSR.flipX = true;
                else
                    rockslamSR.flipX = false;
            }


            var clip = Resources.Load<AudioClip>("Sounds/rockThrow");
            AudioManager.instance.playSound(clip);
            _groundPounding = true;
            this._rigidbody2D.AddForce(new Vector2(0,_downwardStompSpeed));
        }
        else if (PlayerManager.instance.material == Material.Vine)
        {
            PlayerManager.instance.playerMovement.anim.SetBool("Vineatk", true);

            var SFX = Resources.Load<AudioClip>("Sounds/vineAttack");
            AudioManager.instance.playSound(SFX);
        }
        if  (PlayerManager.instance.material == Material.Fire)
        {
            //set bool to play animation
            PlayerManager.instance.playerMovement.anim.SetBool("Firearc", true);
            GameObject fireObj = Instantiate(fireAniPrefab, transform.position, Quaternion.identity);
            //make the effect object a child of the player so that it follows the player if they move
            fireObj.transform.parent = gameObject.transform;
            //flip with the player
            SpriteRenderer fireSR = fireObj.GetComponent<SpriteRenderer>();
            if (PlayerManager.instance.playerMovement.spriteRenderer.flipX)
                fireSR.flipX = true;
            else
                fireSR.flipX = false;

            var SFX = Resources.Load<AudioClip>("Sounds/vineAttack");
            AudioManager.instance.playSound(SFX);
        }
    }
    
    void Special()
    {
        ResourceController.currentMana -= 25f;
        PlayerManager.instance.materialScript.Special(this.gameObject);

        if (PlayerManager.instance.material == Material.Fire)
        {
            //_playermoveCS.anim.SetBool("Dashing", true);
        } else if (PlayerManager.instance.material == Material.Rock)
        {
            PlayerManager.instance.playerMovement.anim.SetBool("Rockexplo", true);
        }
        else if (PlayerManager.instance.material == Material.Vine)
        {
            //don't have ani for vine special .-.
        }
    }
    
    //CHANGE MATERIAL BY PRESSING NUMBER KEYS------------------------------------------------------------------------
    void DebugChangeMaterial()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlayerManager.instance.ChangeMaterial(Material.None);
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PlayerManager.instance.ChangeMaterial(Material.Vine);
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            PlayerManager.instance.ChangeMaterial(Material.Fire);
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            PlayerManager.instance.ChangeMaterial(Material.Rock);
            return;
        }
    }
    //-------------------------------------------------------------------------------------------------------------------

    private void OnCollisionEnter2D(Collision2D other)
    {
        //On Ground Pound, break Breakable ground pieces
        if (_groundPounding && other.gameObject.CompareTag("Breakable"))
        {
            Destroy(other.gameObject);
        }
        else if (_groundPounding)
        {
            _groundPounding = false;
        }  
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Water") && PlayerManager.instance.material == Material.Fire)
            ResourceController.currentMana = 0;
    }
}
