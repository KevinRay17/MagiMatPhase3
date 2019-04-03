using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// this script is responsible for the managenent of the now coined "mana" AKA resource amount
public class ResourceController : MonoBehaviour
{
    
    //float responsible for the amount of mana current had
    public float currentMana;

    //the highest amount of mana possible to have
    public float maxMana = 100f;
    
    //the amount that is drained every frame
    public float passiveManaDrain = 1f/120f;

    //the mana cost of a small action
    public float smallAction;

    //the mana cost of a medium action
    public float mediumAction;

    //the mana cost of a large action
    public float largeAction;
    
    // Start is called before the first frame update
    void Start()
    {
        currentMana = maxMana; //setting currentMana to max at the begging
        largeAction = maxMana / 3f; //large actions cost a third of max mana
        mediumAction = maxMana / 6f; //medium actions cost a sixth of max mana
        smallAction = maxMana / 10f; //small actions cost a tenth of max mana

    }

    // Update is called once per frame
    void Update()
    {
       currentMana -= passiveManaDrain; //constantly drain mana every frame >:(
    } 
}
