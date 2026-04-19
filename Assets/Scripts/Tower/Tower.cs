using System;
using JetBrains.Annotations;
using UnityEngine;

public class Tower : MonoBehaviour
{
    private TowerData data;
    private float fireCooldown = 0f;
    private Enemy currentTarget;

    [Header("References")]
    public Transform firePoint;
    public GameObject projectilePrefab;  
    public string projectilePoolTag = "Projectile";

    public void Initialize(TowerData towerData)
    {
        data = towerData;
    }

    void Update()
    {
        fireCooldown -= Time.deltaTime;
        FindTarget();

        if (currentTarget != null && fireCooldown <= 0f)
        {
            Shoot();
            fireCooldown = 1f / data.fireRate;
        }
    }

    void FindTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, data.range, LayerMask.GetMask("Enemy"));
        float minDist = float.MaxValue;
        currentTarget = null;

        foreach (var hit in hits)
        {
            var e = hit.GetComponent<Enemy>();
            if (e == null) continue;
            float d = Vector3.Distance(transform.position, e.transform.position);
            if (d < minDist) { minDist = d; currentTarget = e; }
        }
    }

    void Shoot()
    {
        if (currentTarget == null) return;

        Vector3 dir = (currentTarget.transform.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(dir);

        if (projectilePrefab != null)
        {
            var proj = ObjectPool.Instance.Spawn(projectilePoolTag, firePoint.position, firePoint.rotation);
            var p = proj?.GetComponent<Projectile>();
            p?.Initialize(currentTarget, data.damage, projectilePoolTag);
        }
        else
        {
            currentTarget.TakeDamage(data.damage);
            AudioManager.Instance?.PlaySFX("shoot");
        }
    }

    void OnDrawGizmosSelected()
    {
        if (data == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, data.range);
    }
}
