using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawingConstellations : MonoBehaviour
{
    
    [SerializeField] private float maxDistance = 100f; // Maximum distance for the raycast
    [SerializeField] private LayerMask layer;

    private Camera camera;
    
    private bool constellationsDrawingEnabled;
    private bool constellationsEnabled;
    private void Start()
    {
        camera = Camera.main;
    }
    
    //allows a raycast to hit a star and show its related constellations
    public void EnableRayCastNormalConstellations()
    {
        constellationsEnabled = !constellationsEnabled;
    }

    void Update()
    {
        // Check if the left mouse button is clicked
        if (!constellationsEnabled) return;

        // Create a ray from the camera pointing forward
        Ray ray = new Ray(camera.transform.position, camera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, layer))
        {
            // If it hits, log the name of the object
            Debug.Log("Hit object: " + hit.collider.name);
            Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.green);
            Star star = hit.collider.gameObject.GetComponent<Star>();
            star.ActivateConstellation();
        }
        else
        {
            Debug.Log("No collision with 'Star' layer detected.");
            Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.red);
        }
    }
    
    public void Zoom(float increment)
    {
        camera.fieldOfView = Mathf.Clamp(camera.fieldOfView - increment, 20f, 100f);
    }

}
