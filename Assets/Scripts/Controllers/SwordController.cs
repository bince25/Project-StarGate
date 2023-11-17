using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    public float weight = 1f; // kg
    public float defaultRotationSpeed = 100f; // ms^-1
    private float currentRotationSpeed; // ms^-1
    private bool isReversed = false;
    public GameObject collisionParticles;

    public float durability = SwordConstants.SWORD_DEFAULT_DURABILITY;
    public float damage = SwordConstants.SWORD_DEFAULT_DAMAGE;

    private ContactPoint2D[] contacts = new ContactPoint2D[1];

    void Start()
    {
        currentRotationSpeed = defaultRotationSpeed;
    }

    void Update()
    {
        float direction = isReversed ? -1 : 1;
        transform.RotateAround(transform.parent.position, Vector3.forward, direction * currentRotationSpeed * Time.deltaTime);
    }

    public void ReverseRotation()
    {
        isReversed = !isReversed;
    }

    public void ChangeSpeedTemporarily(float targetSpeed, float duration)
    {
        StartCoroutine(AdjustSpeed(targetSpeed, duration));
    }

    private IEnumerator AdjustSpeed(float targetSpeed, float duration)
    {
        float time = 0;
        float initialSpeed = currentRotationSpeed;

        // Increase speed to targetSpeed
        while (time < duration / 2)
        {
            currentRotationSpeed = Mathf.Lerp(initialSpeed, targetSpeed, time / (duration / 4));
            time += Time.deltaTime;
            yield return null;
        }

        // Decrease speed back to default
        time = 0; // reset time for the decreasing phase
        while (time < duration / 2)
        {
            currentRotationSpeed = Mathf.Lerp(targetSpeed, defaultRotationSpeed, time / (3 * duration / 4));
            time += Time.deltaTime;
            yield return null;
        }

        currentRotationSpeed = defaultRotationSpeed; // ensure it's set back to default
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Sword collided with " + other.collider.name);
        // Check if the sword collided with another sword
        if (other.collider.CompareTag("Sword"))
        {
            HandleSwordCollision(other);
        }
        else if (other.collider.CompareTag("Enemy"))
        {
            HandleEnemyCollision(other);
        }
        else if (other.collider.CompareTag("Wall"))
        {
            HandleWallCollision(other);
        }
        else if (other.collider.CompareTag("Obstacle"))
        {
            HandleObstacleCollision(other);
        }
        else if (other.collider.CompareTag("Player"))
        {
            // Do nothing
        }
        else if (other.collider.CompareTag("Bullet"))
        {
            HandleBulletCollision(other);
        }
        else
        {
            Debug.LogWarning("Sword collided with an unknown object: " + other.collider.name);
        }
    }

    private void HandleSwordCollision(Collision2D collision)
    {
        SwordController otherSword = collision.collider.GetComponent<SwordController>();

        HandleSoundEffect();
        CreateSparkParticles(collision);
        HandleMomentumCollision(otherSword);

        HandleDurabilityChange(CollisionType.Sword);

        Camera.main.GetComponent<CameraController>().TriggerShake();
    }

    private void HandleMomentumCollision(SwordController otherSword)
    {
        // Change the rotation speed according to the sword's momentum comparison
        float otherSwordMomentum = otherSword.weight * otherSword.currentRotationSpeed;
        float thisSwordMomentum = weight * currentRotationSpeed;
        float momentumDifference = otherSwordMomentum - thisSwordMomentum;
        float momentumDifferencePercentage = (otherSwordMomentum - thisSwordMomentum) / Mathf.Max(otherSwordMomentum, thisSwordMomentum);

        if (momentumDifferencePercentage > SwordConstants.SWORD_DEFAULT_MOMENTUM_DIFFERENCE_THRESHOLD_PRECENTAGE)
        {
            // The other sword has more momentum, so this sword's rotation speed should be increased
            ChangeSpeedTemporarily(defaultRotationSpeed + Math.Abs(momentumDifference) / weight, 0.7f);
            ReverseRotation();
        }
    }

    private void CreateSparkParticles(Collision2D collision)
    {
        // Get the contact points from the collision
        collision.GetContacts(contacts);
        // Change the rotation direction
        // Play the particle effect
        if (collisionParticles != null && this.transform.parent.gameObject.tag == "Player")
        {
            if (contacts.Length > 0)
            {
                Vector2 contactPoint = contacts[0].point;
                Debug.Log("Contact point: " + contactPoint);
                GameObject particles = Instantiate(collisionParticles, contactPoint, Quaternion.identity);

                // Get the particle system component from the instantiated prefab
                ParticleSystem particlesSystem = particles.GetComponent<ParticleSystem>();

                // Destroy the instantiated object after the duration of the particle system
                Destroy(particles, particlesSystem.main.duration + 0.3f);
            }
        }
    }

    private void HandleSoundEffect()
    {
        if (!gameObject.transform.parent.CompareTag("Player"))
        {
            // Do nothing if the sword is colliding with another sword from the same player
            return;
        }
        SoundManager.Instance.PlaySwordCollideSound();
    }

    private void HandleEnemyCollision(Collision2D collision)
    {
        EnemyController enemy = collision.collider.GetComponent<EnemyController>();
        enemy.TakeDamage(damage);

        switch (enemy.enemyType)
        {
            case Enemy.Basic:
                HandleDurabilityChange(CollisionType.EnemyBasic);
                break;
            case Enemy.Normal:
                HandleDurabilityChange(CollisionType.EnemyNormal);
                break;
            case Enemy.Elite:
                HandleDurabilityChange(CollisionType.EnemyElite);
                break;
            case Enemy.Boss:
                HandleDurabilityChange(CollisionType.EnemyBoss);
                break;
        }
    }

    private void HandleWallCollision(Collision2D collision)
    {
        ReverseRotation();
        HandleDurabilityChange(CollisionType.Wall);
        CreateSparkParticles(collision);
        HandleSoundEffect();
    }

    private void HandleObstacleCollision(Collision2D collision)
    {
        ReverseRotation();
        HandleDurabilityChange(CollisionType.Obstacle);
    }

    private void HandleBulletCollision(Collision2D collision)
    {
        CreateSparkParticles(collision);
        //bullet.Reflect();
        HandleSoundEffect();
    }

    private void HandleDurabilityChange(CollisionType collisionType)
    {
        switch (collisionType)
        {
            case CollisionType.EnemyBasic:
                durability -= SwordConstants.BASIC_ENEMY_DEFAULT_DURABILITY_LOSS;
                break;
            case CollisionType.EnemyNormal:
                durability -= SwordConstants.NORMAL_ENEMY_DEFAULT_DURABILITY_LOSS;
                break;
            case CollisionType.EnemyElite:
                durability -= SwordConstants.ELITE_ENEMY_DEFAULT_DURABILITY_LOSS;
                break;
            case CollisionType.EnemyBoss:
                durability -= SwordConstants.BOSS_ENEMY_DEFAULT_DURABILITY_LOSS;
                break;
            case CollisionType.Sword:
                durability -= SwordConstants.OTHER_SWORD_DEFAULT_DURABILITY_LOSS;
                break;
            case CollisionType.Wall:
                durability -= SwordConstants.OTHER_SWORD_DEFAULT_DURABILITY_LOSS;
                break;
            case CollisionType.Obstacle:
                durability -= SwordConstants.OTHER_SWORD_DEFAULT_DURABILITY_LOSS;
                break;
            case CollisionType.Bullet:
                durability -= SwordConstants.BULLET_DEFAULT_DURABILITY_LOSS;
                break;
        }
    }
}
