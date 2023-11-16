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

    void Start()
    {
        currentRotationSpeed = defaultRotationSpeed;
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
            currentRotationSpeed = Mathf.Lerp(initialSpeed, targetSpeed, time / (duration / 2));
            time += Time.deltaTime;
            yield return null;
        }

        // Decrease speed back to default
        time = 0; // reset time for the decreasing phase
        while (time < duration / 2)
        {
            currentRotationSpeed = Mathf.Lerp(targetSpeed, defaultRotationSpeed, time / (duration / 2));
            time += Time.deltaTime;
            yield return null;
        }

        currentRotationSpeed = defaultRotationSpeed; // ensure it's set back to default
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Sword collided with " + other.name);
        // Check if the sword collided with another sword
        if (other.CompareTag("Sword"))
        {
            SwordController otherSword = other.GetComponent<SwordController>();
            // Change the rotation direction

            // Change the rotation speed according to the sword's momentum comparison
            float otherSwordMomentum = otherSword.weight * otherSword.currentRotationSpeed;
            float thisSwordMomentum = weight * currentRotationSpeed;
            float momentumDifference = otherSwordMomentum - thisSwordMomentum;
            float momentumDifferencePercentage = (otherSwordMomentum - thisSwordMomentum) / Mathf.Max(otherSwordMomentum, thisSwordMomentum);

            if (momentumDifferencePercentage > Constants.SWORD_DEFAULT_MOMENTUM_DIFFERENCE_THRESHOLD_PRECENTAGE)
            {
                // The other sword has more momentum, so this sword's rotation speed should be increased
                ChangeSpeedTemporarily(defaultRotationSpeed + Math.Abs(momentumDifference) / weight, 0.7f);
                ReverseRotation();
            }
        }
    }
}
