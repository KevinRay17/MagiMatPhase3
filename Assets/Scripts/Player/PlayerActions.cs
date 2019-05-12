using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


//handles player actions: absorbing materials, abilities, attacking

public class PlayerActions : MonoBehaviour
{
    public static PlayerActions instance;
    //private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody2D;
    private Collider2D _collider;
    
    public bool didAttack = false;
    private float didAttackResetCounter = 10;
    
    public Vector2 inputVector;
    public Vector2 mousePos;
    public Vector2 aimDirection;
    
    //Direction of right joystick
    public Vector2 joystickDirection;
    public Vector2 lastDirection;

    private MaterialAbsorberProjectile _currentMaterialAbsorber;
    public bool materialAbsorberOut; //is the absorber currently out, player shouldn't be able to throw another one until it returns
    public float materialAbsorberSpeed; //how fast the absorber travels out
    public float materialAbsorberReturnSpeed; //how fast the absorber returns to you
    public float materialAbsorberMaxDistance; //the max distance the absorber can travel before automatically returning
    public GameObject materialAbsorberPrefab;

    private float _downwardStompSpeed = -1000f;
    private bool _groundPounding = false;

    [HideInInspector] public ResourceController RC;
    
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
        RC = gameObject.GetComponent<ResourceController>();
    }

    void Update()
    {
        
            
        aimDirection = InputManager.GetAimDirection();
        
        //Debug.Log(joystickDirection);
        didAttackResetCounter -= 1;
        if (didAttackResetCounter < 0)
        {
            didAttack = false;
        }
        //axis inputs to Vector2
        float horizontal = InputManager.GetMovementAxisHorizontal();
        float vertical = InputManager.GetMovementAxisVertical();
        inputVector = new Vector2(horizontal, vertical);

        if (InputManager.GetThrowButtonDown())
        {
            if (materialAbsorberOut)
            {
                _currentMaterialAbsorber.StartReturning();
            }
            else
            {
                //<<<<<<< HEAD
                //              ThrowMaterialAbsorber(mouseDirection);

                PlayerManager.instance.playerMovement.anim.SetBool("Throw", true);

                //Sound for Knife Landing
                var knifeThrow = Resources.Load<AudioClip>("Sounds/KnifeThrow");
                AudioManager.instance.PlaySound(knifeThrow);

                if (aimDirection == Vector2.zero)
                {
                    ThrowMaterialAbsorber(GlobalFunctions.FaceDirectionToVector2(PlayerManager.instance.playerMovement.faceDirection));
                }
                else
                {
                    ThrowMaterialAbsorber(aimDirection);
                }
            }
        }
        
        if (InputManager.GetAttackButtonDown() && RC.isAvailable(RC.attackIndex)) {
            
            Attack();
            RC.resetCooldown(RC.attackIndex);
        }
        
        if (InputManager.GetSpecialButtonDown() && RC.isAvailable(RC.specialIndex)) {
            Special();
            RC.resetCooldown(RC.specialIndex);
        }
        
        DebugChangeMaterial();
    }

    void ThrowMaterialAbsorber(Vector2 direction)
    {
        //disable collisions between projectile and player until returning
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Absorber"), true);
        
        //instantiate absorber prefab, set its velocity and facing relative to aim direction
        materialAbsorberOut = true;
        
        GameObject projectile = Instantiate(materialAbsorberPrefab, transform.position, Quaternion.identity);
        
        Rigidbody2D projRB = projectile.GetComponent<Rigidbody2D>();
        projRB.velocity = direction * materialAbsorberSpeed;
        projectile.transform.right = projRB.velocity;
        _currentMaterialAbsorber = projectile.GetComponent<MaterialAbsorberProjectile>();
        _currentMaterialAbsorber.maxDistance = materialAbsorberMaxDistance;
        _currentMaterialAbsorber.returnSpeed = materialAbsorberReturnSpeed;
    }

    public void Attack()
    {
        didAttack = true;
        PlayerManager.instance.materialScript.Attack(this.gameObject);
        
        PlayerManager.instance.playerMovement.anim.SetBool("Atk", true);

        if (PlayerManager.instance.material == Material.None)
        {
            //Debug.Log(PlayerManager.instance.playerMovement.faceDirection);
            //PlayerManager.instance.playerMovement.anim.SetBool("BasicAtk", true);

            var BasicAttack = Resources.Load<AudioClip>("Sounds/BasicAttack");
            AudioManager.instance.PlaySound(BasicAttack);
        }
        //Ground pound with Rock Abilities
        else if (PlayerManager.instance.material == Material.Rock)
        {
            /*
            //animation stuff
            //only create the rocks if you're on the ground
            if (PlayerManager.instance.playerMovement.isGrounded)
            {
                
                PlayerManager.instance.playerMovement.anim.SetBool("Rockslam", true);
                //PlayerManager.instance.playerMovement.anim.SetBool("Special", true);
                GameObject rockslamObj = Instantiate(rockslamAniPrefab, transform.position, Quaternion.identity);
                rockslamObj.transform.parent = gameObject.transform;
                SpriteRenderer rockslamSR = rockslamObj.GetComponent<SpriteRenderer>();
                //flip with the player
                if (PlayerManager.instance.playerMovement.spriteRenderer.flipX)
                    rockslamSR.flipX = true;
                else
                    rockslamSR.flipX = false;
                
            }
            */
            var clip = Resources.Load<AudioClip>("Sounds/RockAttack");
            AudioManager.instance.PlaySound(clip, 0.8f);
            _groundPounding = true;
            this._rigidbody2D.AddForce(new Vector2(0,_downwardStompSpeed));
        }
        else if (PlayerManager.instance.material == Material.Vine)
        {
            //PlayerManager.instance.playerMovement.anim.SetBool("Vineatk", true);

            //vertical attack
            //Debug.Log(PlayerManager.instance.playerMovement.faceDirection);
            
            /*
            if (PlayerManager.instance.playerMovement.faceDirection == 1)
                PlayerManager.instance.playerMovement.anim.SetBool("VineUp", true);
            else if (PlayerManager.instance.playerMovement.faceDirection == 2 || PlayerManager.instance.playerMovement.faceDirection == 4)
                PlayerManager.instance.playerMovement.anim.SetBool("Vineatk", true);
            else if (PlayerManager.instance.playerMovement.faceDirection == 3)
                PlayerManager.instance.playerMovement.anim.SetBool("VineDown", true);
            */

            var VineAttack = Resources.Load<AudioClip>("Sounds/VineAttack");
            AudioManager.instance.PlaySound(VineAttack);
        }
        if  (PlayerManager.instance.material == Material.Fire)
        {
            /*
            //fire prefab animation
            PlayerManager.instance.playerMovement.anim.SetBool("Firearc", true);
            GameObject fireObj = Instantiate(fireAniPrefab, transform.position, Quaternion.identity);
            //make the effect object a child of the player so that it follows the player if they move
            fireObj.transform.parent = gameObject.transform;
            //flip with the player
            SpriteRenderer fireSR = fireObj.GetComponent<SpriteRenderer>();
            //please kill me
            //Debug.Log(PlayerManager.instance.playerMovement.faceDirection);
            if (PlayerManager.instance.playerMovement.faceDirection == 2)
                fireSR.flipX = true;
            else if (PlayerManager.instance.playerMovement.faceDirection == 4)
                fireSR.flipX = false;
            */



            var fireAttack = Resources.Load<AudioClip>("Sounds/FireAttack");
            AudioManager.instance.PlaySound(fireAttack);
        }
    }
    
    void Special()
    {
        PlayerManager.instance.materialScript.Special(this.gameObject);

        if (!PlayerManager.instance.material.Equals(Material.None))
            PlayerManager.instance.playerMovement.anim.SetBool("Special", true);
    }
    
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

    private void OnCollisionEnter2D(Collision2D other)
    {
        //On Ground Pound, break Breakable ground pieces
        if (_groundPounding && other.gameObject.CompareTag("Breakable"))
        {
            Destroy(other.gameObject);
            _groundPounding = false;
            PlayerManager.instance.playerMovement.anim.SetBool("Atk", false);
        }
        else if (_groundPounding)
        {
            PlayerManager.instance.playerMovement.anim.SetBool("Atk", false);
            _groundPounding = false;
        }  
    }
}
