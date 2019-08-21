using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GPS : MonoBehaviour
{
    #region - Variables

    // Request Interval
    public float Interval = 5; // sec

    // GPS data
    public float Lat;
    public float Lng;
    public float Alt;
    public float HorAcc;
    public double GPS_Time;

    // Default location if device has no GPS
    // FH Kiel, Medien, LINK
    public float Default_Lat = 54.3302f;
    public float Default_Lng = 10.17894f;

    // Turn GPS on/off
    private bool Enabled = true;
    
    // Async query
    private IEnumerator GPSQuery;

    // Time of last request
    float TimeStamp = 0;

    #endregion


    #region - Unity Events

    // Use this for initialization
    void Start()
    {
        // Set default location to FH Kiel, Medien, LINK
        Default_Lat = 54.3302f;
        Default_Lng = 10.17894f;

        // Immediately run a GPS query on application start 
        if (IsEnabled()) QueryCoordinates();
    }

    // Update is called once per frame
    void Update()
    {
        // GPS is enabled
        if (IsEnabled())
        {
            // Wait for Update Interval
            if (Time.realtimeSinceStartup > TimeStamp + Interval)
            {
                // If no query exists, request device location
                if (GPSQuery == null && Input.location.status != LocationServiceStatus.Running) QueryCoordinates();
                TimeStamp = Time.realtimeSinceStartup;
            }
        }
        // GPS is disabled, but there still runs a query
        else if (GPSQuery != null)
        {
            // Stop the last GPS request
            StopCoroutine(GPSQuery);
            GPSQuery = null;
        }
    }

    #endregion


    #region - Utility

        // Calculates distance between two sets of coordinates, taking into account the curvature of the earth.
        public float GetDistance(float lat1, float lon1, float lat2, float lon2)
        {
            double distance = 0;
            var R = 6378.137; // Radius of earth in KM
            var dLat = lat2 * Mathf.PI / 180 - lat1 * Mathf.PI / 180;
            var dLon = lon2 * Mathf.PI / 180 - lon1 * Mathf.PI / 180;
            float a = Mathf.Sin(dLat / 2) * Mathf.Sin(dLat / 2) +
              Mathf.Cos(lat1 * Mathf.PI / 180) * Mathf.Cos(lat2 * Mathf.PI / 180) *
              Mathf.Sin(dLon / 2) * Mathf.Sin(dLon / 2);
            var c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));
            distance = R * c;
            distance = distance * 1000f; // meters
            //convert distance from double to float
            float distanceFloat = (float)distance;
            return distanceFloat;
        }

    #endregion


    #region - GPS

    // ? Enabled/Disabled
    public bool IsEnabled()
    {
        return Enabled;
    }
    
    // Enable/Disable GPS
    public void ToggleGPS()
    {
        Enabled = !Enabled;
        Debug.Log("GPS Enabled: " + IsEnabled().ToString());
    }
    
    // Start location request
    void QueryCoordinates()
    {
        Debug.Log("Starting query for coordinates");
        GPSQuery = GetCoordinates();
        StartCoroutine(GPSQuery);
    }
    
    // Async function to request location of device
    IEnumerator GetCoordinates()
    {
        while (IsEnabled())
        {
            // check if user has location service enabled
            if (!Input.location.isEnabledByUser)
            {
                print("Device location is not enabled");
                // Default Lat/Lng
                Lat = Default_Lat;
                Lng = Default_Lng;
                print("Location ist set to: Lat: " + Lat + ", Lng: " + Lng);
                yield break;
            }
    
            // Start service before querying location
            Input.location.Start(1f, .1f);
    
            // Wait until service initializes
            int maxWait = 20;
            while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
            {
                yield return new WaitForSeconds(1);
                maxWait--;
            }
    
            // Service didn't initialize in 20 seconds
            if (maxWait < 1)
            {
                print("Timed out");
                yield break;
            }
    
            // Connection has failed
            if (Input.location.status == LocationServiceStatus.Failed)
            {
                print("Unable to determine device location");
                yield break;
            }
            else
            {
                // Access granted and location value could be retrieved
                print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
    
                // Overwrite current lat and lon everytime
                Lat = Input.location.lastData.latitude;
                Lng = Input.location.lastData.longitude;
                Alt = Input.location.lastData.altitude;
                HorAcc = Input.location.lastData.horizontalAccuracy;
                GPS_Time = Input.location.lastData.timestamp;
            }
            GPSQuery = null;
            Input.location.Stop();
        }
    }

    #endregion
}