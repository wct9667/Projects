using System.Collections;
using UnityEngine;
using System;


public class CalibrateStarField : MonoBehaviour
{
    [SerializeField] private Star referenceStar;


    [SerializeField] private Vector2EventChannelSO calibrateEvent;
    void Start()
    {
        // Start the location service to get device position
        StartCoroutine(StartLocationService());
    }

    IEnumerator StartLocationService()
    {
        // Wait until the location service initializes
        while (!Input.location.isEnabledByUser)
        {
            yield return new WaitForSeconds(1);
        }
        Input.location.Start();
    }

    internal void Calibrate()
    {
        if (Input.location.status == LocationServiceStatus.Running)
        {
            // User's longitude and latitude
            float userLatitude = Input.location.lastData.latitude;
            float userLongitude = Input.location.lastData.longitude;
            
            calibrateEvent.RaiseEvent(new Vector2(userLatitude, userLongitude));


            // Get current UTC time
            DateTime utcNow = DateTime.UtcNow;

            // Reference star's RA/Dec (Polaris) 
            float referenceRA = 2.5303f; // Right Ascension of Polaris in hours
            float referenceDec = 89.2641f;//(float)(referenceStar.declination * (180 / Math.PI)); //89.2641f; // Declination of Polaris in degrees
            
            // Calculate Local Sidereal Time (LST) for longitude of the user
            double LST = CalculateLocalSiderealTime(utcNow, userLongitude);

            // Convert RA/Dec to Altitude/Azimuth
            Vector2 starAzAlt = CalculateStarAzimuthAltitude(referenceRA, referenceDec, userLatitude, LST);

            // Get the phone's azimuth (compass heading)
            float phoneAzimuth = Input.compass.trueHeading;

            // Rotate the starmap to align the reference star
            AlignStarField(starAzAlt, phoneAzimuth);
        }
    }
    
    
    internal void CalibrateWithData(float userLatitude, float userLongitude)
    {
        // Get current UTC time
        DateTime utcNow = DateTime.UtcNow;

        // Reference star's RA/Dec (Polaris) 
        float referenceRA = 2.5303f; // Right Ascension of Polaris in hours
        float referenceDec = 89.2641f;//(float)(referenceStar.declination * (180 / Math.PI)); //89.2641f; // Declination of Polaris in degrees
        
        // Calculate Local Sidereal Time (LST) for longitude of the user
        double LST = CalculateLocalSiderealTime(utcNow, userLongitude);

        // Convert RA/Dec to Altitude/Azimuth
        Vector2 starAzAlt = CalculateStarAzimuthAltitude(referenceRA, referenceDec, userLatitude, LST);

        // Get the phone's azimuth (compass heading)
        float phoneAzimuth = Input.compass.trueHeading;

        // Rotate the starmap to align the reference star
        AlignStarField(starAzAlt, phoneAzimuth);
    }
    
    // Calculate the reference star's azimuth and altitude
    Vector2 CalculateStarAzimuthAltitude(float ra, float dec, float latitude, double lst)
    {
        float HA = (float)lst - ra;  // Hour Angle

        // Calculate altitude
        float sinAlt = Mathf.Sin(dec * Mathf.Deg2Rad) * Mathf.Sin(latitude * Mathf.Deg2Rad)
                       + Mathf.Cos(dec * Mathf.Deg2Rad) * Mathf.Cos(latitude * Mathf.Deg2Rad) * Mathf.Cos(HA * Mathf.Deg2Rad);
        float altitude = Mathf.Asin(sinAlt) * Mathf.Rad2Deg;

        // Calculate azimuth
        float cosAz = (Mathf.Sin(dec * Mathf.Deg2Rad) - Mathf.Sin(altitude * Mathf.Deg2Rad) * Mathf.Sin(latitude * Mathf.Deg2Rad))
                      / (Mathf.Cos(altitude * Mathf.Deg2Rad) * Mathf.Cos(latitude * Mathf.Deg2Rad));
        float azimuth = Mathf.Acos(cosAz) * Mathf.Rad2Deg;
        
        // degree correction
        if (Mathf.Sin(HA * Mathf.Deg2Rad) > 0)
        {
            azimuth = 360f - azimuth; 
        }

        return new Vector2(azimuth, altitude);
    }

    // Rotate starmap to align reference star
    void AlignStarField(Vector2 starAzAlt, float phoneAzimuth)
    {
        // Calculate rotation based on azimuth
        Quaternion azimuthRotation = Quaternion.Euler(0, starAzAlt.x - phoneAzimuth, 0);

        // Calculate rotation based on altitude
        Quaternion altitudeRotation = Quaternion.Euler(starAzAlt.y, 0, 0);

        // Combine rotations with the device's tilt
        Quaternion finalRotation = azimuthRotation * altitudeRotation;

        // Apply the rotation to the entire starmap
        transform.rotation = finalRotation;
    }
    
    // Speed factor (e.g., 3600 to make 1 second equal to 1 hour of real time)
    public float speedFactor = 3600f;

    // Earth's rotation in degrees per hour (360 / 24)
    private float earthRotationSpeedPerHour = 15f;

    void Update()
    {
        // Speed up rotation by multiplying by speedFactor
        float adjustedSpeed = earthRotationSpeedPerHour * speedFactor;
        transform.Rotate(Vector3.up, adjustedSpeed * Time.deltaTime);
    }

    private double CalculateLocalSiderealTime(DateTime utcNow, float userLongitude)
    {
        // 2. Calculate Julian Date
        double JD = CalculateJulianDate(utcNow);
        
        // 3. Calculate GMST
        double GMST = CalculateGMST(JD, utcNow);
        
        // 4. Convert GMST to Local Sidereal Time (LST)
        double LST = GMST + (userLongitude / 15.0);
        LST = LST % 24; // Normalize LST between 0 and 24 hours
        
        Debug.Log("Local Sidereal Time: " + LST + " hours");
        return LST;
    }

    // Calculate Julian Date (JD) for the given UTC time
    double CalculateJulianDate(DateTime utcTime)
    {
        int year = utcTime.Year;
        int month = utcTime.Month;
        int day = utcTime.Day;
        
        if (month <= 2)
        {
            year -= 1;
            month += 12;
        }

        int A = year / 100;
        int B = 2 - A + (A / 4);
        double JD = (int)(365.25 * (year + 4716)) + (int)(30.6001 * (month + 1)) + day + B - 1524.5;

        // Add fractional day
        double fractionalDay = (utcTime.Hour + utcTime.Minute / 60.0 + utcTime.Second / 3600.0) / 24.0;
        JD += fractionalDay;

        return JD;
    }

    // Calculate Greenwich Mean Sidereal Time (GMST)
    //GMST=6.697374558+0.06570982441908×(JD−2451545.0)+1.00273790935×
    double CalculateGMST(double JD, DateTime utcTime)
    {
        double d = JD - 2451545.0; // Days since J2000 epoch

        // GMST at 0h UT
        double GMST = 6.697374558 + 0.06570982441908 * d;

        // Add the time passed since 0h UT in hours
        double UT = utcTime.Hour + utcTime.Minute / 60.0 + utcTime.Second / 3600.0;
        GMST += 1.00273790935 * UT;

        // Normalize GMST between 0 and 24 hours
        GMST = GMST % 24.0;
        if (GMST < 0)
            GMST += 24.0;

        return GMST;
    }
    
    
}
