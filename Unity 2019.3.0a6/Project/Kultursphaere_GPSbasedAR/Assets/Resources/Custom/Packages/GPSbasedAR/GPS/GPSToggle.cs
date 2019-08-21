using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Class with public method to enable/disable GPS with a UI button
public class GPSToggle : MonoBehaviour
{
    GPS GPS;
    Text ButtonText;
    Button Button;

    public Color Enabled;
    public Color Disabled;

    void Start()
    {
        GPS = FindObjectOfType<GPS>();
        ButtonText = GetComponentInChildren<Text>();
        Button = GetComponent<Button>();

        if (GPS != null)
        {
            if (GPS.IsEnabled())
            {
                ButtonText.text = "Disable GPS";
                Button.image.color = Enabled;
            }
            else
            {
                ButtonText.text = "Enable GPS";
                Button.image.color = Disabled;
            }
        }
    }

    public void ToggleGPS()
    {
        if (GPS != null)
        {
            GPS.ToggleGPS();
            if (GPS.IsEnabled())
            {
                ButtonText.text = "Disable GPS";
                Button.image.color = Enabled;
            }
            else
            {
                ButtonText.text = "Enable GPS";
                Button.image.color = Disabled;
            }
        }
        else Debug.LogError("Missing GPS Object");
    }
}
