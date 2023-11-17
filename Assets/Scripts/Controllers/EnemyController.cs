using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private bool dead = false;
    public float health = 1f;
    public float damage = 1f;
    public float speed = 1f;
    public Enemy enemyType = Enemy.Basic;

    public Transform bulletSpawnPoint; // Set this in the Unity Inspector
    public float bulletSpeed = 10f;
    public float shootingInterval = 2f;

    void Start()
    {
        switch (enemyType)
        {
            case Enemy.Basic:
                health = EnemyConstants.ENEMY_BASIC_HEALTH;
                damage = EnemyConstants.ENEMY_BASIC_DAMAGE;
                speed = EnemyConstants.ENEMY_BASIC_SPEED;
                break;
            case Enemy.Normal:
                health = EnemyConstants.ENEMY_NORMAL_HEALTH;
                damage = EnemyConstants.ENEMY_NORMAL_DAMAGE;
                speed = EnemyConstants.ENEMY_NORMAL_SPEED;
                break;
            case Enemy.Elite:
                health = EnemyConstants.ENEMY_ELITE_HEALTH;
                damage = EnemyConstants.ENEMY_ELITE_DAMAGE;
                speed = EnemyConstants.ENEMY_ELITE_SPEED;
                break;
            case Enemy.Boss:
                health = EnemyConstants.ENEMY_BOSS_HEALTH;
                damage = EnemyConstants.ENEMY_BOSS_DAMAGE;
                speed = EnemyConstants.ENEMY_BOSS_SPEED;
                break;
        }

        if (enemyType == Enemy.Normal)
        {
            InvokeRepeating("ShootAtPlayer", shootingInterval, shootingInterval);
        }
    }

    private void ShootAtPlayer()
    {
        GameObject bullet = PoolManager.Instance.GetObject("Bullet");
        if (bullet != null)
        {
            bullet.transform.position = bulletSpawnPoint.position;
            bullet.SetActive(true);

            Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
            if (bulletRigidbody != null)
            {
                Vector2 direction = (PlayerController.Instance.transform.position - transform.position).normalized; // Direction towards the player
                bulletRigidbody.velocity = direction * bulletSpeed;

                // Calculate the angle in degrees, add 90 degrees to correct the orientation, and rotate the bullet
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;
                bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            }
            // Optionally return the bullet to the pool after a set time
            //PoolManager.Instance.ReturnObject("Bullet", bullet, 5f); // Return after 5 seconds
        }
    }

    public void TakeDamage(float damage)
    {
        SoundManager.Instance.PlayEnemyHitSound();
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void DropGears()
    {
        switch (enemyType)
        {
            case Enemy.Basic:
                SpawnGears(EnemyConstants.ENEMY_BASIC_GEAR_DROP);
                break;
            case Enemy.Normal:
                SpawnGears(EnemyConstants.ENEMY_NORMAL_GEAR_DROP);
                break;
            case Enemy.Elite:
                SpawnGears(EnemyConstants.ENEMY_ELITE_GEAR_DROP);
                break;
            case Enemy.Boss:
                SpawnGears(EnemyConstants.ENEMY_BOSS_GEAR_DROP);
                break;
        }
    }

    private void SpawnGears(int count)
    {
        StartCoroutine(SpawnGearsCoroutine(count));
    }

    private IEnumerator SpawnGearsCoroutine(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject gear = PoolManager.Instance.GetObject("Gear"); // Assuming "Gear" is the tag for gears in PoolManager
            if (gear != null)
            {
                gear.transform.position = transform.position + Random.insideUnitSphere * 2f; // Random position around the enemy
                gear.SetActive(true);

                Rigidbody gearRigidbody = gear.GetComponent<Rigidbody>();
                if (gearRigidbody != null)
                {
                    gearRigidbody.AddForce(Random.insideUnitSphere * 10f, ForceMode.Impulse); // Adding random force for spill effect
                }
            }
            yield return new WaitForSeconds(0.2f); // Wait for 0.1 seconds before spawning the next gear
        }
    }


    public void Die()
    {
        if (dead)
        {
            return;
        }
        dead = true;
        DropGears();

        SoundManager.Instance.PlayEnemyDeathSound();
        Destroy(gameObject, 0.2f);
    }
}
