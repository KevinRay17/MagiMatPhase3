using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;


// this script is responsible for the managenent of the now coined "mana" AKA resource amount
public class ResourceController : MonoBehaviour
{

    //float responsible for the amount of mana current had
    static public float currentMana;
    private float oneTimeUse;

    //private PlayerActions PA;
    //the highest amount of mana possible to have
    public float maxMana = 100f;

    //the amount that is drained every frame
    private float passiveManaDrain = 1f/15f;

    /*
    //the mana cost of a small action
    public float smallAction;

    //the mana cost of a medium action
    public float mediumAction;

    //the mana cost of a large action
    public float largeAction;

    //Cooldown timer for attacking
    public float attackCD = 0.5f;

    //boolean for attack availability
    public bool attackStatus;

    //Cooldown timer for special ability
    public float specialCD = 2f;

    //boolean for special ability availability lol
    public bool specialStatus;  */

    //MANUALLY ADJUST THIS VARIABLE
    public const int numOfCooldowns = 2;
    
    //setting constant floats for the length of cooldown timers
    public const float attackCooldown = 0.5f;
    public const float specialCooldown = 1f;
    
    //setting constant ints to track index of cooldown timers
    public  int attackIndex = 0;
    public  int specialIndex = 1;

    //2d array, first number is for collumns, second number is for rows (which should be equal to the number of cooldowns)
    private float [,] cooldowns = new float[2,2];

    // Start is called before the first frame update
    void Start()
    {
        

        currentMana = 0; //setting currentMana to zero at the beginning
       
        /*
        largeAction = maxMana / 3f; //large actions cost a third of max mana
        mediumAction = maxMana / 6f; //medium actions cost a sixth of max mana
        smallAction = maxMana / 10f; //small actions cost a tenth of max mana

        currentMana = 100f;
        */

        //Assigning the values of the 2D array so that all values in collumn 0 are equal to 0
        // and the values in collumn 1 are equal to the cooldowntimers
        cooldowns[0, attackIndex] = 0;
        cooldowns[1, attackIndex] = attackCooldown;
        cooldowns[0, specialIndex] = 0;
        cooldowns[1, specialIndex] = specialCooldown;
    }

    // Update is called once per frame
    void Update()
    {   /*
        adjustCDTimers();
        setAttackStatus();
        setSpecialStatus();
        */
        //Debug.Log(oneTimeUse);
        
        if (currentMana > 0)
        {
            currentMana -= passiveManaDrain*10; //constantly drain mana every frame >:(
        }

        if (currentMana < 0)
        {
            /*
            oneTimeUse = 1;
            //if (Input.GetKey(KeyCode.Mouse1) || Input.GetKey(KeyCode.Q))\
            if (PlayerManager.instance.playerActions.didAttack) 
            {
                oneTimeUse = 0;
            }
            if (oneTimeUse == 0)

            {
                PlayerManager.instance.material = Material.None;
            }
            */
            PlayerManager.instance.material = Material.None;
        }


        for (var i = 0; i < numOfCooldowns; i++)
        {
            cooldowns[0, i] += Time.deltaTime;
        }
    }

    //function used for assigning attackStatus boolean
    //if the cooldown is greater than 0, attackStatus is false, meaning it is still on cool down
    //if the cooldown is less than 0, attackStatus is true, meaning it is off cooldown
    //when the cooldown is less that -1, set it to -1
    /*
    public void setAttackStatus()
    {
        if (attackCD > 0)
        {
            attackStatus = false;
        }

        else
        {
            attackStatus = true;
        }

        if (attackCD <= -1f)
        {
            attackCD = -1f;
        }
    }
    
    //function used for assigning specialStatus boolean
    //if the cooldown is greater than 0, attackStatus is false, meaning it is still on cool down
    //if the cooldown is less than 0, attackStatus is true, meaning it is off cooldown
    //when the cooldown is less that -1, set it to -1
    public void setSpecialStatus()
    {
        if (specialCD > 0)
        {
            specialStatus = false;
        }

        else
        {
            specialStatus = true;
        }

        if (specialCD <= -1f)
        {
            specialCD = -1f;
        }
    }

    //function that makes the cooldowns actual timers
    public void adjustCDTimers()
    {
        attackCD -= Time.deltaTime;
        specialCD -= Time.deltaTime;
    }
*/
    public bool isAvailable(int indexToCheck)
    {
        return cooldowns[0, indexToCheck] >= cooldowns[1, indexToCheck] && hasMana();
    }

    public void resetCooldown(int indexToReset)
    {
        cooldowns[0, indexToReset] = 0;
    }

    public bool hasMana()
    {
        return currentMana >= 0;
    }
    
}