using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Settings")]
    public int startingHP = 20;
    public int startingCoins = 150;
    public int startingGems = 5;

    // state
    public int PlayerHP { get; private set; }
    public int Coins { get; private set; }
    public int Gems { get; private set; }
    public int EnemiesDefeated { get; private set; }
    public int CoinsEarned { get; private set; }
    public bool IsGameOver { get; private set; }

    // Events
    public event System.Action<int> OnHPChanged;
    public event System.Action<int> OnCoinsChanged;
    public event System.Action<int> OnGemsChanged;
    public event System.Action OnGameOver;
    public event System.Action OnVictory;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        PlayerHP = startingHP;
        Coins = startingCoins + SaveSystem.LoadCoins();
        Gems = startingGems + SaveSystem.LoadGems();
        IsGameOver = false;

        OnHPChanged?.Invoke(PlayerHP);
        OnCoinsChanged?.Invoke(Coins);
        OnGemsChanged?.Invoke(Gems);
    }

    public void DamageBase(int amount)
    {
        if (IsGameOver) return;
        PlayerHP = Mathf.Max(0, PlayerHP - amount);
        OnHPChanged?.Invoke(PlayerHP);

        if (PlayerHP <= 0)
        {
            IsGameOver = true;
            int best = SaveSystem.LoadBestWave();
            int current = WaveManager.Instance != null ? WaveManager.Instance.CurrentWave : 0;
            if (current > best) SaveSystem.SaveBestWave(current);
            OnGameOver?.Invoke();
        }
    }

    public bool SpendCoins(int amount)
    {
        if (Coins < amount) return false;
        Coins -= amount;
        OnCoinsChanged?.Invoke(Coins);
        SaveSystem.SaveCoins(Coins);
        return true;
    }

    public bool SpendGems(int amount)
    {
        if (Gems < amount) return false;
        Gems -= amount;
        OnGemsChanged?.Invoke(Gems);
        SaveSystem.SaveGems(Gems);
        return true;
    }

    public void AddCoins(int amount)
    {
        Coins += amount;
        CoinsEarned += amount;
        OnCoinsChanged?.Invoke(Coins);
        SaveSystem.SaveCoins(Coins);
    }

    public void AddGems(int amount)
    {
        Gems += amount;
        OnGemsChanged?.Invoke(Gems);
        SaveSystem.SaveGems(Gems);
    }

    public void RegisterEnemyDefeated()
    {
        EnemiesDefeated++;
    }

    public void TriggerVictory()
    {
        if (IsGameOver) return;
        IsGameOver = true;
        int best = SaveSystem.LoadBestWave();
        int current = WaveManager.Instance != null ? WaveManager.Instance.CurrentWave : 0;
        if (current > best) SaveSystem.SaveBestWave(current);
        OnVictory?.Invoke();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}