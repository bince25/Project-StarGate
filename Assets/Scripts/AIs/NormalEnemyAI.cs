using UnityEngine;
using Pathfinding;

public class NormalEnemyAI : MonoBehaviour
{
    public Transform playerTransform;
    public float shootingDistance = 10f;
    public float minDistance = 5f;
    public float detectionRange = 15f;
    public float shootingCooldown = 2f;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private EnemyController enemyController;
    private AILerp aiLerp;
    private float lastShootTime;

    void Awake()
    {
        aiLerp = GetComponent<AILerp>();
        enemyController = GetComponent<EnemyController>();
        lastShootTime = -shootingCooldown; // Initialize so that enemy can shoot immediately

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
            float distance = Vector3.Distance(transform.position, playerTransform.position);
            if (distance < minDistance)
            {
                MoveAwayFromPlayer();
            }
            else if (distance <= shootingDistance && Time.time > lastShootTime + shootingCooldown)
            {
                // Stop and shoot
                aiLerp.canMove = false;
                enemyController.ShootAtPlayer();
                lastShootTime = Time.time;
            }
            else
            {
                // Follow the player
                aiLerp.destination = playerTransform.position;
                aiLerp.canMove = true;
            }

            UpdateAnimation();
        }
    }

    void UpdateAnimation()
    {
        bool isMoving = aiLerp.canMove && aiLerp.velocity.magnitude > 0.1f;
        animator.SetBool("IsMoving", isMoving);
        animator.SetFloat("Speed", aiLerp.velocity.magnitude);

        if (isMoving)
        {
            UpdateSpriteFlip(aiLerp.velocity.x);
        }
    }

    void UpdateSpriteFlip(float direction)
    {
        spriteRenderer.flipX = direction < 0;
    }

    private void MoveAwayFromPlayer()
    {
        Vector3 dirToPlayer = (transform.position - playerTransform.position).normalized;
        aiLerp.destination = transform.position + dirToPlayer * minDistance;
        aiLerp.canMove = true;
    }

    private bool IsPlayerInDetectionRange()
    {
        return Vector3.Distance(transform.position, playerTransform.position) <= detectionRange;
    }
}
