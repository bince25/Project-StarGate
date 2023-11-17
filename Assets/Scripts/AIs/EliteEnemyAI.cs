using UnityEngine;
using UnityEngine.AI;

public class EliteEnemyAI : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform playerTransform;
    public SwordController playerSword;
    public float attackDistance = 2f; // Distance at which the enemy engages in combat
    public float safeDistanceFromSword = 1.5f; // Safe distance to maintain from the player's sword
    public float detectionRange = 10f;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {
        if (playerTransform != null && IsPlayerInDetectionRange())
        {
            EngageInCombat();
        }
    }

    private void EngageInCombat()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        Vector3 directionToPlayerSword = playerSword.transform.position - transform.position;
        float angleToPlayerSword = Vector3.Angle(transform.forward, directionToPlayerSword);
        bool isSwordThreatening = angleToPlayerSword < 90 && directionToPlayerSword.magnitude < safeDistanceFromSword;

        if (distanceToPlayer > attackDistance)
        {
            // Move towards the player
            agent.SetDestination(playerTransform.position);
        }
        else if (isSwordThreatening)
        {
            // Try to move to a position that avoids the sword
            AvoidPlayerSword();
        }
        else
        {
            // Engage in combat, attack player
            AttackPlayer();
        }
    }

    private void AvoidPlayerSword()
    {
        // Logic to find a safer position away from the player's sword
        // This could involve calculating a position to the side or behind the player
        Vector3 dirToPlayer = (transform.position - playerTransform.position).normalized;
        Vector3 sidestepDirection = Vector3.Cross(dirToPlayer, Vector3.up); // Sidestep
        Vector3 newPos = transform.position + sidestepDirection * agent.speed * Time.deltaTime;
        agent.SetDestination(newPos);
    }

    private void AttackPlayer()
    {
        // Attack logic here
        // This could involve triggering an attack animation and dealing damage if in range
    }

    private bool IsPlayerInDetectionRange()
    {
        return Vector3.Distance(transform.position, playerTransform.position) <= detectionRange;
    }
}
