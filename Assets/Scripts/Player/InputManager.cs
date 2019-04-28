using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    
    public enum InputDevice
    {
        Keyboard, Controller
    }
    
    public InputDevice inputDevice;

    [Header("Keyboard Binds")] 
    public KeyCode keyboardJump;
    public KeyCode keyboardThrow;
    public KeyCode keyboardAttack;
    public KeyCode keyboardSpecial;
    
    [Header("Controller Binds")]
    public string controllerJump;
    public string controllerThrow;
    public string controllerAttack;
    public string controllerSpecial;

    public static bool GetJumpButton()
    {
        if (InputManager.instance.inputDevice == InputDevice.Keyboard)
        {
            return Input.GetButton(instance.keyboardJump.ToString());
        }
        else
        {
            return Input.GetButton(instance.controllerJump);
        }
    }
    
    public static bool GetJumpButtonDown()
    {
        if (InputManager.instance.inputDevice == InputDevice.Keyboard)
        {
            return Input.GetButtonDown(instance.keyboardJump.ToString());
        }
        else
        {
            return Input.GetButtonDown(instance.controllerJump);
        }
    }
    
    public static bool GetThrowButtonDown()
    {
        if (InputManager.instance.inputDevice == InputDevice.Keyboard)
        {
            return Input.GetButtonDown(instance.keyboardThrow.ToString());
        }
        else
        {
            return Input.GetButtonDown(instance.controllerThrow);
        }
    }
    
    public static bool GetAttackButtonDown()
    {
        if (InputManager.instance.inputDevice == InputDevice.Keyboard)
        {
            return Input.GetButtonDown(instance.keyboardAttack.ToString());
        }
        else
        {
            return Input.GetButtonDown(instance.controllerAttack);
        }
    }
    
    public static bool GetSpecialButtonDown()
    {
        if (InputManager.instance.inputDevice == InputDevice.Keyboard)
        {
            return Input.GetButtonDown(instance.keyboardSpecial.ToString());
        }
        else
        {
            return Input.GetButtonDown(instance.controllerSpecial);
        }
    }

    public static float GetMovementAxisHorizontal()
    {
        if (InputManager.instance.inputDevice == InputDevice.Keyboard)
        {
            return Input.GetAxisRaw("Horizontal");
        }
        else
        {
            return Input.GetAxisRaw("LeftJSHorizontal");
        }
    }

    public static float GetMovementAxisVertical()
    {
        if (InputManager.instance.inputDevice == InputDevice.Keyboard)
        {
            return Input.GetAxisRaw("Vertical");
        }
        else
        {
            return Input.GetAxisRaw("LeftJSVertical");
        } 
    }

    public static Vector2 GetAimDirection()
    {
        if (InputManager.instance.inputDevice == InputDevice.Keyboard)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 playerPos = transform.position;
            return (mousePos - playerPos).normalized;
        }
        else
        {
            return new Vector2(Input.GetAxisRaw("RightJSHorizontal"), Input.GetAxisRaw("RightJSVertical"));
        } 
    }
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    
    
}
