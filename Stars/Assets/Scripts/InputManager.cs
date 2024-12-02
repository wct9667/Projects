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
    [SerializeField] private float zoomSpeed = 0.1f;
    private bool isZooming = false;


    void Update()
    {
        DetectHold();
        CheckZoom();
        DetectDoubleTap();
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
        
        if (Input.touchCount == 2)
        {
            isZooming = true;
            // Get the touches
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);
            
            currentDistance = Vector2.Distance(touch1.position, touch2.position);
            
            if (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began)
            {
                previousDistance = currentDistance;
            }
            
            float distanceDelta = currentDistance - previousDistance;
            
            zoomEventChannelSO.RaiseEvent(distanceDelta * zoomSpeed);
            
            previousDistance = currentDistance;
        }
        else isZooming = false;
    }

    private void DetectHold()
    {
        if (isZooming) return;
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0); 

            if (touch.phase == TouchPhase.Began)
            {
                touchStartTime = Time.time;
                isHolding = true;
            }
            else if (touch.phase == TouchPhase.Stationary && isHolding)
            {
                if (Time.time - touchStartTime >= holdTimeThreshold)
                {
                    isHolding = false; 
                   // holdEventChannelSO.RaiseEvent();
                }
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isHolding = false;
            }
        }
    }
}
