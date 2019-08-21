using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneDebugInfo : MonoBehaviour
{
    SceneManager SceneManager;
    Text ScText;
    Text DistText;
    string ScStr;
    string DistStr;

    void Start()
    {
        Text[] texts = GetComponentsInChildren<Text>();
        foreach (Text txt in texts)
        {
            if (txt.name == "Text Scene") ScText = txt;
            else if (txt.name == "Text Distance") DistText = txt;
        }
        ScText = GetComponentInChildren<Text>();
        SceneManager = FindObjectOfType<SceneManager>();
    }

    void Update()
    {
        if (ScText != null && DistText != null && SceneManager != null && SceneManager.Scene != null)
        {
            GPS gps = FindObjectOfType<GPS>();

            ScStr = "Scene: " + SceneManager.Scene.Name;
            DistStr = "Dist: " + gps.GetDistance(SceneManager.Scene.Latitude, SceneManager.Scene.Longitude, gps.Lat, gps.Lng);

            if (ScText.text != ScStr) ScText.text = ScStr;
            if (DistText.text != DistStr) DistText.text = DistStr;
        }
    }
}
