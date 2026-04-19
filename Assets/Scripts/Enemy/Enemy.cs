using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [HideInInspector] public EnemyData data;

    private float currentHealth;
    private Transform[] path;
    private int pathIndex = 0;
    private NavMeshAgent agent;

    [Header("VFX")]
    public GameObject deathVFXPrefab;
    public string poolTag = "BasicEnemy";

    // Health bar (optional UI above enemy)
    public EnemyHealthBar healthBar;

    public void Initialize(EnemyData enemyData, Transform[] waypoints)
    {
        data = enemyData;
        currentHealth = data.maxHealth;
        path = waypoints;
        pathIndex = 0;

        agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.speed = data.speed;
            agent.isStopped = false;
        }

        if (healthBar != null)
            healthBar.SetMaxHealth(currentHealth);

        MoveToNext();
    }

    void Update()
    {
        if (path == null || path.Length == 0) return;
        if (agent == null) return;

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            pathIndex++;
            if (pathIndex >= path.Length)
            {
                ReachBase();
                return;
            }
            MoveToNext();
        }
    }

    void MoveToNext()
    {
        if (agent != null && pathIndex < path.Length)
            agent.SetDestination(path[pathIndex].position);
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (healthBar != null)
            healthBar.UpdateHealth(currentHealth);

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        GameManager.Instance.AddCoins(data.coinReward);
        GameManager.Instance.RegisterEnemyDefeated();
        WaveManager.Instance.OnEnemyDefeated();

        if (deathVFXPrefab != null)
            Instantiate(deathVFXPrefab, transform.position, Quaternion.identity);

        ObjectPool.Instance.ReturnToPool(poolTag, gameObject);
    }

    void ReachBase()
    {
        GameManager.Instance.DamageBase(data.damage);
        WaveManager.Instance.OnEnemyDefeated();
        ObjectPool.Instance.ReturnToPool(poolTag, gameObject);
    }
}
