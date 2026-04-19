using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Enemy target;
    private float damage;
    private string poolTag;
    public float speed = 10f;

    public void Initialize(Enemy t, float dmg, string tag)
    {
        target = t;
        damage = dmg;
        poolTag = tag;
    }

    void Update()
    {
        if (target == null || !target.gameObject.activeInHierarchy)
        {
            ObjectPool.Instance.ReturnToPool(poolTag, gameObject);
            return;
        }

        Vector3 dir = (target.transform.position - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;
        transform.LookAt(target.transform);

        if (Vector3.Distance(transform.position, target.transform.position) < 0.3f)
        {
            target.TakeDamage(damage);
            AudioManager.Instance?.PlaySFX("hit");
            ObjectPool.Instance.ReturnToPool(poolTag, gameObject);
        }
    }
}
