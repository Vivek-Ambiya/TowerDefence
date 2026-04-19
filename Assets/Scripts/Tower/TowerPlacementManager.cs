using UnityEngine;

public class TowerPlacementManager : MonoBehaviour
{
    public static TowerPlacementManager Instance { get; private set; }

    private TowerData selectedTowerData;

    void Awake() => Instance = this;

    public void SelectTower(TowerData data)
    {
        selectedTowerData = data;
    }

    public void DeselectTower()
    {
        selectedTowerData = null;
    }

    public void TryPlaceTower(TowerSpot spot)
    {
        if (selectedTowerData == null)
        {
            Debug.Log("No tower selected.");
            return;
        }

        if (selectedTowerData.lockedByDefault && !SaveSystem.IsTowerUnlocked(selectedTowerData.towerName))
        {
            UIManager.Instance.ShowMessage("Tower not unlocked! Visit the Shop.");
            return;
        }

        bool paid = false;
        if (selectedTowerData.gemCost > 0)
            paid = GameManager.Instance.SpendGems(selectedTowerData.gemCost);
        else
            paid = GameManager.Instance.SpendCoins(selectedTowerData.coinCost);

        if (!paid)
        {
            UIManager.Instance.ShowMessage("Not enough currency!");
            return;
        }

        var go = Instantiate(selectedTowerData.prefab, spot.transform.position, Quaternion.identity);
        var tower = go.GetComponent<Tower>();
        tower.Initialize(selectedTowerData);
        spot.PlaceTower(tower);

        AudioManager.Instance?.PlaySFX("place");
        DeselectTower();
    }
}