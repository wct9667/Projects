using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    //double tap
    [SerializeField] private VoidEventChannelSO doubleTapEventChannelSO;
    private float lastTapTime = 0f; 
    private float doubleTapTime = 0.3f; 
    
    //hold
    [SerializeField] private VoidEventChannelSO holdEventChannelSO;
    private float holdTimeThreshold = 0.5f; 
    private float touchStartTime = 0f; 
    private bool isHolding = false;
    
    //zoom
    [SerializeField] private FloatEventChannelSO zoomEventChannelSO;
    private float previousDistance;
    private float currentDistance;
    private float zoomSpeed = 0.1f;
    private bool isZooming = false;


    void Update()
    {
        DetectHold();
        CheckZoom();
    }

    private void DetectDoubleTap()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            if (Time.time - lastTapTime < doubleTapTime)
            {
                doubleTapEventChannelSO.RaiseEvent(null);
            }
            lastTapTime = Time.time;
        }
    }

    private void CheckZoom()
    {
        
        // Check if there are two touches on the screen
        if (Input.touchCount == 2)
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

            // Zoom in or out
            zoomEventChannelSO.RaiseEvent(distanceDelta * zoomSpeed);

            // Update the previous distance for the next frame
            previousDistance = currentDistance;
        }
    }

    private void DetectHold()
    {
        if (isZooming) return;
        // Check if there is at least one touch on the screen
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0); // Get the first touch

            if (touch.phase == TouchPhase.Began)
            {
                // Record the time when the touch started
                touchStartTime = Time.time;
                isHolding = true;
            }
            else if (touch.phase == TouchPhase.Stationary && isHolding)
            {
                // Check if the touch has been held long enough
                if (Time.time - touchStartTime >= holdTimeThreshold)
                {
                    isHolding = false; 
                    holdEventChannelSO.RaiseEvent();
                }
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                // Reset hold status if the touch ends or is canceled
                isHolding = false;
            }
        }
    }
}
