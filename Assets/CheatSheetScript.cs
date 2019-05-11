using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatSheetScript : MonoBehaviour
{
    public GameObject CheatSheet;

    private bool paused = false;
 
    void Start()
    {
        CheatSheet.SetActive(false);
    }

    void Update()
    {

            
        if (InputManager.GetPauseButton())
        {
            paused = !paused;
            SetPause();
        }
        
    }

    void SetPause()
    {
        if (paused)
        {
            CheatSheet.SetActive(true);
            Time.timeScale = 0;
        }
        else 
        {
            CheatSheet.SetActive(false);
            Time.timeScale = 1;
        }
    }
}
