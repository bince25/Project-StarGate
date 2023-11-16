using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    public float rotationSpeed = 100f;

    void Update()
    {
        // Rotate the sword around the player
        transform.RotateAround(transform.parent.position, Vector3.forward, rotationSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Sword collided with " + other.name);
        // Check if the sword collided with another sword
        if (other.CompareTag("Sword"))
        {
            // Change the rotation direction
            rotationSpeed *= -1f;
        }
    }
}
