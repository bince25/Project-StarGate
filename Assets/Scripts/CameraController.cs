using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // Reference to the player's transform
    public float smoothSpeed = 0.125f;
    public Vector3 offset = new Vector3(0f, 0f, -10f); // Adjust the offset as needed

    void LateUpdate()
    {
        if (target != null)
        {
            // Calculate the desired position for the camera
            Vector3 desiredPosition = target.position + offset;

            // Smoothly move the camera towards the desired position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}
