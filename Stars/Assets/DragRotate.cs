using System;
using Unity.VisualScripting;
using UnityEngine;

public class DragRotate : MonoBehaviour
{
    [SerializeField]
    RectTransform _rawImageTransform; 

    [SerializeField]
    Transform _objectToRotate; 

    [SerializeField]
    Camera _camera; 

    [SerializeField]
    float _multiplier = 0.1f;

    private Vector2 _startTouchPosition; 
    private bool _isTouchingMap;

    [SerializeField] private Vector2EventChannelSO calibrateEvent;
    [SerializeField] private Vector2EventChannelSO duplicateCalibrateEvent;


    private float referenceFOV;
    private void Start()
    {
        referenceFOV = _camera.fieldOfView;
    }

    public void TriggerLocation()
    {
        Ray ray = new Ray();
        ray.origin = _camera.transform.position;
        ray.direction = _camera.transform.forward * 1000;

        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.red);
        
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 8))
        {
            Vector3 hitPoint = hit.point;
            
            Quaternion globeRotation = _objectToRotate.rotation; 
            Vector3 localHitPoint = Quaternion.Inverse(globeRotation) *
                                    (hitPoint - _objectToRotate.position);
            
            Vector3 normalizedLocalHitPoint = localHitPoint.normalized;
            
            float latitude =
                Mathf.Asin(normalizedLocalHitPoint.y) * Mathf.Rad2Deg; 
            float longitude = Mathf.Atan2(normalizedLocalHitPoint.z, normalizedLocalHitPoint.x) *
                              Mathf.Rad2Deg; 
            
            Debug.Log($"Latitude: {latitude}, Longitude: {longitude}");
            calibrateEvent.RaiseEvent(new Vector2(latitude, longitude));
            duplicateCalibrateEvent.RaiseEvent(new Vector2(latitude, longitude));
        }
    }

    private void Update()
    {
        if (Input.touchCount == 1 && !isZooming) 
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

                    float rotationX = dragDelta.y * _multiplier * _camera.fieldOfView/ referenceFOV; 
                    float rotationY = -dragDelta.x * _multiplier * _camera.fieldOfView/ referenceFOV; 

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

        if (Input.touchCount == 2) 
        {
            isZooming = true;
            Touch touch1 = Input.GetTouch(0);
                Touch touch2 = Input.GetTouch(1);
                
                currentDistance = Vector2.Distance(touch1.position, touch2.position);
                
                if (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began)
                {
                    previousDistance = currentDistance;
                }
                
                float distanceDelta = currentDistance - previousDistance;

                
                Zoom(distanceDelta * zoomSpeed);
                
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
