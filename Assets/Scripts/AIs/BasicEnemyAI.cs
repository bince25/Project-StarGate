using Pathfinding;
using UnityEngine;

public class BasicEnemyAI : MonoBehaviour
{
    public Transform playerTransform;
    public float detectionRange = 10f;

    private IAstarAI ai;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        ai = GetComponent<IAstarAI>();

        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        playerTransform = PlayerController.Instance.transform;
    }

    void Update()
    {
        if (playerTransform != null && IsPlayerInDetectionRange())
        {
            ai.destination = playerTransform.position;
            ai.SearchPath();

            bool isMoving = ai.velocity.magnitude > 0.1f;
            animator.SetBool("IsMoving", isMoving);
            animator.SetFloat("Speed", ai.velocity.magnitude);

            if (isMoving)
            {
                UpdateSpriteFlip(ai.velocity.x);
            }
        }
    }

    void UpdateSpriteFlip(float direction)
    {
        spriteRenderer.flipX = direction < 0;
    }

    private bool IsPlayerInDetectionRange()
    {
        return Vector3.Distance(transform.position, playerTransform.position) <= detectionRange;
    }
}
