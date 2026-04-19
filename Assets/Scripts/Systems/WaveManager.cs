using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance { get; private set; }

    [Header("Wave Config")]
    public WaveData[] waves;
    public Transform spawnPoint;
    public Transform[] waypoints;

    public int CurrentWave { get; private set; } = 0;
    private int enemiesAlive = 0;
    private bool spawning = false;

    public event System.Action<int> OnWaveStart;
    public event System.Action OnAllWavesComplete;

    void Awake() => Instance = this;

    void Start() => StartCoroutine(RunWaves());

    IEnumerator RunWaves()
    {
        yield return new WaitForSeconds(2f);

        for (int w = 0; w < waves.Length; w++)
        {
            if (GameManager.Instance.IsGameOver) yield break;

            CurrentWave = w + 1;
            OnWaveStart?.Invoke(CurrentWave);
            yield return StartCoroutine(SpawnWave(waves[w]));

            // Wait until all enemies defeated
            while (enemiesAlive > 0)
                yield return null;

            if (w < waves.Length - 1)
                yield return new WaitForSeconds(waves[w].timeBetweenWaves);
        }

        if (!GameManager.Instance.IsGameOver)
            GameManager.Instance.TriggerVictory();
    }

    IEnumerator SpawnWave(WaveData wave)
    {
        foreach (var entry in wave.entries)
        {
            for (int i = 0; i < entry.count; i++)
            {
                if (GameManager.Instance.IsGameOver) yield break;
                SpawnEnemy(entry.enemyData);
                yield return new WaitForSeconds(entry.spawnInterval);
            }
        }
    }

    void SpawnEnemy(EnemyData data)
    {
        var go = ObjectPool.Instance.Spawn(data.enemyData_poolTag(), spawnPoint.position, Quaternion.identity);
        if (go == null) return;

        var enemy = go.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.poolTag = data.enemyData_poolTag();
            enemy.Initialize(data, waypoints);
            enemiesAlive++;
        }
    }

    public void OnEnemyDefeated()
    {
        enemiesAlive = Mathf.Max(0, enemiesAlive - 1);
    }
}

public static class EnemyDataExtensions
{
    public static string enemyData_poolTag(this EnemyData d) => d.enemyName.Replace(" ", "");
}