using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// TODO: Modify PlayerManager, move canMove variable to PlayerManager
public class Death : MonoBehaviour
{
    // Text enabled telling the player they died
    public GameObject youDiedText;

    public float timeTilRespawn;

    // youDiedText is set to false as soon as the scene is reloaded so there's no issues on reloading the screen
    void Awake()
    {
        youDiedText.SetActive(false);
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

        // Instantiates the text set in the inspector on death
        youDiedText.SetActive(true);
        
        // Waits to reload the screen
        StartCoroutine(waitForText());
    }

    private IEnumerator waitForText()
    {
        // Waits for the same amount of time before destroying instantiatedYouDiedText.
        // Separated in case we want to add some kind of fade out function in the future,
        // in that case, un-comment timeTilTextDestroy and use it instead of timeTilRespawn in the destroy statement above
        yield return new WaitForSeconds(timeTilRespawn);
        
        // Reloads the level once we're done waiting, re-spawning the player back at the start
        Application.LoadLevel(Application.loadedLevel);
    }
}