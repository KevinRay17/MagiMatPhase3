using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.SceneManagement;

//handles player actions: absorbing materials, abilities, attacking

public class PlayerActions : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
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

    void Awake()
    {
        //assign components
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
    }

    void Update()
    {
        //reset scene for testing
        if(Input.GetKeyDown(KeyCode.R))
        {
            ResetScene();
        }
        
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
            Attack();
        }
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
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
        PlayerManager.instance.materialScript.Attack(this.gameObject);
        //Ground pound with Rock Abilities
        if (PlayerManager.instance.material == Material.Rock)
        {
            _groundPounding = true;
            this._rigidbody2D.AddForce(new Vector2(0,_downwardStompSpeed));
        }
    }
    
    void Special()
    {
        PlayerManager.instance.materialScript.Special(this.gameObject);
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
        }
        else if (_groundPounding)
        {
            _groundPounding = false;
        }  
    }
    
   

    void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
