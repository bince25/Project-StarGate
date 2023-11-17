using UnityEngine;
using UnityEngine.AI;

public class BasicEnemyAI : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform playerTransform;
    public float detectionRange = 10f;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }
    void Start()
    {

    }

    void Update()
    {
        if (playerTransform != null && IsPlayerInDetectionRange())
        {
            agent.SetDestination(playerTransform.position);
        }
    }

    private bool IsPlayerInDetectionRange()
    {
        return Vector3.Distance(transform.position, playerTransform.position) <= detectionRange;
    }
}
