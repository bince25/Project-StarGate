using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float health = 1f;
    public float damage = 1f;
    public float speed = 1f;
    public Enemy enemyType = Enemy.Basic;

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
    }

    void Update()
    {

    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
