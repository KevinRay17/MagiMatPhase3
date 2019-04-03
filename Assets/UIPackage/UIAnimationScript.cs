using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimationScript : MonoBehaviour
{
    // The animator attached to this gameObject
    private Animator myAnimator;
    
    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerManager.instance.material == Material.None)
        {
            myAnimator.SetInteger("currentMaterial", 0);
        }
        else if (PlayerManager.instance.material == Material.Vine)
        {
            myAnimator.SetInteger("currentMaterial", 1);
        }
        else if (PlayerManager.instance.material == Material.Fire)
        {
            myAnimator.SetInteger("currentMaterial", 3);
        }
        else if (PlayerManager.instance.material == Material.Rock)
        {
            myAnimator.SetInteger("currentMaterial", 4);
        }
        else
        {
            return;
        }
    }
}
