using UnityEngine;

[CreateAssetMenu(fileName = "WaveData", menuName = "TowerDefence/WaveData")]
public class WaveData : ScriptableObject 
{
    [System.Serializable]
    public class WaveEntry
    {
        public EnemyData enemyData;
        public int count = 5;
        public float spawnInterval = 1f;
    }
    public WaveEntry[] entries;
    public float timeBetweenWaves = 5f;
}
