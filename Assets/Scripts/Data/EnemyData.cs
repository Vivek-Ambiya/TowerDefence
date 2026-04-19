using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "TowerDefence/EnemyData")]
public class EnemyData : ScriptableObject
{
    public string enemyName = "Basic Enemy";
    public float maxHealth = 100f;
    public float speed = 3f;
    public int damage = 1;          // damage to player base
    public int coinReward = 10;
    public GameObject prefab;
}