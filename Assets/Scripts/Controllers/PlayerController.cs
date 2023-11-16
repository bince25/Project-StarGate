using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;

    void Update()
    {
        // Get input axes
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Calculate movement vector
        Vector3 movement = new Vector3(horizontal, vertical, 0f).normalized;

        // Check if there is any input
        if (movement.magnitude >= 0.1f)
        {
            // Calculate movement vector and move the character without changing rotation
            Vector3 moveDir = new Vector3(horizontal, vertical, 0f).normalized;
            transform.Translate(moveDir * moveSpeed * Time.deltaTime, Space.World);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Gear"))
        {
            other.collider.GetComponent<GearController>().GetCollected();
        }
    }
}
