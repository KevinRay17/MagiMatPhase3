using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// TODO: Modify PlayerManager, move canMove variable to PlayerManager
public class Death : MonoBehaviour
{

    // Text enabled telling the player they died
    public GameObject youDiedText;
    
    public GameObject GameOverObject;

    public float timeTilRespawn;

    // This script assumes that we're attached to the Player, if that is incorrect, I'll edit the code later on.
    // Where we'll respawn after dying
    private Vector3 respawnPosition;

    // youDiedText is set to false as soon as the scene is reloaded so there's no issues on reloading the screen
    void Awake()
    {
        // Set our respawn to where we're placed in the level, indicating this is the starting area
        respawnPosition = transform.position;
    }
    
    // Done in LateUpdate alongside physics calculations, to match the timing in other scripts
    void LateUpdate()
    {
        // Once the player drops below 0 health, we die
        if (PlayerManager.instance.playerHealth.health <= 0)
        {
            Die();
        }
    }

    // 
    public void Die()
    {
        // canMove variable controls whether or not movement can be influenced through input, setting it to false
        // 'disables' movement so players can't move after dying
        PlayerManager.instance.playerMovement.canMove = false;

        PlayerManager.instance.playerHealth.health = 1;
        // Instantiates the text set in the inspector on death
        //youDiedText.SetActive(true);

        
        // Waits to reload the screen
        StartCoroutine(waitForText());
    }

    private IEnumerator waitForText()
    {
        // Waits for the same amount of time before destroying instantiatedYouDiedText.
        // Separated in case we want to add some kind of fade out function in the future,
        // in that case, un-comment timeTilTextDestroy and use it instead of timeTilRespawn in the destroy statement above
        Instantiate(GameOverObject, Camera.main.transform.position + Vector3.forward, Quaternion.identity,
        Camera.main.transform);
        yield return new WaitForSeconds(timeTilRespawn);
        
        /*
        // Reloads the level once we're done waiting, re-spawning the player back at the start
        Application.LoadLevel(Application.loadedLevel);
        */
        // We're no longer doing this, so instead the player is going to be respawned at checkpoints
        Respawn();
    }
    
    private void Respawn()
    {
        // Respawn at our last checkpoint position, stored in respawnPosition
        //Player.transform.position = respawnPosition;
        
        // TODO: Reset our health, mana, material, etc.
        
        
        // Everything is reset, so now we can move again!
        PlayerManager.instance.playerMovement.canMove = true;
        
        Application.LoadLevel(Application.loadedLevel);
    }
}
