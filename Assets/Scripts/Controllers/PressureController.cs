using System.Collections;
using UnityEngine;

public class PressureController : MonoBehaviour
{

    public TrapController[] traps;
    public Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Triggered");
        if (other.CompareTag("Player"))
        {
            SoundManager.Instance.PlaySwordObstacleSound();
            foreach (TrapController trap in traps)
            {
                trap.isActive = false;
                trap.disabled = true;
                trap.animator.SetBool("isAlwaysActive", false);
                trap.animator.SetBool("isActivated", false);
                animator.SetBool("isTriggered", true);
                StartCoroutine(trap.DeactivateTrap());
                StartCoroutine(StopAnimation());
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (TrapController trap in traps)
            {
                StartCoroutine(trap.DeactivateTrap());
            }
        }
    }

    IEnumerator StopAnimation()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length + 0.05f);
        animator.SetBool("isTriggered", false);
    }
}