using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using UnityEngine;

public class MapClickHandler : MonoBehaviour
{
    [SerializeField]
    private AbstractMap map; // Reference to the Mapbox map in the scene.

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Detect left mouse click.
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // Convert world position to lat/lon
                Vector2d latLon = map.WorldToGeoPosition(hit.point);
                Debug.Log($"Latitude: {latLon.x}, Longitude: {latLon.y}");
                
                // Add your logic here to use or display the selected coordinates.
            }
        }
    }
}