using Unity.VisualScripting;
using UnityEngine;

    public class DragRotate : MonoBehaviour
    {
        [SerializeField]
        RectTransform _rawImageTransform; // Reference to the RawImage's RectTransform (the planet map)

        [SerializeField]
        Transform _objectToRotate; // The object to rotate

        [SerializeField]
        Camera _camera; // The camera that renders the planet to the RawImage

        [SerializeField]
        float _multiplier = 0.1f; // Rotation sensitivity

        private Vector2 _startTouchPosition; // Store start touch position as Vector2
        private bool _isTouchingMap;

        [SerializeField] private Vector2EventChannelSO calibrateEvent;
        [SerializeField] private Vector2EventChannelSO duplicateCalibrateEvent;

        
        public void TriggerLocation()
        {
            Ray ray = new Ray();
                ray.origin = _camera.transform.position;
                ray.direction = _camera.transform.forward * 1000;

                Debug.DrawRay(ray.origin, ray.direction * 1000, Color.red);
                // Step 3: Raycast to the planet
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 8))
                {
                    Vector3 hitPoint = hit.point;

                    // Step 2: Transform the hit point into the globe's local space considering its rotation
                    Quaternion globeRotation = _objectToRotate.rotation; // Get the globe's rotation
                    Vector3 localHitPoint = Quaternion.Inverse(globeRotation) *
                                            (hitPoint - _objectToRotate.position);

                    // Step 3: Normalize the local hit point to the unit sphere
                    Vector3 normalizedLocalHitPoint = localHitPoint.normalized;

                    // Step 4: Calculate latitude and longitude from the normalized local hit point
                    float latitude =
                        Mathf.Asin(normalizedLocalHitPoint.y) * Mathf.Rad2Deg; // Latitude is based on Y-axis
                    float longitude = Mathf.Atan2(normalizedLocalHitPoint.z, normalizedLocalHitPoint.x) *
                                      Mathf.Rad2Deg; // Longitude is based on X and Z axes

                    // Log the latitude and longitude
                    Debug.Log($"Latitude: {latitude}, Longitude: {longitude}");
                    calibrateEvent.RaiseEvent(new Vector2(latitude, longitude));
                    duplicateCalibrateEvent.RaiseEvent(new Vector2(latitude, longitude));
                }
        }
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
                    // Step 2: Calculate the drag delta in screen space (Vector2)
                    Vector2 dragDelta = screenPosition - _startTouchPosition;

                    // Step 3: Calculate rotation based on drag
                    float rotationX = dragDelta.y * _multiplier; // Vertical drag affects X-axis
                    float rotationY = -dragDelta.x * _multiplier; // Horizontal drag affects Y-axis

                    // Step 4: Apply rotation to the object (world-space)
                    _objectToRotate.Rotate(Vector3.right, rotationX, Space.World);
                    _objectToRotate.Rotate(Vector3.up, rotationY, Space.World);
                    //screen pos to camera ortho space

                    //raycast to planet

                    //get lat and long
                    
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

