using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Material
{
    None,
    Vine,
    Fire,
    Rock
}

public abstract class MaterialClass : MonoBehaviour
{
    public Material material;

    protected virtual void Start()
    {
        MaterialsManager.AddMaterialScript(material, this);
    }

    public abstract void Attack(GameObject player);
    
    public abstract void Special(GameObject player);
}
