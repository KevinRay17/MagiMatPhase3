using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManaBar : MonoBehaviour
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
        ourCurrentImage.fillAmount = ResourceController.currentMana/100f;
    }
}
