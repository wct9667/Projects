using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GyroController : MonoBehaviour
{
    public float slerpValue = 0.2f;
    public int verticalOffsetAngle = -90;
    public int horizontalOffsetAngle = 0;
    
    private Quaternion phoneOrientation;
    private Quaternion correctedPhoneOrientation;
    private Quaternion horizontalRotationCorrection;
    private Quaternion verticalRotationCorrection;
    private Quaternion inGameOrientation;

    private void Start()
    {
        // Check if device has a gyroscope and enable it
        if (SystemInfo.supportsGyroscope)
        {
            Input.gyro.enabled = true;
        }
    }

    private void Update()
    {
        // Retrieve gyroscopic information from the phone
        phoneOrientation = Input.gyro.attitude;

        // Correct the phone's orientation to match Unity's coordinate system
        correctedPhoneOrientation = new Quaternion(phoneOrientation.x, phoneOrientation.y, -phoneOrientation.z, -phoneOrientation.w);

        // Apply vertical and horizontal rotation corrections
        verticalRotationCorrection = Quaternion.AngleAxis(verticalOffsetAngle, Vector3.left);
        horizontalRotationCorrection = Quaternion.AngleAxis(horizontalOffsetAngle, Vector3.up);

        // Combine all rotations to get the in-game orientation
        inGameOrientation = horizontalRotationCorrection * verticalRotationCorrection * correctedPhoneOrientation;

        // Smoothly rotate the camera to match the phone's orientation
        transform.rotation = Quaternion.Slerp(transform.rotation, inGameOrientation, slerpValue);
    }
}

