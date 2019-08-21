using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AugmentedScene : MonoBehaviour
{
    public string Name = "New Augmented Scene";
    public float Latitude = 0;
    public float Longitude = 0;

    bool IsActive = false;

    private void Start()
    {
        if (Name == null || Name == "") Name = gameObject.name;
    }
}
