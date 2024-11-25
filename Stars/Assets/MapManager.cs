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
    
    void AddMarkerAtLocation(float latitude, float longitude)
    {
        if (!marker)
            marker = Instantiate(markerPrefab);
        
        
        float latRad = Mathf.Deg2Rad * latitude;
        float lonRad = Mathf.Deg2Rad * longitude;

      
        Vector3 localPos = new Vector3(
            Mathf.Cos(latRad) * Mathf.Cos(lonRad),
            Mathf.Sin(latRad),
            Mathf.Cos(latRad) * Mathf.Sin(lonRad)
        );
        
        localPos *= collider.radius;
        
        Vector3 worldPos = map.transform.TransformPoint(localPos);
        
        marker.transform.position = worldPos; 
        marker.transform.localScale = new Vector3(0.04f, 0.04f, 0.04f);
        
        marker.transform.SetParent(transform);
    }
}