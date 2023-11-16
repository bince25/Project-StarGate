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

    private AudioSource audioSource;
    public AudioClip deathSound;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.clip = deathSound;
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
    }

    void Update()
    {

    }

    public void TakeDamage(float damage)
    {
        audioSource.Play();
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

        if (deathSound != null && audioSource != null)
        {
            audioSource.Play();
            Destroy(gameObject, 0.2f);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
