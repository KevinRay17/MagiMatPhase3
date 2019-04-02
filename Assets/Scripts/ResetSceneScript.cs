using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetSceneScript : MonoBehaviour
{
    void Update()
    {
        //reset scene for testing
        if(Input.GetKeyDown(KeyCode.R))
        {
            ResetScene();
        }
    }
    
    void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
