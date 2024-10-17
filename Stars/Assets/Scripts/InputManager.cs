using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private VoidEventChannelSO doubleTapEventChannelSO;
    [SerializeField] private VoidEventChannelSO holdEventChannelSO;
    
    //double tap
    private float lastTapTime = 0f; 
    private float doubleTapTime = 0.3f; 
    
    //hold
    private float holdTimeThreshold = 0.5f; 
    private float touchStartTime = 0f; 
    private bool isHolding = false;


    void Update()
    {
        DetectHold();
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

    private void DetectHold()
    {
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
