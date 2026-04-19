using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    const string KEY_COINS = "SavedCoins";
    const string KEY_GEMS = "SavedGems";
    const string KEY_BEST_WAVE = "BestWave";
    const string KEY_TOWER_UNLOCKED = "TowerUnlocked";

    public static void SaveCoins(int v) => PlayerPrefs.SetInt(KEY_COINS, v);
    public static int LoadCoins() => PlayerPrefs.GetInt(KEY_COINS, 0);

    public static void SaveGems(int v) => PlayerPrefs.SetInt(KEY_GEMS, v);
    public static int LoadGems() => PlayerPrefs.GetInt(KEY_GEMS, 0);

    public static void SaveBestWave(int v) => PlayerPrefs.SetInt(KEY_BEST_WAVE, v);
    public static int LoadBestWave() => PlayerPrefs.GetInt(KEY_BEST_WAVE, 0);

    public static void UnlockTower(string name) => PlayerPrefs.SetInt(KEY_TOWER_UNLOCKED + name, 1);
    public static bool IsTowerUnlocked(string name) => PlayerPrefs.GetInt(KEY_TOWER_UNLOCKED + name, 0) == 1;

    public static void Save() => PlayerPrefs.Save();
}