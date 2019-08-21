using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    GPS GPS;
    [Range(1, 50)]
    public float ARSceneRadius = 1;

    AugmentedScene[] Scenes;
    public AugmentedScene Scene;
    AugmentedScene DefaultScene;

    void Start()
    {
        GPS = FindObjectOfType<GPS>();
        Scenes = GetComponentsInChildren<AugmentedScene>();
        foreach (AugmentedScene sc in Scenes)
        {
            if (sc.name.Equals("Default") || sc.name.Equals("Default Scene")) DefaultScene = sc;
        }
        if (DefaultScene != null)
        {
            // Activate Default scene
            ChangeScene(DefaultScene);
        }
        else Debug.Log("SceneManager --- Default Scene not found");
    }

    void Update()
    {
        if (GPS != null)
        {
            // Active last scene if GPS is available
            if (GPS.IsEnabled())
            {
                AugmentedScene NearScene = GetNearestScene();
                if (NearScene != Scene) ChangeScene(NearScene);
            }
        }
    }

    void ChangeScene(AugmentedScene NewScene)
    {
        if (gameObject.activeSelf && gameObject.activeInHierarchy)
        {
            // Disable all scenes
            foreach (AugmentedScene scene in Scenes) scene.gameObject.SetActive(false);
            // Set new scene as active scene
            Scene = NewScene;
            Scene.gameObject.SetActive(true);
        }
    }

    AugmentedScene GetNearestScene()
    {
        AugmentedScene NearScene = Scene;
        float dist = -1;

        foreach (AugmentedScene scene in Scenes)
        {
            if (dist == -1)
            {
                dist = GPS.GetDistance(scene.Latitude, scene.Longitude, GPS.Lat, GPS.Lng);
                NearScene = scene;
            }
            else
            {
                float distance = GPS.GetDistance(scene.Latitude, scene.Longitude, GPS.Lat, GPS.Lng);
                if (distance < dist)
                {
                    dist = distance;
                    NearScene = scene;
                }
            }
        }

        // If near scene is not in the AR Scene Radius, we return the default scene
        if (DefaultScene != null)
        {
            if (dist > ARSceneRadius) return DefaultScene;
        }
        else Debug.Log("Missing Default Scene");

        return NearScene;
    }
}
