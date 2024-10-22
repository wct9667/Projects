using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawingConstellations : MonoBehaviour
{
    [Header("Collision Detection")]
    [SerializeField] private float maxDistance = 100f; // Maximum distance for the raycast
    [SerializeField] private LayerMask layer;
    [SerializeField] private ConstellationManager _constellationManager;

    private Camera camera;
    private int currentIndex = -1;
    
    private bool constellationsDrawingEnabled;
    private bool constellationsEnabled;
    
    private void Start()
    {
        camera = Camera.main;
        currentIndex = _constellationManager.ConstellationNum - 1;
    }
    
    //allows a raycast to hit a star and show its related constellations
    public void EnableRayCastNormalConstellations()
    {
        constellationsEnabled = !constellationsEnabled;
        if (!constellationsEnabled) constellationsDrawingEnabled = false;
    }
    
    //allows a raycast to hit a star and show its related constellations
    public void EnableRayCastDrawConstellations()
    {
        if(!constellationsEnabled) return;
        constellationsDrawingEnabled = !constellationsDrawingEnabled;
        if(constellationsDrawingEnabled)  currentIndex++;
        
    }

    void Update()
    {
        if (!constellationsEnabled && !constellationsDrawingEnabled) return;
        
        // Create a ray from the camera pointing forward
        Ray ray = new Ray(camera.transform.position, camera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, layer))
        {
            Debug.Log("Hit object: " + hit.collider.name);
            Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.green);
            Star star = hit.collider.gameObject.GetComponent<Star>();
            
            
            //add the star to a new constellation with the constellation manager
            if (constellationsDrawingEnabled)
            {
                _constellationManager.AddConstellation(currentIndex, (int)star.catalogNumber);
                star.SetScale();
            }

            if(constellationsEnabled || constellationsDrawingEnabled) star.ActivateConstellation();
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