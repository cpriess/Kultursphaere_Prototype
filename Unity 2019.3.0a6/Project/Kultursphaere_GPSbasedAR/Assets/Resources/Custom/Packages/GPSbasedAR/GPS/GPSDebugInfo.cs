using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Debug class to write GPS data to UI text elements.
// The purpose is to make the requested GPS data visible on mobile devices to test the application
public class GPSDebugInfo : MonoBehaviour
{
    #region Variables

    // Instance of GPS class
    GPS GPS;

    // UI text elements
    Text Lat;
    Text Lng;
    Text Alt;
    Text HorAcc;
    Text Time;

    // ? update process enabled/disabled
    bool Process = false;

    #endregion


    #region Unity Events

    // Use this for initialization
    void Start()
    {
        if (InitGPSDebug())
        {
            Debug.Log("GPS Debug Info initialization successful");
            Process = true;
        }
        else if (GPS != null) Debug.LogError("GPS Debug Info Error: Missing some UI text elements");
        else Debug.LogError("GPS Debug Info Error: Missing GPS Object");
    }

    // Update is called once per frame
    void Update()
    {
        // Continuously update GPS data on UI text elemts
        if (Process) ShowGPSInfo();
    }

    #endregion


    #region GPSDebugInfo

    // Initialize UI text elements 
    bool InitGPSDebug()
    {
        GPS = FindObjectOfType<GPS>();
        if (GPS != null)
        {
            Text[] Texts = GetComponentsInChildren<Text>();
            foreach (Text text in Texts)
            {
                switch (text.text)
                {
                    case "Lat":
                        Lat = text;
                        break;
                    case "Lng":
                        Lng = text;
                        break;
                    case "Alt":
                        Alt = text;
                        break;
                    case "HorAcc":
                        HorAcc = text;
                        break;
                    case "Time":
                        Time = text;
                        break;
                    default:
                        // default...
                        break;
                }
            }
            if (Lat != null && Lng != null && Alt != null && HorAcc != null && Time != null) return true;
        }
        return false;
    }

    // Update UI text elements with current GPS data
    void ShowGPSInfo()
    {
        Lat.text = "Lat: " + GPS.Lat.ToString();
        Lng.text = "Lng: " + GPS.Lng.ToString();
        Alt.text = "Alt: " + GPS.Alt.ToString();
        HorAcc.text = "HorAcc: " + GPS.HorAcc.ToString();
        Time.text = "Time: " + GPS.GPS_Time.ToString();

        // format string
        //Lat.text = "Lat: " + GPS.Lat.ToString("0.00");
    }

    #endregion
}
