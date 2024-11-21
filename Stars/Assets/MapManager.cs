using System;
using System.Collections;
using System.Collections.Generic;
using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] private AbstractMap map;
    [SerializeField] private GameObject markerPrefab;
    public void UpdateMap(Vector2 data)
    {
        if (map)
        {
            map.SetCenterLatitudeLongitude(new(data[0], data[1]));
            AddMarkerAtLocation(data[0], data[1]);
        }
        
    }
    
    
    // Method to add a marker to the map at a specific latitude and longitude
    void AddMarkerAtLocation(float latitude, float longitude)
    {
        var instance = Instantiate(markerPrefab);

        // Convert coordinates to world position relative to the map's center
        Vector3 worldPos = map.GeoToWorldPosition(new Vector2d(latitude, longitude), true);
        instance.transform.localPosition = worldPos.normalized * 10;
        
        // Set marker as a child of the map's transform
        instance.transform.SetParent(transform);
    }

}
