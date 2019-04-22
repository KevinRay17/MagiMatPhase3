using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public CameraScript cameraScriptVariable;
    public Transform mainCamera;
    private float cameraY;
    public Transform camStartPos;
    public Transform camEndPos;
    public GameObject ForlornLogo;
    public float startAlphaLogo = 0;
    public float endAlphaLogo = 255;
    
    

    public float numberOfSeconds;
    public float numberOfSecondsFade;
   

    void Start()
    {
        cameraScriptVariable.enabled = false;
        cameraY = mainCamera.transform.position.y;
        StartCoroutine(CameraMoveDown(camStartPos.position, camEndPos.position, numberOfSeconds));
        //StartCoroutine(LogoFadeIn(startAlphaLogo, endAlphaLogo, numberOfSecondsFade));
        ForlornLogo.GetComponent<Renderer>().enabled = false;
       
       

    }

    void Update()
    {
       
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(1);
        }
    }

    IEnumerator CameraMoveDown(Vector3 startPosition, Vector3 endingPosition, float numSeconds)
    {
        var t = 0f;
        
        
        while (t < 1)
        {
            mainCamera.transform.position = Vector3.Lerp(startPosition, endingPosition, t);
            t += Time.deltaTime * (1 / numSeconds);
            yield return null;
        }
        
        // anything that happens at the end
        ForlornLogo.GetComponent<Renderer>().enabled = true;
        StartCoroutine(LogoFadeIn(startAlphaLogo, endAlphaLogo, numberOfSecondsFade));
        //ForlornLogo.GetComponent<Renderer>().enabled = true;
        
        
        
    }

    IEnumerator LogoFadeIn(float startAlpha,float endAlpha, float numSecondsFade)
    {
        var t = 0f;
        Color myColor = ForlornLogo.GetComponent<SpriteRenderer>().color;
        while (t < 1)
        {
            myColor.a =
                Mathf.Lerp(startAlpha,endAlpha, Time.deltaTime * t);
            ForlornLogo.GetComponent<SpriteRenderer>().color = myColor;
            t += Time.deltaTime * (1 / numSecondsFade);
            yield return null;
        }

    }
}
