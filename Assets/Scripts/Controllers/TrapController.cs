using System.Collections;
using UnityEngine;

public class TrapController : MonoBehaviour
{
    public Trap trapType;
    public float activationInterval = 2f; // Time between activations
    public Animator animator; // Animator for the trap

    private float timer;

    public bool isActive = false;
    public bool disabled = false;

    public bool isVs = false;

    void Start()
    {
        switch (trapType)
        {
            case Trap.Spikes:
                if (animator == null)
                {
                    animator = GetComponent<Animator>();
                }
                break;
            case Trap.Fall:
                break;
        }
        timer = activationInterval;
    }

    void Update()
    {
        // Timer to activate the trap periodically
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = activationInterval;

            if (animator)
            {
                ActivateTrap();
            }
        }
    }

    void ActivateTrap()
    {
        // Activate the trap and play animation
        if (!disabled) animator.SetBool("isActivated", true);
        if (isActive) animator.SetBool("isAlwaysActive", true);

        // Optionally, add a delay before deactivating for the animation to play
        if (!isActive) StartCoroutine(DeactivateTrap());
    }

    public IEnumerator DeactivateTrap()
    {
        yield return new WaitForSeconds(1f); // Wait for the animation to play
        animator.SetBool("isActivated", false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        switch (trapType)
        {
            case Trap.Spikes:
                if (other.CompareTag("Player") && animator.GetBool("isActivated"))
                {
                    if (isVs)
                    {
                        other.gameObject.GetComponent<Player>().Die();
                    }
                    else
                    {
                        PlayerController.Instance.Die();
                    }
                }
                break;
            case Trap.Fall:
                if (other.CompareTag("Player"))
                {
                    if (isVs)
                    {
                        other.gameObject.GetComponent<Player>().Die();
                    }
                    else
                    {
                        PlayerController.Instance.Die();
                    }
                }
                break;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        switch (trapType)
        {
            case Trap.Spikes:
                if (other.CompareTag("Player") && animator.GetBool("isActivated"))
                {
                    if (isVs)
                    {
                        other.gameObject.GetComponent<Player>().Die();
                    }
                    else
                    {
                        PlayerController.Instance.Die();
                    }
                }
                break;
            case Trap.Fall:
                if (other.CompareTag("Player"))
                {
                    if (isVs)
                    {
                        other.gameObject.GetComponent<Player>().Die();
                    }
                    else
                    {
                        PlayerController.Instance.Die();
                    }
                }
                break;
        }
    }
}
public enum Trap
{
    Spikes,
    Fall
}