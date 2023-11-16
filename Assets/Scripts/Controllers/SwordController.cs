using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    public float weight = 1f; // kg
    public float defaultRotationSpeed = 100f; // ms^-1
    private float currentRotationSpeed; // ms^-1
    private bool isReversed = false;
    public GameObject collisionParticles;

    // SOUND 
    public AudioClip collisionSound; // Add this variable

    private AudioSource audioSource; // Reference to the AudioSource component

    void Start()
    {
        currentRotationSpeed = defaultRotationSpeed;

         // Get the AudioSource component or add one if not present
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Set the collision sound clip
        audioSource.clip = collisionSound;
    }

    void Update()
    {
        float direction = isReversed ? -1 : 1;
        transform.RotateAround(transform.parent.position, Vector3.forward, direction * currentRotationSpeed * Time.deltaTime);
    }

    public void ReverseRotation()
    {
        isReversed = !isReversed;
    }

    public void ChangeSpeedTemporarily(float targetSpeed, float duration)
    {
        StartCoroutine(AdjustSpeed(targetSpeed, duration));
    }

    private IEnumerator AdjustSpeed(float targetSpeed, float duration)
    {
        float time = 0;
        float initialSpeed = currentRotationSpeed;

        // Increase speed to targetSpeed
        while (time < duration / 2)
        {
            currentRotationSpeed = Mathf.Lerp(initialSpeed, targetSpeed, time / (duration / 4));
            time += Time.deltaTime;
            yield return null;
        }

        // Decrease speed back to default
        time = 0; // reset time for the decreasing phase
        while (time < duration / 2)
        {
            currentRotationSpeed = Mathf.Lerp(targetSpeed, defaultRotationSpeed, time / (3 * duration / 4));
            time += Time.deltaTime;
            yield return null;
        }

        currentRotationSpeed = defaultRotationSpeed; // ensure it's set back to default
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Sword collided with " + other.collider.name);
        // Check if the sword collided with another sword
        if (other.collider.CompareTag("Sword"))
        {
            SwordController otherSword = other.collider.GetComponent<SwordController>();

            // Get the contact points from the collision
            ContactPoint2D[] contacts = new ContactPoint2D[1];
            other.GetContacts(contacts);
            // Change the rotation direction
             // Play the particle effect
            if (collisionParticles != null && this.transform.parent.gameObject.tag == "Player")
            {
                if (contacts.Length > 0)
            {   
                Vector2 contactPoint = contacts[0].point;
                Debug.Log("Contact point: " + contactPoint);
                GameObject particles =  Instantiate(collisionParticles, contactPoint, Quaternion.identity);

                 // Get the particle system component from the instantiated prefab
                ParticleSystem particlesSystem = particles.GetComponent<ParticleSystem>();

                // Destroy the instantiated object after the duration of the particle system
                Destroy(particles, particlesSystem.main.duration+ 0.3f);
            }
            }
            // Change the rotation speed according to the sword's momentum comparison
            float otherSwordMomentum = otherSword.weight * otherSword.currentRotationSpeed;
            float thisSwordMomentum = weight * currentRotationSpeed;
            float momentumDifference = otherSwordMomentum - thisSwordMomentum;
            float momentumDifferencePercentage = (otherSwordMomentum - thisSwordMomentum) / Mathf.Max(otherSwordMomentum, thisSwordMomentum);
            Camera.main.GetComponent<CameraController>().TriggerShake();
            if (collisionSound != null && audioSource != null)
            {
                audioSource.Play();
            }

            if (momentumDifferencePercentage > Constants.SWORD_DEFAULT_MOMENTUM_DIFFERENCE_THRESHOLD_PRECENTAGE)
            {
                // The other sword has more momentum, so this sword's rotation speed should be increased
                ChangeSpeedTemporarily(defaultRotationSpeed + Math.Abs(momentumDifference) / weight, 0.7f);
                ReverseRotation();
            }
        }
        else if (other.collider.CompareTag("Enemy"))
        {
            EnemyController enemy = other.collider.GetComponent<EnemyController>();
            enemy.TakeDamage(10);
        }
    }
}
