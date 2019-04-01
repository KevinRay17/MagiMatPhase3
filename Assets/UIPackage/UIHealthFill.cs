using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthFill : MonoBehaviour
{
    // Our current image, assigned in the inspector
    private Image ourCurrentImage;
    
    // Start is called before the first frame update
    void Start()
    {
        ourCurrentImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        ourCurrentImage.fillAmount = PlayerManager.instance.playerHealth.health / 5;
    }
}
