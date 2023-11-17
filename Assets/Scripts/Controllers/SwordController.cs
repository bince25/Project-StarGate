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

    [SerializeField]
    private Vector3 edgeOfSword = new Vector3();

    private Vector3 contact = new Vector3();

    private bool isPlayerSword = false;

    void Start()
    {
        currentRotationSpeed = defaultRotationSpeed;
        edgeOfSword = GetComponentInChildren<Transform>().position;
        isPlayerSword = transform.parent.gameObject.CompareTag("Player");
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

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the sword collided with another sword
        if (other.CompareTag("Sword"))
        {
            HandleSwordCollision(other);
        }
        else if (other.CompareTag("Enemy") && isPlayerSword)
        {
            HandleEnemyCollision(other);
        }
        else if (other.CompareTag("Wall"))
        {
            HandleWallCollision(other);
        }
        else if (other.CompareTag("Obstacle"))
        {
            HandleObstacleCollision(other);
        }
        else if (other.CompareTag("Player"))
        {
            // Do nothing
        }
        else if (other.CompareTag("Bullet"))
        {
            HandleBulletCollision(other);
        }
        else
        {
            Debug.LogWarning("Sword collided with an unknown object: " + other.name);
        }
    }

    private void HandleSwordCollision(Collider2D collider)
    {
        SwordController otherSword = collider.GetComponent<SwordController>();

        HandleSoundEffect(CollisionType.Sword);
        CreateSparkParticles(collider);
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
        else
        {
            ReverseRotation();
        }
    }

    private void CreateSparkParticles(Collider2D collider)
    {
        // Get the contact points from the collision
        contact = collider.bounds.ClosestPoint(collider.transform.position);
        // Change the rotation direction
        // Play the particle effect
        if (collisionParticles != null && isPlayerSword)
        {
            if (contact != null)
            {
                Debug.Log("Contact point: " + contact);
                GameObject particles = Instantiate(collisionParticles, contact, Quaternion.identity);

                // Get the particle system component from the instantiated prefab
                ParticleSystem particlesSystem = particles.GetComponent<ParticleSystem>();

                // Destroy the instantiated object after the duration of the particle system
                Destroy(particles, particlesSystem.main.duration + 0.3f);
            }
        }
    }

    private void HandleWallSpark()
    {
        edgeOfSword = GetComponentInChildren<Transform>().position;
        // Create spark at the end of the sword
        if (collisionParticles != null && isPlayerSword)
        {
            if (edgeOfSword != null)
            {
                GameObject particles = Instantiate(collisionParticles, edgeOfSword, Quaternion.identity);

                // Get the particle system component from the instantiated prefab
                ParticleSystem particlesSystem = particles.GetComponent<ParticleSystem>();

                // Destroy the instantiated object after the duration of the particle system
                Destroy(particles, particlesSystem.main.duration + 0.3f);
            }
        }
    }

    private void HandleSoundEffect(CollisionType collisionType)
    {
        if (!gameObject.transform.parent.CompareTag("Player"))
        {
            // Do nothing if the sword is colliding with another sword from the same player
            return;
        }
        switch (collisionType)
        {
            case CollisionType.Sword:
                SoundManager.Instance.PlaySwordCollideSound();
                break;
            case CollisionType.Wall:
                SoundManager.Instance.PlaySwordObstacleSound();
                break;
            case CollisionType.Bullet:
                SoundManager.Instance.PlaySwordBulletSound();
                break;
            default:
                //SoundManager.Instance.PlaySwordCollideSound();
                break;
        }

        SoundManager.Instance.PlaySwordCollideSound();
    }

    private void HandleEnemyCollision(Collider2D collider)
    {
        EnemyController enemy = collider.GetComponent<EnemyController>();
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

    private void HandleWallCollision(Collider2D collider)
    {
        ReverseRotation();
        HandleDurabilityChange(CollisionType.Wall);
        HandleWallSpark();
        HandleSoundEffect(CollisionType.Wall);
    }

    private void HandleObstacleCollision(Collider2D collider)
    {
        ReverseRotation();
        HandleDurabilityChange(CollisionType.Obstacle);
    }

    private void HandleBulletCollision(Collider2D collider)
    {
        CreateSparkParticles(collider);
        HandleDurabilityChange(CollisionType.Bullet);
        //bullet.Reflect();
        HandleSoundEffect(CollisionType.Bullet);
        collider.gameObject.GetComponent<BulletController>().ReturnToPool();
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
