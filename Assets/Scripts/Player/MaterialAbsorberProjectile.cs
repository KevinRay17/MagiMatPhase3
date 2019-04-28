using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialAbsorberProjectile : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    private Collider2D _collider2D;

    [HideInInspector]public float maxDistance = 1000; //max distance before projectile automatically returning, change in PlayerActions script (materialAbsorberMaxDistance)
    private float _distanceTravelled;

    [HideInInspector] public bool attached; //whether or not the absorber has hit a target
    [HideInInspector] public Material attachedMaterial = Material.None; //if attached or returning, the material that the absorber is now holding

    [HideInInspector] public bool returning;
    [HideInInspector] public float returnSpeed; //speed that absorber returns to player, change in PlayerActions script

    public GameObject particlePrefab;
    public int particleAmount;

    public static bool FireUnlocked = false;
    
    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _collider2D = GetComponent<Collider2D>();
        

        

    }

    void Update()
    {
        //track distance travelled, when reaching maxDistance, start returning automatically
        _distanceTravelled += _rigidbody2D.velocity.magnitude * Time.deltaTime;
        if (_distanceTravelled >= maxDistance)
        {
            StartReturning();
        }

        //handles moving the absorber when returning
        if (returning)
        {
            _rigidbody2D.MovePosition(Vector2.MoveTowards(transform.position, PlayerManager.instance.player.transform.position, returnSpeed * Time.deltaTime));
            transform.right = PlayerManager.instance.player.transform.position - transform.position;
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
<<<<<<< HEAD
        Attach();
        OnTriggerStay2D(other.collider);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        //going outwards
=======
        
        //thrown out
>>>>>>> origin/CH_Branch2
        if (!attached && !returning)
        {
            if (other.CompareTag("Enemy"))
            {
                Enemy enemyScript = other.gameObject.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    if (enemyScript.material != Material.None)
                    {
                        Attach(enemyScript.material);
                        
                    }
                }
            }
            else if (other.CompareTag("MaterialSource"))
            {
                MaterialSource materialSourceScript = other.gameObject.GetComponent<MaterialSource>();
                if (materialSourceScript != null)
                {
                    if (materialSourceScript.material != Material.None && FireUnlocked)
                    {
                        Attach(materialSourceScript.material);
                    }
                    else if (materialSourceScript.material == Material.Vine ||
                             materialSourceScript.material == Material.Rock)
                    {
                        Attach(materialSourceScript.material);
                    }
                }
            }
            else if (other.CompareTag("MaterialUnlock"))
            {
                MaterialSource materialSourceScript = other.gameObject.GetComponent<MaterialSource>();
                Attach(materialSourceScript.material);
                FireUnlocked = true;
            }
        }
        else
        {
            //returning
            if (other.CompareTag("Player"))
            {
                //if the absorber is returning and collides with the player, give the player the attachedMaterial and destroy itself
                if (returning)
                {
                    if (attachedMaterial != Material.None)
                    {
                        ResourceController.currentMana = 100f;
                        PlayerManager.instance.ChangeMaterial(attachedMaterial);
                    }

                    PlayerManager.instance.playerActions.materialAbsorberOut = false;
                    DestroySelf();
                }
            }
        }
    }

    private void Attach(Material material = Material.None)
    {
        //call when absorber hits a collider when going out
        
        //Sound for Knife Landing
        var knifeLand = Resources.Load<AudioClip>("Sounds/KnifeLand");
        AudioManager.instance.playSound(knifeLand);
        _rigidbody2D.velocity = Vector2.zero;
        attached = true;
        attachedMaterial = material;
    }

    public void StartReturning()
    {
        //call to make absorber return towards the player
        _rigidbody2D.velocity = Vector2.zero;
        attached = false;
        returning = true;
        
        //on return, allow the absorber to collide with the player again and make it a trigger so it can fly through obstacles
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Absorber"), false);
        _collider2D.isTrigger = true;
    }
    
    private void SpawnParticles()
    {
        for (int i = 0; i < particleAmount; i++)
        {
            Vector2 pos = transform.position;
            GameObject particle = Instantiate(particlePrefab, pos, Quaternion.identity);
        }
    }
    
    private void DestroySelf()
    {
        PlayerManager.instance.playerActions.materialAbsorberOut = false;
        Destroy(this.gameObject);  
    }
}
