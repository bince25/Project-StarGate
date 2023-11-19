using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorController : MonoBehaviour
{
    public float speed = 5f;
    public float damage = 20f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyAfterDelay(.3f));
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check for collisions with player or other game elements
        if (other.CompareTag("Player"))
        {
            // Apply damage to the player
            Destroy(other.gameObject);
            // Destroy the meteor upon collision with a player
            Destroy(gameObject);
        }
        else if (other.CompareTag("Obstacle"))
        {
            // Destroy the meteor upon collision with an obstacle
            Destroy(gameObject);
        }
    }
    IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        this.GetComponent<CircleCollider2D>().enabled = false;
    }
}
