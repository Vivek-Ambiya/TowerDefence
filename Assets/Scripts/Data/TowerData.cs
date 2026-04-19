using UnityEngine;

[CreateAssetMenu(fileName ="TowerData", menuName ="TowerDefence/TowerData")]
public class TowerData : ScriptableObject
{
    public string towerName = "Tower";
    public float damage = 20f;
    public float range = 5f;
    public float fireRate = 1f;
    public int coinCost = 50;
    public int gemCost = 0;
    public bool lockedByDefault = false;
    public GameObject prefab;
    public Sprite icon;
}
