using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//stores references to all player scripts
//store player's active material?

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    
    [HideInInspector] public PlayerMovement playerMovement;
    [HideInInspector] public PlayerHealth playerHealth;
    [HideInInspector] public PlayerActions playerActions;

    public GameObject player;

    public Material material;
    [HideInInspector] public MaterialClass materialScript;

    void Awake()
    {
        instance = this;
        
        playerMovement = GetComponent<PlayerMovement>();
        playerHealth = GetComponent<PlayerHealth>();
        playerActions = GetComponent<PlayerActions>();

        player = this.gameObject;
    }

    void Start()
    {
        ChangeMaterial(Material.None);
    }

    public void ChangeMaterial(Material newMaterial)
    {
        material = newMaterial;
        materialScript = MaterialsManager.GetMaterialScript(newMaterial);
        
        Debug.Log("New Material: " + material);
    }
    
    public void Death()
    {
        playerMovement.enabled = false;
        playerHealth.enabled = false;
        playerActions.enabled = false;
    }
}
