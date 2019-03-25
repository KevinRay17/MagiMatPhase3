using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialsManager : MonoBehaviour
{
    public static MaterialsManager instance;
    public static Dictionary<Material, MaterialClass> MaterialsDict = new Dictionary<Material, MaterialClass>();

    void Awake()
    {
        instance = this;
    }
    
    public static void AddMaterialScript(Material material, MaterialClass script)
    {
        if (!MaterialsDict.ContainsKey(material))
        {
            MaterialsDict.Add(material, script);
        }
    }

    public static MaterialClass GetMaterialScript(Material material)
    {
        MaterialsDict.TryGetValue(material, out MaterialClass newMaterialScript);
        return newMaterialScript;
    }
}
