using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopUI : MonoBehaviour
{
    [System.Serializable]
    public class ShopItem
    {
        public TowerData towerData;
        public Button buyButton;
        public TextMeshProUGUI statusText;
    }

    public ShopItem[] items;

    void OnEnable() => RefreshUI();

    void RefreshUI()
    {
        foreach (var item in items)
        {
            bool unlocked = !item.towerData.lockedByDefault
                            || SaveSystem.IsTowerUnlocked(item.towerData.towerName);

            if (item.statusText) item.statusText.text = unlocked ? "Owned" : "Buy";

            var td = item.towerData; // capture for lambda
            item.buyButton.onClick.RemoveAllListeners();
            item.buyButton.interactable = !unlocked;

            if (!unlocked)
            {
                item.buyButton.onClick.AddListener(() =>
                {
                    bool paid = td.gemCost > 0
                        ? GameManager.Instance.SpendGems(td.gemCost)
                        : GameManager.Instance.SpendCoins(td.coinCost);

                    if (paid)
                    {
                        SaveSystem.UnlockTower(td.towerName);
                        SaveSystem.Save();
                        RefreshUI();
                        UIManager.Instance.ShowMessage($"{td.towerName} Unlocked!");
                    }
                    else
                    {
                        UIManager.Instance.ShowMessage("Not enough currency!");
                    }
                });
            }
        }
    }
}
