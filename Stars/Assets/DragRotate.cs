using Mapbox.Unity.Map;
using Mapbox.Utils;
using UnityEngine;
using UnityEngine.Rendering;


    public class DragRotate : MonoBehaviour
    {
        [SerializeField]
        RectTransform _rawImageTransform; // Reference to the RawImage's RectTransform (the planet map)

        [SerializeField]
        Transform _objectToRotate; // The object to rotate

        [SerializeField] private AbstractMap map;

        [SerializeField]
        Camera _camera; // The camera that renders the planet to the RawImage

        [SerializeField]
        float _multiplier = 0.1f; // Rotation sensitivity

        [SerializeField]
        float _zoomSpeed = 0.1f; // Speed of zooming in/out
        [SerializeField]
        float _minZoom = 5f; // Minimum camera zoom distance
        [SerializeField]
        float _maxZoom = 20f; // Maximum camera zoom distance

        private Vector2 _startTouchPosition; // Store start touch position as Vector2
        private bool _isTouchingMap = false;

        [SerializeField] private Vector2EventChannelSO calibrateEvent;

        private void Update()
        {
            // Get the current mouse position in screen space as Vector2
            Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            // Step 1: Check if the mouse is over the RawImage UI element
            if (RectTransformUtility.RectangleContainsScreenPoint(_rawImageTransform, screenPosition))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    // Record the start position when the drag starts
                    _startTouchPosition = screenPosition;
                    _isTouchingMap = true;
                }

                if (Input.GetMouseButton(0) && _isTouchingMap)
                {

                    //screen pos to camera ortho space
                    
                    //raycast to planet
                    
                    //get lat and long

                    Ray ray = new Ray();
                    ray.origin= _camera.transform.position;
                    ray.direction = _camera.transform.forward;
                    
                    Debug.DrawRay(ray.origin, ray.direction * 1000, Color.red);
                    // Step 3: Raycast to the planet
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, 8))
                    {
                        // Step 4: Get the world position where the ray hits the globe
                        Vector3 hitPoint = hit.point;

                        // Step 5: Convert the hit point to latitude and longitude
                        // Transform hitPoint into the globe's local space
                        Vector3 localHitPoint = _objectToRotate.InverseTransformPoint(hitPoint);

                        // Calculate latitude and longitude from the local hit point
                        float latitude = Mathf.Asin(localHitPoint.y / 10) * Mathf.Rad2Deg;
                        float longitude = Mathf.Atan2(localHitPoint.z, localHitPoint.x) * Mathf.Rad2Deg;
                        
                        // Log the latitude and longitude
                        Debug.Log($"Latitude: {latitude}, Longitude: {longitude}");
                    }


                   
                    
                    // Step 2: Calculate the drag delta in screen space (Vector2)
                    Vector2 dragDelta = screenPosition - _startTouchPosition;

                    // Step 3: Calculate rotation based on drag
                    float rotationX = dragDelta.y * _multiplier; // Vertical drag affects X-axis
                    float rotationY = -dragDelta.x * _multiplier; // Horizontal drag affects Y-axis

                    // Step 4: Apply rotation to the object (world-space)
                    _objectToRotate.Rotate(Vector3.right, rotationX, Space.World);
                    _objectToRotate.Rotate(Vector3.up, rotationY, Space.World);

                    // Update the start touch position for smooth movement
                    _startTouchPosition = screenPosition;
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                // Reset touch state when the user releases the mouse button
                _isTouchingMap = false;
            }

         }
    }

