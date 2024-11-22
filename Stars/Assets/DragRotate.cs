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
        if (Input.touchCount == 1 && !isZooming) // Single touch for rotation
        {
            Touch touch = Input.GetTouch(0);

            if (RectTransformUtility.RectangleContainsScreenPoint(_rawImageTransform, touch.position))
            {
                if (touch.phase == TouchPhase.Began)
                {
                    _startTouchPosition = touch.position;
                    _isTouchingMap = true;
                }

                if (touch.phase == TouchPhase.Moved && _isTouchingMap)
                {
                    Vector2 dragDelta = touch.position - _startTouchPosition;

                    float rotationX = dragDelta.y * _multiplier; // Vertical drag affects X-axis
                    float rotationY = -dragDelta.x * _multiplier; // Horizontal drag affects Y-axis

                    _objectToRotate.Rotate(Vector3.right, rotationX, Space.World);
                    _objectToRotate.Rotate(Vector3.up, rotationY, Space.World);

                    _startTouchPosition = touch.position;
                }

                if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    _isTouchingMap = false;
                }
            }
        }

        if (Input.touchCount == 2) // Pinch zoom for two fingers
        {
            isZooming = true;
                // Get the touches
                Touch touch1 = Input.GetTouch(0);
                Touch touch2 = Input.GetTouch(1);

                // Calculate the current distance between the touches
                currentDistance = Vector2.Distance(touch1.position, touch2.position);

                // If the touches began, reset the previous distance
                if (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began)
                {
                    previousDistance = currentDistance;
                }

                // Calculate the difference in distances between frames
                float distanceDelta = currentDistance - previousDistance;

                
                Zoom(distanceDelta * zoomSpeed);

                // Update the previous distance for the next frame
                previousDistance = currentDistance;
        }
        else isZooming = false;
    }
    
    //zoom
    private float previousDistance;
    private float currentDistance;
    [SerializeField] private float zoomSpeed = 0.1f;
    private bool isZooming = false;

    public void Zoom(float increment)
    {
        _camera.fieldOfView = Mathf.Clamp(_camera.fieldOfView - increment, 1f, 20f);
    }
}
