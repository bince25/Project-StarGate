using UnityEngine;
using UnityEngine.AI;

public class NormalEnemyAI : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform playerTransform;
    public float shootingDistance = 10f;
    public float minDistance = 5f;
    public float detectionRange = 15f;
    public float shootingCooldown = 2f;

    private EnemyController enemyController;
    private float lastShootTime;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        enemyController = GetComponent<EnemyController>();
        lastShootTime = -shootingCooldown; // Initialize so that enemy can shoot immediately
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
                agent.SetDestination(transform.position); // Stop and shoot
                enemyController.ShootAtPlayer();
                lastShootTime = Time.time;
            }
            else
            {
                agent.SetDestination(playerTransform.position); // Follow the player
            }
        }
    }

    private void MoveAwayFromPlayer()
    {
        Vector3 dirToPlayer = transform.position - playerTransform.position;
        Vector3 newPos = transform.position + dirToPlayer;
        agent.SetDestination(newPos);
    }

    private bool IsPlayerInDetectionRange()
    {
        return Vector3.Distance(transform.position, playerTransform.position) <= detectionRange;
    }
}
