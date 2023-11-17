using UnityEngine;
using Pathfinding;

public class EliteEnemyAI : MonoBehaviour
{
    public Transform playerTransform;
    public SwordController playerSword;
    public float attackDistance = 2f;
    public float safeDistanceFromSword = 1.5f;
    public float detectionRange = 10f;

    private IAstarAI ai;

    void Awake()
    {
        ai = GetComponent<IAstarAI>();
    }

    void Start()
    {
        playerTransform = PlayerController.Instance.transform;
        playerSword = playerTransform.GetComponentInChildren<SwordController>();
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
            ai.destination = playerTransform.position;
            ai.SearchPath();
        }
        else if (isSwordThreatening)
        {
            AvoidPlayerSword();
        }
        else
        {
            AttackPlayer();
        }
    }

    private void AvoidPlayerSword()
    {
        Vector3 dirToPlayer = (transform.position - playerTransform.position).normalized;
        Vector3 sidestepDirection = Vector3.Cross(dirToPlayer, Vector3.up);
        Vector3 newPos = transform.position + sidestepDirection * (ai.maxSpeed * Time.deltaTime);
        ai.destination = newPos;
        ai.SearchPath();
    }

    private void AttackPlayer()
    {
        // Implement attack logic here
        // This could involve triggering an attack animation and dealing damage if in range
    }

    private bool IsPlayerInDetectionRange()
    {
        return Vector3.Distance(transform.position, playerTransform.position) <= detectionRange;
    }
}
