using System;
using System.Collections;
using System.Collections.Generic;
using Mapbox.Map;
using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] private AbstractMap map;
    [SerializeField] private GameObject markerPrefab;
    [SerializeField] private SphereCollider collider;
    
    

    private GameObject marker;
    public void UpdateMap(Vector2 data)
    {
        if (map)
        {
            map.Initialize(new Vector2d(0,0), 3);
            Debug.Log(data[0] + ""+  data[1]);
            //map.SetCenterLatitudeLongitude(new(data[0], data[1]));
            AddMarkerAtLocation(data[0], data[1]);
        }
        
    }
    

    // Method to add a marker to the map at a specific latitude and longitude
    void AddMarkerAtLocation(float latitude, float longitude)
    {
        GameObject instance;
        if (!marker)
            instance = Instantiate(markerPrefab);
        else 
            instance = marker;

        // Convert latitude and longitude to radians
        float latRad = Mathf.Deg2Rad * latitude;
        float lonRad = Mathf.Deg2Rad * longitude;

        // Globe radius (adjust with map's scale)
        float _globeRadius = collider.radius * map.transform.lossyScale.z;
    
        // Convert latitude/longitude to Cartesian coordinates
        float x = _globeRadius * Mathf.Cos(latRad) * Mathf.Cos(lonRad);
        float y = _globeRadius * Mathf.Cos(latRad) * Mathf.Sin(lonRad);
        float z = _globeRadius * Mathf.Sin(latRad);

        // Calculate the final position (offset by the map's position)
        Vector3 pos = new Vector3(x, z, y) + map.transform.position;

        // Set the position and scale for the instance
        instance.transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
        instance.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        instance.transform.SetParent(transform);
    }

    
    
}
