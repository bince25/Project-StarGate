using UnityEngine;
using UnityEngine.AI;

public class BasicEnemyAI : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform playerTransform;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        playerTransform = PlayerController.Instance.transform;

        // Get the Animator component
        animator = GetComponent<Animator>();

        // Get the SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        if (playerTransform != null)
        {
            agent.SetDestination(playerTransform.position);

            // Set IsMoving parameter based on agent's velocity
            bool isMoving = agent.velocity.magnitude > 0.1f;
            animator.SetBool("IsMoving", isMoving);
            animator.SetFloat("Speed", agent.velocity.magnitude);

            // Flip the sprite based on the direction of movement
            if (isMoving)
            {
                UpdateSpriteFlip(agent.velocity.x);
            }
        }
    }

    void UpdateSpriteFlip(float direction)
    {
        // Flip the sprite based on the direction of movement
        spriteRenderer.flipX = direction < 0;
    }
}
