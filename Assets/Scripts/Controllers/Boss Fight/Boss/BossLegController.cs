using UnityEngine;

public class BossLegController : MonoBehaviour
{
    public float legHealth = 25f; // Health of each individual leg
    private BossEnemyController boss; // Reference to the boss enemy controller

    void Start()
    {
        // Find the boss enemy controller in the hierarchy
        boss = transform.parent.GetComponent<BossEnemyController>();
    }

    public void DamageLeg(float damage)
    {
        // Reduce the leg's health
        legHealth -= damage;
        boss.GetComponent<BossEnemyController>().DamageBoss(damage);

        // Check if the leg is destroyed
        if (legHealth <= 0)
        {
            DestroyLeg();
            Destroy(this.gameObject);
        }
    }

    void DestroyLeg()
    {
        // Notify the boss controller that this leg is destroyed
        boss.DestroyLeg();

        // Destroy the leg GameObject
        Destroy(gameObject);
    }
}
