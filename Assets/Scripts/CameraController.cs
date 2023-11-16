using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // Reference to the player's transform
    public float smoothSpeed = 0.125f;
    public Vector3 offset = new Vector3(0f, 0f, -10f); // Adjust the offset as needed

    private bool isShaking = false;
    private float shakeDuration = 0.2f;
    private float shakeMagnitude = 0.1f;

    void LateUpdate()
    {
        if (target != null)
        {
            // Calculate the desired position for the camera
            Vector3 desiredPosition = target.position + offset;

            // Apply camera shake if triggered
            if (isShaking)
            {
                // Generate a random offset within a sphere
                Vector3 randomOffset = Random.insideUnitSphere * shakeMagnitude;

                // Apply the random offset to the desired position
                desiredPosition += randomOffset;

                shakeDuration -= Time.deltaTime;

                if (shakeDuration <= 0f)
                {
                    isShaking = false;
                    shakeDuration = 0f;
                }
            }

            // Smoothly move the camera towards the desired position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }

    // Trigger camera shake externally
    public void TriggerShake()
    {
        isShaking = true;
        shakeDuration = 0.3f; // Adjust the duration as needed
        shakeMagnitude = 0.3f; // Adjust the magnitude as needed
    }
}
