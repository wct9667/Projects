using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] private GameObject markerPrefab;
    [SerializeField] private SphereCollider collider;
    [SerializeField] private Transform map;

    private GameObject marker;

    public void UpdateMap(Vector2 data)
    {
        AddMarkerAtLocation(data[0], data[1]);
    }

    // Method to add a marker to the map at a specific latitude and longitude
    void AddMarkerAtLocation(float latitude, float longitude)
    {
        if (!marker)
            marker = Instantiate(markerPrefab);
        

        // Convert latitude and longitude to radians
        float latRad = Mathf.Deg2Rad * latitude;
        float lonRad = Mathf.Deg2Rad * longitude;

        // Globe radius (adjust with map's scale)
        //float _globeRadius = collider.radius * Mathf.Max(map.transform.lossyScale.x, map.transform.lossyScale.y, map.transform.lossyScale.z);

        // Convert latitude/longitude to Cartesian coordinates
        Vector3 localPos = new Vector3(
            Mathf.Cos(latRad) * Mathf.Cos(lonRad),
            Mathf.Sin(latRad),
            Mathf.Cos(latRad) * Mathf.Sin(lonRad)
        );

        // Scale to match the radius of the globe
        localPos *= collider.radius;

        // Transform the local position to world space, taking map rotation into account
        Vector3 worldPos = map.transform.TransformPoint(localPos);

        // Set the position and scale for the instance
        marker.transform.position = worldPos; // Place it directly in world space
        marker.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

        // Parent the instance to this object (optional)
        marker.transform.SetParent(transform);
    }
}