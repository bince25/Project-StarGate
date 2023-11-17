using UnityEngine;
using UnityEngine.AI;

public class NormalEnemyAI : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform playerTransform;
    public float shootingDistance = 10f;
    public float minDistance = 5f; // Minimum distance to keep from the player

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        playerTransform = PlayerController.Instance.transform;
    }

    void Start()
    {
    }

    void Update()
    {
        if (playerTransform != null)
        {
            float distance = Vector3.Distance(transform.position, playerTransform.position);
            if (distance < minDistance)
            {
                // Move away from the player
                Vector3 dirToPlayer = transform.position - playerTransform.position;
                Vector3 newPos = transform.position + dirToPlayer;

                agent.SetDestination(newPos);
            }
            else if (distance <= shootingDistance)
            {
                // Stop and shoot
                agent.SetDestination(transform.position);
                // Call shooting method here
            }
            else
            {
                // Follow the player
                agent.SetDestination(playerTransform.position);
            }
        }
    }
}
