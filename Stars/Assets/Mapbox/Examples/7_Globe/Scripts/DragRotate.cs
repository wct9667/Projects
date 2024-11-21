using UnityEngine;

namespace Mapbox.Examples
{
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

        [SerializeField]
        float _zoomSpeed = 0.1f; // Speed of zooming in/out
        [SerializeField]
        float _minZoom = 5f; // Minimum camera zoom distance
        [SerializeField]
        float _maxZoom = 20f; // Maximum camera zoom distance

        private Vector2 _startTouchPosition; // Store start touch position as Vector2
        private bool _isTouchingMap = false;

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

                    // Step 4: Apply rotation to the object (world-space or local-space as needed)
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

            // Step 5: Handle pinch-to-zoom
            if (Input.touchCount == 2)
            {
                // Get the positions of the first two touches
                Touch touch1 = Input.GetTouch(0);
                Touch touch2 = Input.GetTouch(1);

                // Calculate the distance between the two touches in the previous and current frames
                float previousDistance = (touch1.position - touch2.position).magnitude;
                float currentDistance = (touch1.position - touch2.position).magnitude;

                // Calculate the zoom difference based on the change in distance
                float zoomDelta = currentDistance - previousDistance;

                // Zoom the camera by modifying its field of view or position
                ZoomCamera(zoomDelta);
            }
        }

        // Step 6: Function to zoom the camera based on pinch gesture
        private void ZoomCamera(float zoomDelta)
        {
            // Adjust the camera's field of view (FOV) for zoom effect
            // Use negative zoomDelta for zooming out and positive for zooming in
            _camera.fieldOfView = Mathf.Clamp(_camera.fieldOfView - zoomDelta * _zoomSpeed, _minZoom, _maxZoom);

            // Alternatively, zoom the camera by changing its position
            // Vector3 direction = (_camera.transform.position - _objectToRotate.position).normalized;
            // _camera.transform.position += direction * zoomDelta * _zoomSpeed;
        }
    }
}
